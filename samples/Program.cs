namespace Samples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            MonsterAttributeExample.MonsterAttributeExample.Run();
            SerializerOptions.SerializerOptionsExample.Run();
            SchemaFilesExample.SchemaFilesExample.Run();
            SchemaFilesExample2.SchemaFilesExample2.Run();
            GrpcExample.GrpcExample.Run();
            CopyConstructorsExample.CopyConstructorsExample.Run();
            IncludesExample.IncludesExample.Run();
            SortedVectors.SortedVectorsExample.Run();
            Unions.UnionsExample.Run();
            SharedStrings.SharedStringsExample.Run();
            IndexedVectors.IndexedVectorsExample.Run();
            TypeFacades.TypeFacadesExample.Run();
            StructVectors.StructVectorsSample.Run();

            int i = 3;
            Bar bar = new Bar();
            if (i is IFoo foo)
            {

            }

            if (bar is IFoo foo2)
            {

            }
        }
    }

    public interface IFoo { }

    public class Bar { }
}
