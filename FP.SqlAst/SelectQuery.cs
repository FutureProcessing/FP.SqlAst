namespace FP.SqlAst {
    using System.Collections.Generic;
    using System.Linq;
    using Generators;

    /// <summary>
    /// Represents SELECT query
    /// </summary>
    public class SelectQuery : RecordSet {
        /// <summary>
        /// Gets the columns.
        /// </summary>       
        public List<Column> Columns { get; private set; }

        /// <summary>
        /// Gets or sets FROM recordset.
        /// </summary>     
        public RecordSet From { get; set; }

        /// <summary>
        /// Gets or sets query condition
        /// </summary>
        public Condition Where { get; set; }

        public List<SqlAstElement> GroupBy { get; private set; }

        public List<Join> Joins { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuery"/> class.
        /// </summary>
        public SelectQuery() {
            this.Columns = new List<Column>();
            this.GroupBy = new List<SqlAstElement>();
            this.Joins = new List<Join>();
        }

        /// <summary>
        /// Calls visitor with current element
        /// </summary>
        /// <param name="visitor">Visitor to call</param>
        public override void Accept(SqlAstVisitorBase visitor) {
            visitor.VisitQueryProjection(this.Columns);
            visitor.VisitQueryFrom(this.From);

            if (this.Joins.Any())
            {
                visitor.VisitJoins(this.Joins);
            }

            if (this.Where != null) {
                visitor.VisitQueryCondition(this.Where);
            }

            if (this.GroupBy.Any()) {
                visitor.VisitGroupBy(this.GroupBy);
            }
        }
    }
}