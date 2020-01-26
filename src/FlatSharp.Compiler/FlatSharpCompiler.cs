using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlatSharp.Compiler
{
    public static class FlatSharpCompiler
    {
        static void Main(string[] args)
        {
            using (var context = ErrorContext.Current)
            {
                context.PushScope(".schema");

                string text = File.ReadAllText(@"C:\Users\jcour\source\repos\jamescourtney\FlatSharp\src\Benchmark\FBBench\Google.Flatbuffers.fbs");
                string cSharp = CreateCSharp(text);
                Console.Write(cSharp);

                context.PopScope();
            }
        }

        internal static Assembly CompileAndLoadAssembly(string fbsSchema)
        {
            using (var context = ErrorContext.Current)
            {
                context.PushScope("$");
                try
                {
                    string cSharp = CreateCSharp(fbsSchema);
                    var (assembly, formattedText, _) = RoslynSerializerGenerator.CompileAssembly(cSharp, true);
                    string debugText = formattedText();
                    return assembly;
                }
                finally
                {
                    context.PopScope();
                }
            }
        }

        internal static string CreateCSharp(string fbsSchema)
        {
            AntlrInputStream input = new AntlrInputStream(fbsSchema);
            FlatBuffersLexer lexer = new FlatBuffersLexer(input);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            FlatBuffersParser parser = new FlatBuffersParser(tokenStream);

            parser.AddErrorListener(new CustomErrorListener());

            SchemaVisitor visitor = new SchemaVisitor();
            BaseSchemaMember rootNode = visitor.Visit(parser.schema());

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            CodeWriter writer = new CodeWriter();

            rootNode.WriteCode(writer);

            if (ErrorContext.Current.Errors.Any())
            {
                throw new InvalidFbsFileException(ErrorContext.Current.Errors);
            }

            return writer.ToString();
        }
    }

    internal class CustomErrorListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(
            [NotNull] IRecognizer recognizer, 
            [Nullable] IToken offendingSymbol, 
            int line, 
            int charPositionInLine, 
            [NotNull] string msg, 
            [Nullable] RecognitionException e)
        {
            ErrorContext.Current?.RegisterError($"Syntax error in FBS file: Token='{offendingSymbol.Text}', Msg='{msg}'.");
        }
    }
}
