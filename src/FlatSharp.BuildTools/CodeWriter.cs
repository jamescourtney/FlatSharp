namespace FlatSharp.Compiler
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Utility class for generating reasonably formatted code.
    /// </summary>
    internal class CodeWriter
    {
        private const string OneIndent = "    ";
        private int indent;
        private readonly StringBuilder builder = new StringBuilder();

        public void AppendLine(string line)
        {
            for (int i = 0; i < this.indent; ++i)
            {
                this.builder.Append(OneIndent);
            }

            this.builder.AppendLine(line);
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }

        public static string GetPrimitiveTypeLiteral(string type, string value)
        {
            return GetPrimitiveTypeLiteral(type, value, out _);
        }

        public static string GetPrimitiveTypeLiteral(string type, string value, out BigInteger bigInt)
        {
            bigInt = default;
            Func<string, (bool, string, BigInteger)> tryParseAndFormat;

            switch (type)
            {
                case "bool":
                    tryParseAndFormat = str =>
                    {
                        if (str == "false" || str == "true")
                        {
                            return (true, str, default);
                        }

                        return (false, null, default);
                    };
                    break;

                case "byte":
                    tryParseAndFormat = CreateTryParseAndFormat<byte>(byte.TryParse, cast: "byte");
                    break;

                case "sbyte":
                    tryParseAndFormat = CreateTryParseAndFormat<sbyte>(sbyte.TryParse, cast: "sbyte");
                    break;

                case "short":
                    tryParseAndFormat = CreateTryParseAndFormat<short>(short.TryParse, cast: "short");
                    break;

                case "ushort":
                    tryParseAndFormat = CreateTryParseAndFormat<ushort>(ushort.TryParse, cast: "ushort");
                    break;

                case "int":
                    tryParseAndFormat = CreateTryParseAndFormat<int>(int.TryParse);
                    break;

                case "uint":
                    tryParseAndFormat = CreateTryParseAndFormat<uint>(uint.TryParse, suffix: "u");
                    break;

                case "long":
                    tryParseAndFormat = CreateTryParseAndFormat<long>(long.TryParse, suffix: "L");
                    break;

                case "ulong":
                    tryParseAndFormat = CreateTryParseAndFormat<ulong>(ulong.TryParse, suffix: "ul");
                    break;

                case "float":
                    tryParseAndFormat = CreateTryParseAndFormat<float>(float.TryParse, suffix: "f");
                    break;

                case "double":
                    tryParseAndFormat = CreateTryParseAndFormat<double>(double.TryParse, suffix: "d");
                    break;

                default:
                    ErrorContext.Current?.RegisterError($"Unrecognized literal type: '{type}'");
                    tryParseAndFormat = s => (false, null, default);
                    break;
            }

            bool result;
            string formatted;
            (result, formatted, bigInt) = tryParseAndFormat(value);

            if (!result)
            {
                ErrorContext.Current?.RegisterError($"Unable to parse literal '{value}' as a {type}.");
            }

            return formatted;
        }

        private delegate bool TryParseDelegate<T>(string value, NumberStyles numberStyle, IFormatProvider formatProvider, out T result);

        private static Func<string, (bool, string, BigInteger)> CreateTryParseAndFormat<T>(TryParseDelegate<T> tryParse, string suffix = "", string cast = "")
        {
            return str =>
            {
                bool success = false;
                BigInteger bigInt = default;

                if (tryParse(str, NumberStyles.Integer, null, out T result))
                {
                    success = true;
                    bigInt = BigInteger.Parse(str, NumberStyles.Integer, null);
                }
                else if (tryParse(str, NumberStyles.Float, null, out result))
                {
                    success = true;
                }
                else
                {
                    // Hex parsing is broken in .NET. Strip off the sign, parse as either long or ulong hex depending on sign, convert to decimal, and then reparse for our given format.
                    // Not fast, but works well enough.
                    bool negative = str.StartsWith("-");
                    if (negative || str.StartsWith("+"))
                    {
                        str = str.Substring(1);
                    }

                    if (str.StartsWith("0x") || str.StartsWith("0X"))
                    {
                        str = str.Substring(2);
                        if (BigInteger.TryParse(str, NumberStyles.HexNumber, null, out bigInt))
                        {
                            if (negative)
                            {
                                bigInt *= -1;
                            }

                            success = tryParse(bigInt.ToString(), NumberStyles.Integer, null, out result);
                        }
                    }
                }

                string formatted = result.ToString();
                if (!string.IsNullOrEmpty(cast))
                {
                    formatted = $"({cast}){formatted}";
                }

                if (!string.IsNullOrEmpty(suffix))
                {
                    formatted = $"{formatted}{suffix}";
                }

                return (success, formatted, bigInt);
            };
        }

        public IDisposable IncreaseIndent()
        {
            this.indent++;
            return new FakeDisposable(() => this.indent--);
        }

        private class FakeDisposable : IDisposable
        {
            private readonly Action onDispose;

            public FakeDisposable(Action onDispose)
            {
                this.onDispose = onDispose;
            }

            public void Dispose()
            {
                this.onDispose();
            }
        }
    }
}
