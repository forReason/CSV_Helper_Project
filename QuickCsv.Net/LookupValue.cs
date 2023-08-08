namespace QuickCsv.Net
{
    /// <summary>
    /// Represents a search criteria based on a string value and a column name.
    /// </summary>
    public struct LookupValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupValue"/> struct.
        /// </summary>
        /// <param name="searchString">The string value to search for.</param>
        /// <param name="columnToLookup">The name of the column to search in.</param>
        public LookupValue(string searchString, string columnToLookup)
        {
            this.SearchString = searchString;
            this.ColumnName = columnToLookup;
        }

        /// <summary>
        /// Gets or sets the string value to search for.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to search in.
        /// </summary>
        public string ColumnName { get; set; }
    }

    /// <summary>
    /// Represents a search criteria based on a string value and a column index.
    /// </summary>
    public struct LookupIndexValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupIndexValue"/> struct.
        /// </summary>
        /// <param name="searchString">The string value to search for.</param>
        /// <param name="columnIndexToLookup">The index of the column to search in.</param>
        public LookupIndexValue(string searchString, int columnIndexToLookup)
        {
            this.SearchString = searchString;
            this.ColumnIndex = columnIndexToLookup;
        }

        /// <summary>
        /// Gets or sets the string value to search for.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the index of the column to search in.
        /// </summary>
        public int ColumnIndex { get; set; }
    }
}
