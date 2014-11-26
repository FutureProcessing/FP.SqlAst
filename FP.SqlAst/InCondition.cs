namespace FP.SqlAst
{
    using System.Collections.Generic;
    using Generators;

    public class InCondition : Condition
    {
        public SqlAstElement Value { get; set; }

        public List<SqlAstElement> Values { get; set; }

        public InCondition()
        {
            this.Values = new List<SqlAstElement>();
        }

        public override void Accept(SqlAstVisitorBase visitor)
        {
            visitor.VisitInCondition(this);
        }
    }
}