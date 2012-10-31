namespace AccessProviderSample
{
    /// <summary>
    /// Type of item represented by the path
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// Represents a database
        /// </summary>
        Database,
        /// <summary>
        /// Represents a table
        /// </summary>
        Table,
        /// <summary>
        /// Represents a row
        /// </summary>
        Row,
        /// <summary>
        /// Represents an invalid path
        /// </summary>
        Invalid
    };
}