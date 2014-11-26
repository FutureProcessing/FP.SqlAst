namespace FP.SqlAst
{
    using System.Collections.Generic;
    using Generators;

    public class FunctionCall : Column
    {
        public string FunctionName { get; set; }
        public List<SqlAstElement> Arguments { get; private set; }

        public FunctionCall()
        {
            this.Arguments = new List<SqlAstElement>();
        }

        public override void Accept(SqlAstVisitorBase visitor)
        {
            visitor.VisitFunctionCall(this);
        }        
    }
}