namespace FP.SqlAst {
    using Generators;

    /// <summary>
    /// Base class for all elements of SQL Abstract Syntax Tree
    /// </summary>
    public abstract class SqlAstElement {
        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public abstract void Accept(SqlAstVisitorBase visitor);
    }
}