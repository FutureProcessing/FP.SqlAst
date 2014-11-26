namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents star (*) in SELECT query
    /// </summary>
    public class AllColumns : Column {
        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitAllColumns(this);
        }
    }
}