using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using FlatSharp;

namespace BenchmarkCore.Modified
{
    [ShortRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class MeshServerModifiedUseCases
    {
        [Params(40)]
        public byte RegionSize = 40;

        [Params(0.40f)]
        public float MeshFillPercent = 0.4f;

        [Params(10)] public byte ClientsInChangeRadius = 10;

        public byte[] FakeDiskStoredMesh;
        public byte[] FakeDiskStoredRegion;
        private int fillSize;
        private int regionSizeCubed;
        private int regionSizeSquared;
        private Random rnd;

        private FlatBufferSerializer serializer;

        [GlobalSetup]
        public void Setup()
        {
            this.serializer = new FlatBufferSerializer(FlatBufferDeserializationOption.VectorCacheMutable);

            regionSizeCubed = RegionSize * RegionSize * RegionSize;
            regionSizeSquared = RegionSize * RegionSize;

            fillSize = (int)(MeshFillPercent * regionSizeCubed);

            var region = new VoxelRegion3D();
            region.size = RegionSize;
            region.voxels = new List<Voxel>(regionSizeCubed);
            region.iteration = new MutableUInt32 { Value = 0 };
            region.location = new Vector3Int();
            rnd = new Random(0);
            for (int i = 0; i < regionSizeCubed; i++)
            {
                region.voxels.Add(new Voxel
                {
                    VoxelType = (byte)Math.Clamp(rnd.Next(-255, 255), 0, 255),
                    SubType = (byte)rnd.Next(255),
                    Hp = (byte)rnd.Next(255),
                    Unused = (byte)rnd.Next(255)
                });
            }

            SaveToDisk(region);

            // Update fillSize to accomodate max. Some items may be null.
            Mesh mesh = new Mesh
            {
                color = new Color[fillSize * 3],
                normals = new Vector3[fillSize * 3],
                triangles = new ushort[fillSize * 3],
                uv = new Vector2[fillSize * 3],
                vertices = new Vector3[fillSize * 3]
            };

            UpdateMeshInPlace(rnd, region, mesh);
            SaveToDisk(mesh);
        }

        private Mesh UpdateMeshInPlace(Random rnd, VoxelRegion3D voxelRegion3D, Mesh mesh)
        {
            int filled = 0;
            for (int i = 0; i < regionSizeCubed - (regionSizeSquared + RegionSize + 1); i++)
            {
                var adjacentVoxels = new[]
                {
                    voxelRegion3D.voxels[i],
                    voxelRegion3D.voxels[i + 1],
                    voxelRegion3D.voxels[i + RegionSize],
                    voxelRegion3D.voxels[i + RegionSize + 1],
                    voxelRegion3D.voxels[regionSizeSquared + i],
                    voxelRegion3D.voxels[regionSizeSquared + i+ 1],
                    voxelRegion3D.voxels[regionSizeSquared + i + RegionSize],
                    voxelRegion3D.voxels[regionSizeSquared + i + RegionSize + 1],
                };

                byte meshMask = 0;
                for (int j = 0; j < adjacentVoxels.Length; j++)
                {
                    meshMask |= (byte)(Math.Min(adjacentVoxels[j].VoxelType, (byte)1) << j);
                }

                if (meshMask is > 0 and < 255 && filled < fillSize)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var vertex = (mesh.vertices[filled] ??= new Vector3());
                        vertex.x = rnd.Next();
                        vertex.y = rnd.Next();
                        vertex.z = rnd.Next();
                        vertex.Null = 0;

                        var normal = (mesh.normals[filled] ??= new Vector3());
                        normal.x = rnd.Next();
                        normal.y = rnd.Next();
                        normal.z = rnd.Next();
                        normal.Null = 0;

                        var color = (mesh.color[filled] ??= new Color());
                        color.r = adjacentVoxels[0].VoxelType;
                        color.g = adjacentVoxels[0].SubType;
                        color.b = adjacentVoxels[0].Hp;
                        color.a = adjacentVoxels[0].Unused;
                        color.Null = 0;

                        var uv = (mesh.uv[filled] ??= new Vector2());
                        uv.x = 0f;
                        uv.y = 1f;
                        uv.Null = 0;

                        mesh.triangles[filled] = (ushort)filled;
                        filled++;
                    }
                }
            }

            for (; filled < mesh.color.Length; ++filled)
            {
                var vertex = (mesh.vertices[filled] ??= new Vector3());
                vertex.Null = 1;

                var normal = (mesh.normals[filled] ??= new Vector3());
                normal.Null = 1;

                var uv = (mesh.uv[filled] ??= new Vector2());
                uv.Null = 1;

                var color = (mesh.color[filled] ??= new Color());
                color.Null = 1;

                mesh.triangles[filled] = (ushort)filled;
            }

            return mesh;
        }

