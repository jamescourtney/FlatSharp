namespace FlatSharp.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// Defines an enum.
    /// </summary>
    internal class EnumDefinition : BaseSchemaMember
    {
        private BigInteger nextValue = 1;
        private readonly Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

        public EnumDefinition(string typeName, string underlyingTypeName, BaseSchemaMember parent)
            : base(typeName, parent)
        {
            this.FbsUnderlyingType = underlyingTypeName;
            this.ClrUnderlyingType = SchemaDefinition.ResolvePrimitiveType(underlyingTypeName);
        }

        protected override bool SupportsChildren => false;

        public string FbsUnderlyingType { get; }

        public string ClrUnderlyingType { get; }

        public IReadOnlyDictionary<string, string> NameValuePairs => this.nameValuePairs;

        public void AddNameValuePair(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = CodeWriter.GetPrimitiveTypeLiteral(this.ClrUnderlyingType, value, out BigInteger bigInt);

                // C# compiler won't complain about duplicate values.
                if (this.nameValuePairs.Values.Any(x => x == value))
                {
                    ErrorContext.Current?.RegisterError($"Enum '{this.Name}' contains duplicate value '{value}'.");
                }

                this.nameValuePairs[name] = value;
                this.nextValue = bigInt + 1;
            }
            else
            {
                value = this.nextValue.ToString();
                this.nameValuePairs[name] = CodeWriter.GetPrimitiveTypeLiteral(this.ClrUnderlyingType, value, out BigInteger bigInt);
                this.nextValue++;
            }
        }

        protected override void OnWriteCode(CodeWriter writer)
        {
            writer.AppendLine($"[FlatBufferEnum(typeof({this.ClrUnderlyingType}))]");
            writer.AppendLine($"public enum {this.Name} : {this.ClrUnderlyingType}");
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
        }
    }
}
