namespace FP.SqlAst
{
    using Generators;

    public class AliasColumn : Column {

        public Column InnerColumn { get; set; }
        public string Alias { get; set; }

        public override void Accept(SqlAstVisitorBase visitor)
        {
            visitor.VisitColumnWithAlias(this);
        }
    }
}