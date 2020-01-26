/*
 * Copyright 2020 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace FlatSharp.Compiler
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;

    public static class FlatSharpCompiler
    {
        static int Main(string[] args)
        {
            using (var context = ErrorContext.Current)
            {
                try
                {
                    context.PushScope("$");
                    string cSharp = CreateCSharp(File.ReadAllText(args[0]));
                    File.WriteAllText(args[0] + ".generated.cs", cSharp);
                }
                catch (InvalidFbsFileException ex)
                {
                    foreach (var message in ex.Errors)
                    {
                        Console.Error.WriteLine(message);
                    }

                    return -1;
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine($"File '{args[0]}' was not found");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.Error.WriteLine($"No file specified");
                }
                finally
                {
                    context.PopScope();
                }

                return 0;
            }
        }

        public static Assembly CompileAndLoadAssembly(string fbsSchema)
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
            ErrorContext.Current?.RegisterError($"Syntax error FBS file: Token='{offendingSymbol.Text}', Msg='{msg}' Line='{line}:{charPositionInLine}");
        }
    }
}
