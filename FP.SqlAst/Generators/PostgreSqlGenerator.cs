namespace FP.SqlAst.Generators {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PostgreSqlGenerator {
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
                    this.builder.Append(") as \"" + @from.Alias + "\"");
                }
            }

            public override void VisitTable(Table element) {
                this.builder.Append("\"" + element.Name + "\"");
            }

            public override void VisitAllColumns(AllColumns element) {
                this.builder.Append("*");
            }

            public override void VisitNamedColumn(NamedColumn element) {
                if (element.Base != null)
                {
                    this.builder.Append("\"");
                    this.builder.Append(element.Base);
                    this.builder.Append("\".");
                }

                this.builder.Append("\"");
                this.builder.Append(element.Name);
                this.builder.Append("\"");
            }

            public override void VisitRowNumberColumn(RowNumberColumn rowNumberColumn) {
                throw new NotSupportedException("RowNumberColumn not supported");
            }

            public override void VisitQueryCondition(Condition condition) {
                this.builder.Append(" WHERE ");

                condition.Accept(this);
            }

            public override void VisitGroupBy(List<SqlAstElement> groupBy)
            {
                this.builder.Append(" GROUP BY ");

                foreach (var element in groupBy)
                {
                    element.Accept(this);
                    this.builder.Append(", ");
                }

                this.builder.Remove(this.builder.Length - 2, 2);
            }

            public override void VisitBetween(BetweenCondition betweenCondition) {
                betweenCondition.Value.Accept(this);

                this.builder.Append(" BETWEEN ");

                betweenCondition.Left.Accept(this);

                this.builder.Append(" AND ");

                betweenCondition.Right.Accept(this);
            }

            public override void VisitColumnReference(ColumnReference columnReference) {
                if (columnReference.Base != null)
                {
                    this.builder.Append("\"");
                    this.builder.Append(columnReference.Base);
                    this.builder.Append("\".");
                }

                this.builder.Append("\"");
                this.builder.Append(columnReference.ColumnName);
                this.builder.Append("\"");
            }

            public override void VisitConstant(ConstantValue constantValue) {
                var paramName = "p" + this.Parameters.Keys.Count;
                this.Parameters.Add(paramName, constantValue.Value);

                this.builder.Append("@").Append(paramName);
            }

            public override void VisitBinaryCondition(BinaryCondition binaryCondition)
            {
                this.builder.Append("(");

                binaryCondition.Left.Accept(this);

                this.builder.Append(")");
                
                this.builder.Append(" ").Append(binaryCondition.Operator).Append(" ");

                this.builder.Append("(");

                binaryCondition.Right.Accept(this);

                this.builder.Append(")");
            }

            public override void VisitFunctionCall(FunctionCall functionCall)
            {
                this.builder.Append(functionCall.FunctionName).Append("(");

                foreach (var element in functionCall.Arguments) {
                    element.Accept(this);
                    this.builder.Append(", ");
                }

                this.builder.Remove(this.builder.Length - 2, 2);

                this.builder.Append(")");
            }

            public override void VisitColumnWithAlias(AliasColumn aliasColumn)
            {
                this.builder.Append("(");

                aliasColumn.InnerColumn.Accept(this);

                this.builder.Append(")")
                    .Append(" as \"").Append(aliasColumn.Alias).Append("\"");
            }

            public override void VisitJoins(List<Join> joins)
            {
                foreach (var @join in joins)
                {
                    @join.Accept(this);
                }
            }

            public override void VisitJoin(Join @join)
            {
                this.builder.Append(" ").Append(@join.Type.ToUpper()).Append(" JOIN ");

                if (@join.RecordSet is SelectQuery)
                {
                    this.builder.Append("(");
                }

                @join.RecordSet.Accept(this);

                if (@join.RecordSet is SelectQuery) {
                    this.builder.Append(")");
                }

                if (@join.Alias != null)
                {
                    this.builder.Append(" AS ").Append(@join.Alias);
                }

                this.builder.Append(" ON ");

                @join.Condition.Accept(this);
            }

            public override void VisitInCondition(InCondition inCondition)
            {
                this.builder.Append("(");
                inCondition.Value.Accept(this);
                this.builder.Append(") IN (");

                foreach (var value in inCondition.Values)
                {
                    value.Accept(this);
                    this.builder.Append(", ");
                }

                this.builder.Remove(this.builder.Length - 2, 2);

                this.builder.Append(")");
            }
        }
    }
}