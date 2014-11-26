namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents BETWEEN operator (VALUE BETWEEN LEFT AND RIGHT)
    /// </summary>
    public class BetweenCondition : Condition {
        /// <summary>
        /// Gets or sets value that will be compared
        /// </summary>
        public SqlAstElement Value { get; set; }
        
        /// <summary>
        /// Gets or sets lower range
        /// </summary>
        public SqlAstElement Left { get; set; }

        /// <summary>
        /// Gets or sets upper range
        /// </summary>
        public SqlAstElement Right { get; set; }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitBetween(this);
        }
    }
}