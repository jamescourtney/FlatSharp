using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using FlatSharp;

namespace BenchmarkCore
{
    [ShortRunJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50, BenchmarkDotNet.Environments.Jit.RyuJit, BenchmarkDotNet.Environments.Platform.AnyCpu)]
    public class MeshServerUseCases
    {
        [Params(40)]
        public byte RegionSize;

        [Params(0.40f)] 
        public float MeshFillPercent;

        [Params( 10)] public byte ClientsInChangeRadius;

        [Params(FlatBufferDeserializationOption.GreedyMutable, FlatBufferDeserializationOption.Lazy)] public FlatBufferDeserializationOption ParseOption { get; set; }

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
            this.serializer = new FlatBufferSerializer(this.ParseOption);

            regionSizeCubed = RegionSize * RegionSize * RegionSize;
            regionSizeSquared = RegionSize * RegionSize;

            fillSize = (int)(MeshFillPercent * regionSizeCubed);

            var region = new VoxelRegion3D();
            region.size = RegionSize;
            region.voxels = new List<Voxel>(regionSizeCubed);
            region.iteration = 0;
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

            var mesh = FakeGenerateMeshOnCPU(rnd, region);

            SaveToDisk(mesh);
        }

        private Mesh FakeGenerateMeshOnCPU(Random rnd, VoxelRegion3D voxelRegion3D)
        {
            var mesh = new Mesh();
            mesh.vertices = new List<Vector3>(fillSize);
            mesh.normals = new List<Vector3>(fillSize);
            mesh.color = new List<Color>(fillSize);
            mesh.uv = new List<Vector2>(fillSize);
            mesh.triangles = new List<ushort>(fillSize);
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
                        mesh.vertices.Add(new Vector3 { x = rnd.Next(), y = rnd.Next(), z = rnd.Next() });

                        mesh.normals.Add(new Vector3 { x = rnd.Next(), y = rnd.Next(), z = rnd.Next() });

                        mesh.color.Add(new Color
                        {
                            r = adjacentVoxels[0].VoxelType,
                            g = adjacentVoxels[0].SubType,
                            b = adjacentVoxels[0].Hp,
                            a = adjacentVoxels[0].Unused,
                        });

                        mesh.uv.Add(new Vector2 { x = 0f, y = 1f });

                        mesh.triangles.Add((ushort)filled);
                        filled++;
                    }
                }
            }

            return mesh;
        }

        private void FakeGrpcStreamRegionToClient(VoxelRegion3D region)
        {
            var maxSize = this.serializer.GetMaxSize(region);
            var networkBuffer = new byte[maxSize];
            this.serializer.Serialize(region, networkBuffer);
        }
        private void FakeGrpcStreamMeshToClient(Mesh region)
        {
            var maxSize = this.serializer.GetMaxSize(region);
            var networkBuffer = new byte[maxSize];
            this.serializer.Serialize(region, networkBuffer);
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

            // simulate region data change
            var toModify = deserializedRegion.voxels[rnd.Next(0, deserializedRegion.voxels.Count - 1)];
            var newList = deserializedRegion.voxels.ToList();
            newList[rnd.Next(0, deserializedRegion.voxels.Count - 1)] =
                new Voxel(toModify)
                {
                    VoxelType = (byte)rnd.Next(1)
                };
            var modifiedRegion = new VoxelRegion3D(deserializedRegion);
            modifiedRegion.iteration++;
            modifiedRegion.voxels = newList;

            // generate new mesh
            var mesh = FakeGenerateMeshOnCPU(rnd, modifiedRegion);

            SaveToDisk(mesh);
            SaveToDisk(deserializedRegion);

            for (int i = 0; i < ClientsInChangeRadius; i++)
            {
                FakeGrpcStreamMeshToClient(mesh);
            }
        }

        private int SaveToDisk(VoxelRegion3D deserializedRegion)
        {
            FakeDiskStoredRegion = new byte[this.serializer.GetMaxSize(deserializedRegion)];
            return this.serializer.Serialize(deserializedRegion, FakeDiskStoredRegion);
        }

        private void SaveToDisk(Mesh mesh)
        {
            FakeDiskStoredMesh = new byte[this.serializer.GetMaxSize(mesh)];
            this.serializer.Serialize(mesh, FakeDiskStoredMesh);
        }
    }
}