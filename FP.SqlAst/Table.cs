namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents Table as recordset
    /// </summary>
    public class Table : RecordSet {
        /// <summary>
        /// Gets name of table
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="name">Table name</param>
        public Table(string name) {
            this.Name = name;
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitTable(this);
        }
    }
}