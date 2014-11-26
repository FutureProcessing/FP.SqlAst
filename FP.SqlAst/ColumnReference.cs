namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Column reference. Can be used in conditions
    /// </summary>
    public class ColumnReference : SqlAstElement {
        public string Base { get; private set; }

        /// <summary>
        /// Gets or sets column name
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ColumnReference class
        /// </summary>
        /// <param name="columnName">Column name</param>
        public ColumnReference(string columnName) : this(null, columnName) {
            
        }

        public ColumnReference(string @base, string columnName)
        {
            this.Base = @base;
            this.ColumnName = columnName;
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitColumnReference(this);
        }
    }
}