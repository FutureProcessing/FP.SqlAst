namespace FP.SqlAst {
    /// <summary>
    /// Represents source of records. Used in FROM part of SELECT query
    /// </summary>
    public abstract class RecordSet : SqlAstElement
    {
        public string Alias { get; set; }
    }
}