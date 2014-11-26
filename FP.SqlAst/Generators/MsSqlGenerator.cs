namespace FP.SqlAst.Generators {
    using System.Collections.Generic;
    using System.Text;

    public class MsSqlGenerator {
        public string Generate(SelectQuery query, Dictionary<string, object> parameters) {
            var visitor = new Visitor(parameters);
                        
            query.Accept(visitor);

            return visitor.QueryString;
        }

        public class Visitor : SqlAstVisitorBase {
            private readonly StringBuilder builder;

            public Dictionary<string, object> Parameters { get; private set; }

            public string QueryString {
                get { return this.builder.ToString(); }
            }

            public Visitor(Dictionary<string, object> parameters) {
                this.Parameters = parameters;
                this.builder = new StringBuilder();
            }

            public override void VisitQueryProjection(IEnumerable<Column> columns) {
                this.builder.Append("SELECT ");

                this.WriteColumnNames(columns);
            }

            private void WriteColumnNames(IEnumerable<Column> columns) {
                foreach (var column in columns) {
                    column.Accept(this);
                    this.builder.Append(", ");
                }

                this.builder.Remove(this.builder.Length - 2, 2);
            }

            public override void VisitQueryFrom(RecordSet @from) {
                this.builder.Append(" FROM ");

                var usingSubquery = @from is SelectQuery;

                if (usingSubquery) {
                    this.builder.Append("(");
                }

                from.Accept(this);

                if (usingSubquery) {
                    this.builder.Append(") as x");
                }
            }

            public override void VisitTable(Table element) {
                this.builder.Append(element.Name);
            }

            public override void VisitAllColumns(AllColumns element) {
                this.builder.Append("*");
            }

            public override void VisitNamedColumn(NamedColumn element) {
                this.builder.Append("[");
                this.builder.Append(element.Name);
                this.builder.Append("]");
            }

            public override void VisitRowNumberColumn(RowNumberColumn rowNumberColumn) {
                this.builder.Append("ROW_NUMBER() OVER (ORDER BY ");

                this.WriteColumnNames(rowNumberColumn.OrderBy);

                this.builder.Append(") AS ");
                this.builder.Append(rowNumberColumn.ColumnName);
            }

            public override void VisitQueryCondition(Condition condition) {
                this.builder.Append(" WHERE ");

                condition.Accept(this);
            }

            public override void VisitBetween(BetweenCondition betweenCondition) {
                betweenCondition.Value.Accept(this);

                this.builder.Append(" BETWEEN ");

                betweenCondition.Left.Accept(this);

                this.builder.Append(" AND ");

                betweenCondition.Right.Accept(this);
            }

            public override void VisitColumnReference(ColumnReference columnReference) {
                this.builder.Append("[");
                this.builder.Append(columnReference.ColumnName);
                this.builder.Append("]");
            }

            public override void VisitConstant(ConstantValue constantValue) {
                var paramName = "@p" + this.Parameters.Keys.Count;
                this.Parameters.Add(paramName, constantValue.Value);

                this.builder.Append(paramName);
            }

            public override void VisitBinaryCondition(BinaryCondition binaryCondition) {
                binaryCondition.Left.Accept(this);
                
                this.builder.Append(" ").Append(binaryCondition.Operator).Append(" ");

                binaryCondition.Right.Accept(this);
            }
        }
    }
}