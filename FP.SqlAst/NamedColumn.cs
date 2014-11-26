namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Represents specific column in SELECT query
    /// </summary>
    public class NamedColumn : Column {
        public string Base { get; private set; }

        /// <summary>
        /// Gets column name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedColumn"/> class.
        /// </summary>
        /// <param name="name">Column name.</param>
        public NamedColumn(string @base, string name)
        {
            this.Base = @base;
            this.Name = name;
        }

        public NamedColumn(string name)
            : this(null, name) {

        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitNamedColumn(this);
        }
    }
}