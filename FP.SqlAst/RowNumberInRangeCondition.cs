namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents condition on row number
    /// </summary>
    public class RowNumberInRangeCondition : Condition {
        /// <summary>
        /// Real condition
        /// </summary>
        private readonly Condition realCondition;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowNumberInRangeCondition"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column with row number.</param>
        /// <param name="fromInclusive">From record number inclusive.</param>
        /// <param name="toInclusive">To record number inclusive.</param>
        public RowNumberInRangeCondition(string columnName, int? fromInclusive, int? toInclusive) {
            if (fromInclusive.HasValue && toInclusive.HasValue) {
                this.realCondition = new BetweenCondition {
                    Value = new ColumnReference(columnName),
                    Left = new ConstantValue(fromInclusive),
                    Right = new ConstantValue(toInclusive)
                };
            }
            else if (fromInclusive.HasValue && !toInclusive.HasValue) {
                this.realCondition = new BinaryCondition {
                    Left = new ColumnReference(columnName),
                    Right = new ConstantValue(fromInclusive),
                    Operator = ">="
                };
            }
            else {
                this.realCondition = new BinaryCondition {
                    Left = new ColumnReference(columnName),
                    Right = new ConstantValue(toInclusive),
                    Operator = "<="
                };
            }
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            this.realCondition.Accept(visitor);
        }
    }
}