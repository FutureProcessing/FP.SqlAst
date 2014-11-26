namespace FP.SqlAst.Generators {
    using System;
    using System.Collections.Generic;

    public abstract class SqlAstVisitorBase {       
        public virtual void Visit(SqlAstElement element) {
            throw new NotSupportedException("Element " + element.GetType().Name + " not supported by this visitor");
        }

        public virtual void VisitSqlQuery(SelectQuery selectQuery) {
            this.Visit(selectQuery);
        }

        public virtual void VisitTable(Table table) {
            this.Visit(table);
        }

        public virtual void VisitAllColumns(AllColumns allColumns) {
            this.Visit(allColumns);
        }

        public virtual void VisitNamedColumn(NamedColumn namedColumn) {
            this.Visit(namedColumn);
        }

        public virtual void VisitQueryProjection(IEnumerable<Column> enumerable) {
            throw new NotSupportedException("Query projection not supported");
        }

        public virtual void VisitQueryFrom(RecordSet @from) {
            throw new NotSupportedException("Query from not supported");
        }

        public virtual void VisitRowNumberColumn(RowNumberColumn rowNumberColumn) {
            this.Visit(rowNumberColumn);
        }

        public virtual void VisitQueryCondition(Condition condition) {
            this.Visit(condition);
        }

        public virtual void VisitConstant(ConstantValue constantValue) {
            this.Visit(constantValue);
        }

        public virtual void VisitColumnReference(ColumnReference columnReference) {
            this.Visit(columnReference);
        }

        public virtual void VisitBetween(BetweenCondition betweenCondition) {
            this.Visit(betweenCondition);
        }

        public virtual void VisitBinaryCondition(BinaryCondition binaryCondition) {
            this.Visit(binaryCondition);
        }

        public virtual void VisitGroupBy(List<SqlAstElement> groupBy)
        {
            throw new NotSupportedException("Group by not supported");
        }

        public virtual void VisitFunctionCall(FunctionCall functionCall)
        {
            this.Visit(functionCall);
        }

        public virtual void VisitColumnWithAlias(AliasColumn aliasColumn)
        {
            this.Visit(aliasColumn);
        }

        public virtual void VisitJoins(List<Join> joins)
        {
            throw new NotSupportedException("Joins not supported");
        }

        public virtual void VisitJoin(Join @join)
        {
            this.Visit(@join);
        }

        public virtual void VisitInCondition(InCondition inCondition)
        {
            this.Visit(inCondition);
        }
    }
}