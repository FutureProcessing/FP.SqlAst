namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents binary condition (LEFT OP RIGHT)
    /// </summary>
    public class BinaryCondition : Condition {
        /// <summary>
        /// Gets or sets left side of condition
        /// </summary>
        public SqlAstElement Left { get; set; }

        /// <summary>
        /// Gets or sets right side of condition
        /// </summary>
        public SqlAstElement Right { get; set; }

        /// <summary>
        /// Gets or sets binary operator
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitBinaryCondition(this);
        }
    }
}