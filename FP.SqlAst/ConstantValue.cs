namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents constant value.
    /// </summary>
    public class ConstantValue : SqlAstElement {
        /// <summary>
        /// Gets value
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ConstantValue class.
        /// </summary>
        /// <param name="value">The value</param>
        public ConstantValue(object value) {
            this.Value = value;
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitConstant(this);
        }
    }
}