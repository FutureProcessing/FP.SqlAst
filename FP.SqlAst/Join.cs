namespace FP.SqlAst
{
    using Generators;

    public class Join : SqlAstElement
    {
        public string Type { get; set; }
        public RecordSet RecordSet { get; set; }
        public Condition Condition { get; set; }
        public string Alias { get; set; }

        public override void Accept(SqlAstVisitorBase visitor)
        {
            visitor.VisitJoin(this);
        }
    }
}