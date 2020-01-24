namespace FlatSharp.Compiler
{
    using System;
    using System.IO;
    using System.Linq;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;

    internal class SchemaVisitor : FlatBuffersBaseVisitor<SchemaDefinition>
    {
        private readonly SchemaDefinition schemaDefinition = new SchemaDefinition();

        public override SchemaDefinition VisitSchema([NotNull] FlatBuffersParser.SchemaContext context)
        {
            string namespaceName;
            {
                string[] namespaces = context.namespace_decl()?.Select(ns => new NamespaceVisitor().VisitNamespace_decl(ns)).ToArray();
                if (namespaces?.Length != 1)
                {
                    ErrorContext.Current?.RegisterError("FlatBuffer FBS schema must declare a single namespace.");
                    return null;
                }

                namespaceName = namespaces[0];
            }

            var schema = new SchemaDefinition
            {
                NamespaceName = namespaceName
            };

            ErrorContext.Current.WithScope(namespaceName, () =>
            {
                foreach (var union in context.union_decl()?.Select(x => new UnionVisitor(namespaceName).VisitUnion_decl(x)).ToArray() ?? new UnionDefinition[0])
                {
                    schema.AddType(union);
                }

                foreach (var @enum in context.enum_decl()?.Select(x => new EnumVisitor(namespaceName).VisitEnum_decl(x)).ToArray() ?? new EnumDefinition[0])
                {
                    schema.AddType(@enum);
                }

                foreach (var tableOrStruct in context.type_decl()?.Select(x => new TypeVisitor(namespaceName).VisitType_decl(x)).ToArray() ?? new TableOrStructDefinition[0])
                {
                    schema.AddType(tableOrStruct);
                }

                // Once we've iterated over everything, we can figure out indexes in the tables and structs.
                foreach (var item in schema.Types)
                {
                    if (item.Value is TableOrStructDefinition tableOrStruct)
                    {
                        tableOrStruct.AssignIndexes(schema);
                    }
                }
            });

            return schema;
        }

        public override SchemaDefinition VisitNamespace_decl([NotNull] FlatBuffersParser.Namespace_declContext context)
        {
            return base.VisitNamespace_decl(context);
        }

        public override SchemaDefinition VisitType_decl([NotNull] FlatBuffersParser.Type_declContext context)
        {
            return base.VisitType_decl(context);
        }

        public override SchemaDefinition VisitEnum_decl([NotNull] FlatBuffersParser.Enum_declContext context)
        {
            return base.VisitEnum_decl(context);
        }

        public override SchemaDefinition VisitUnion_decl([NotNull] FlatBuffersParser.Union_declContext context)
        {
            return base.VisitUnion_decl(context);
        }
    }

    internal class NamespaceVisitor : FlatBuffersBaseVisitor<string>
    {
        public override string VisitNamespace_decl([NotNull] FlatBuffersParser.Namespace_declContext context)
        {
            var ident = string.Join(".", context.IDENT().Select(x => x.Symbol.Text));
            return ident;
        }
    }
}
