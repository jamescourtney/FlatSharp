namespace FlatSharp.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// Defines an enum.
    /// </summary>
    internal class EnumDefinition : ITypeDefinition
    {
        private BigInteger nextValue = 1;
        private readonly Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

        public EnumDefinition(string typeName, string underlyingTypeName, string ns)
        {
            this.TypeName = typeName;
            this.FbsUnderlyingType = underlyingTypeName;
            this.Namespace = ns;
        }

        public string TypeName { get; }

        public string Namespace { get; }

        public string FbsUnderlyingType { get; }

        public string ClrUnderlyingType { get; }

        public IReadOnlyDictionary<string, string> NameValuePairs => this.nameValuePairs;

        public void AddNameValuePair(string name, string value)
        {
            string clrType = SchemaDefinition.ResolvePrimitiveType(this.FbsUnderlyingType);

            if (!string.IsNullOrEmpty(value))
            {
                value = CodeWriter.GetPrimitiveTypeLiteral(clrType, value, out BigInteger bigInt);

                // C# compiler won't complain about duplicate values.
                if (this.nameValuePairs.Values.Any(x => x == value))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.TypeName}' contains duplicate value '{value}'.");
                }

                this.nameValuePairs[name] = value;
                this.nextValue = bigInt + 1;
            }
            else
            {
                value = this.nextValue.ToString();
                this.nameValuePairs[name] = CodeWriter.GetPrimitiveTypeLiteral(clrType, value, out BigInteger bigInt);
                this.nextValue++;
            }
        }

        public void WriteType(CodeWriter writer, SchemaDefinition schemaDefinition)
        {
            ErrorContext.Current.WithScope(
                this.TypeName,
                () =>
                {
                    string underlyingType = SchemaDefinition.ResolvePrimitiveType(this.FbsUnderlyingType);
                    writer.AppendLine($"[FlatBufferEnum(typeof({underlyingType}))]");
                    writer.AppendLine($"public enum {this.TypeName} : {underlyingType}");
                    writer.AppendLine($"{{");
                    using (writer.IncreaseIndent())
                    {
                        foreach (var item in this.nameValuePairs)
                        {
                            writer.AppendLine($"{item.Key} = {item.Value},");
                        }
                    }
                    writer.AppendLine($"}}");
                    writer.AppendLine(string.Empty);
                });
        }
    }
}