        private void FakeGrpcStreamRegionToClient(VoxelRegion3D region)
        {
            if (region is IFlatBufferDeserializedObject deserialized)
            {
                StreamDeserializedObjectToClient(deserialized);
            }
            else
            {
                var maxSize = this.serializer.GetMaxSize(region);
                var networkBuffer = new byte[maxSize];
                this.serializer.Serialize(region, networkBuffer);
            }
        }

        private void FakeGrpcStreamMeshToClient(Mesh region)
        {
            if (region is IFlatBufferDeserializedObject deserialized)
            {
                StreamDeserializedObjectToClient(deserialized);
            }
            else
            {
                var maxSize = this.serializer.GetMaxSize(region);
                var networkBuffer = new byte[maxSize];
                this.serializer.Serialize(region, networkBuffer);
            }
        }

        private void StreamDeserializedObjectToClient(IFlatBufferDeserializedObject region)
        {
            var length = region.InputBuffer.Length;
            var networkBuffer = new byte[length];
            region.InputBuffer.GetByteMemory(0, length).CopyTo(networkBuffer);
        }

        [Benchmark]
        public void SendRegionToClient()
        {
            var regionFromStorage = FakeDiskStoredRegion;
            var deserializedRegion = this.serializer.Parse<VoxelRegion3D>(regionFromStorage);
            FakeGrpcStreamRegionToClient(deserializedRegion);
        }

        [Benchmark]
        public void SendVisibleRegionsToClient()
        {
            // Assume visible Regions to be 20 in each direction
            for (int x = 0; x < 20; x++)
            {
                for (int z = 0; z < 20; z++)
                {
                    var regionFromStorage = FakeDiskStoredRegion;
                    var deserializedRegion = this.serializer.Parse<VoxelRegion3D>(regionFromStorage);
                    FakeGrpcStreamRegionToClient(deserializedRegion);
                }
            }
        }

        [Benchmark]
        public void SendVisibleMeshesToClient()
        {
            // Assume visible meshes to be 20 in each direction
            for (int x = 0; x < 20; x++)
            {
                for (int z = 0; z < 20; z++)
                {
                    var regionFromStorage = FakeDiskStoredRegion;
                    var deserializedRegion = this.serializer.Parse<VoxelRegion3D>(regionFromStorage);
                    FakeGrpcStreamRegionToClient(deserializedRegion);
                }
            }
        }

        [Benchmark]
        public void SendMeshToClient()
        {
            var meshFromStorage = FakeDiskStoredMesh;
            var deserializedMesh = this.serializer.Parse<Mesh>(meshFromStorage);
            FakeGrpcStreamMeshToClient(deserializedMesh);
        }

        [Benchmark]
        public void ModifyMeshAndSendToClients()
        {
            var regionFromStorage = FakeDiskStoredRegion;
            var deserializedRegion = this.serializer.Parse<VoxelRegion3D>(regionFromStorage);

            Mesh mesh = this.serializer.Parse<Mesh>(FakeDiskStoredMesh);

            // simulate region data change
            deserializedRegion.voxels[rnd.Next(0, deserializedRegion.voxels.Count - 1)].VoxelType = (byte)rnd.Next(1);
            deserializedRegion.iteration.Value++;

            // generate new mesh
            UpdateMeshInPlace(rnd, deserializedRegion, mesh);

            // Not needed -- assuming we are using mmapped input.
            // Otherwise, we just need to write the full buffer back, but there is no serialization overhead there -- just file I/O.
            // SaveToDisk(mesh); 
            // SaveToDisk(deserializedRegion);

            for (int i = 0; i < ClientsInChangeRadius; i++)
            {
                // Not really needed. We can just memcpy the modified buffer.
                FakeGrpcStreamMeshToClient(mesh); 
            }
        }

        private void SaveToDisk(VoxelRegion3D deserializedRegion)
        {
            FakeDiskStoredRegion = new byte[this.serializer.GetMaxSize(deserializedRegion)];
            int bytesWritten = this.serializer.Serialize(deserializedRegion, FakeDiskStoredRegion);

            FakeDiskStoredRegion = FakeDiskStoredRegion.AsSpan().Slice(0, bytesWritten).ToArray();
        }

        private void SaveToDisk(Mesh mesh)
        {
            FakeDiskStoredMesh = new byte[this.serializer.GetMaxSize(mesh)];
            int bytesWritten = this.serializer.Serialize(mesh, FakeDiskStoredMesh);

            FakeDiskStoredMesh = FakeDiskStoredMesh.AsSpan().Slice(0, bytesWritten).ToArray();
        }
    }
}