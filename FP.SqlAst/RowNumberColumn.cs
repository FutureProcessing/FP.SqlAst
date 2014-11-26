namespace FP.SqlAst {
    using System.Collections.Generic;
    using Generators;

    /// <summary>
    /// Represents column with row number
    /// </summary>
    public class RowNumberColumn : Column {
        /// <summary>
        /// Gets name of the column
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Gets columns used to sort records
        /// </summary>
        public List<NamedColumn> OrderBy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowNumberColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        public RowNumberColumn(string columnName) {
            this.ColumnName = columnName;
            this.OrderBy = new List<NamedColumn>();
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitRowNumberColumn(this);
        }
    }
}