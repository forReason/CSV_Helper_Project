namespace QuickCsv.Net.Table_NS
{
    public partial class Table
    {
        /// <summary>
        /// Sets the column names for the table from an array, primarily used for initial table creation.
        /// </summary>
        /// <param name="names">Array of column names to be set.</param>
        /// <remarks>
        /// This method initializes the table headers without rearranging existing table columns.
        /// <br/><br/>
        /// <example><code>
        /// Before:
        /// | 0 | 1   |
        /// |---|-----|
        /// | A | B   |
        ///
        /// After using SetColumnNames(["First", "Second"]):
        /// | First | Second |
        /// |-------|--------|
        /// | A     | B      |
        /// </code></example>
        /// </remarks>
        public void SetColumnNames(string[] names)
        {
            this._HasHeaders = true;
            this.Headers = names.ToList();
            ContentChanged = true;
        }

        /// <summary>
        /// Removes a record from the table at a given index.
        /// </summary>
        /// <param name="index">Index of the record to be removed.</param>
        /// <remarks>
        /// This method removes a record at the specified index. Indexes outside of the table boundaries will result in exceptions.
        /// <br/><br/>
        /// <example><code>
        /// Given:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        ///
        /// After RemoveRecord(1):
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Charlie |
        /// </code></example>
        /// </remarks>
        /// <exception cref="FieldAccessException">Thrown when the provided index is outside of the table's boundaries.</exception>
        public void RemoveRecord(int index)
        {
            if (index < 0) throw new FieldAccessException($"index {index} is smaller than 0!");
            if (index > CSV_Table.Count) throw new FieldAccessException($"index {index} is bigger than table length {CSV_Table.Count}!");
            CSV_Table.RemoveAt(index);
            ContentChanged = true;
        }


        /// <summary>
        /// Searches for a record, matched by a search string and a column name which should be matched. If found, removes the record.
        /// </summary>
        /// <param name="lookup">The lookup criteria containing the search string and column name to match against.</param>
        /// <remarks>
        /// This method removes a record from the table if it finds a matching value in the specified column by its name. If the table does not have headers, a <see cref="FieldAccessException"/> will be thrown.
        /// <br/><br/>
        /// <example><code>
        /// Given a CSV table with headers:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// 
        /// If searching with a LookupValue("Bob", "Name"):
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Charlie |
        /// </code></example>
        /// </remarks>
        /// <exception cref="FieldAccessException">Thrown when the table does not have headers.</exception>
        public void RemoveRecordIfExists(LookupValue lookup)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(lookup.ColumnName);
            RemoveRecordIfExists(GetLookupIndexValue(lookup));
            ContentChanged = true;
        }

        /// <summary>
        /// Searches for a record, matched by a search string and a column index as unique id which should be matched. <br/>
        /// If found, removes the first instance record.
        /// </summary>
        /// <param name="lookup">The lookup criteria containing the search string and column index to match against.</param>
        /// <remarks>
        /// This method removes a record from the table if it finds a matching value in the specified column.
        /// <br/><br/>
        /// <example><code>
        /// Given a CSV table:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// 
        /// If searching with a LookupIndexValue("Bob", 1):
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Charlie |
        /// </code></example>
        /// </remarks>
        public void RemoveRecordIfExists(LookupIndexValue lookup)
        {
            int index = GetRecordIndex(lookup);
            if (index >= 0)
            {
                CSV_Table.RemoveAt(index);
                ContentChanged = true;
            }
        }
        /// <summary>
        /// if the same record is found, it gets overwritten. If not, appended to the table.
        /// </summary>
        /// <remarks>
        /// test
        /// <example><code>
        /// Given a CSV table with columnToMatch = "Name":
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 25  |
        /// | 1     | Bob     | 30  |
        /// 
        /// If attempting to insert a record ["Alice", "28"] 
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 28  | // overwritten with new age.
        /// | 1     | Bob     | 30  |
        /// 
        /// If attempting to insert a record ["Charlie", "22"]:
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 28  |
        /// | 1     | Bob     | 30  |
        /// | 2     | Charlie | 22  | // new record
        /// </code></example></remarks>
        /// <param name="record">the data to insert/Update</param>
        /// <param name="columnToMatch">the unique identifier column which specifies if the record exists</param>
        public void OverwriteOrInsertRecord(string[] record, string columnToMatch)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(columnToMatch);
            OverwriteOrInsertRecord(record, columnIndex);
            ContentChanged = true;
        }

        /// <summary>
        /// if the same record is found, it gets overwritten. If not, appended to the table.
        /// </summary>
        /// <remarks>
        /// test
        /// <example><code>
        /// Given a CSV table with columnIndexToMatch = 0 (indicating the "Name" column):
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 25  |
        /// | 1     | Bob     | 30  |
        /// 
        /// If attempting to insert a record ["Alice", "28"] 
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 28  | // overwritten with new age.
        /// | 1     | Bob     | 30  |
        /// 
        /// If attempting to insert a record ["Charlie", "22"]:
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 28  |
        /// | 1     | Bob     | 30  |
        /// | 2     | Charlie | 22  | // new record
        /// </code></example></remarks>
        /// <param name="record">the data to insert/Update</param>
        /// <param name="columnIndexToMatch">the unique identifier column which specifies if the record exists</param>
        public void OverwriteOrInsertRecord(string[] record, int columnIndexToMatch)
        {
            LookupIndexValue lookup = new LookupIndexValue(record[columnIndexToMatch], columnIndexToMatch);
            int index = GetRecordIndex(lookup);
            if (index >= 0)
            {
                CSV_Table[index] = record.ToList();
            }
            else
            {
                CSV_Table.Add(record.ToList());
            }
            ContentChanged = true;
        }
        /// <summary>
        /// adds a record to the end of the table, effectively extending it
        /// </summary>
        /// <remarks>
        /// no duplicate check
        /// <br/><br/>
        /// <example><code>
        /// Given a CSV table:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// 
        /// after appending Record ["Eva"]:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// | 3     | Eva     |
        /// </code></example>
        /// </remarks>
        /// <param name="record">the record to append</param>
        public void AppendRecord(string[] record)
        {
            CSV_Table.Add(record.ToList());
            ContentChanged = true;
            { }
        }
        /// <summary>
        /// adds an empty record to the end of the table, effectively extending it
        /// </summary>
        /// <remarks>
        /// <example><code>
        /// Given a CSV table:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// 
        /// after appending Record ["Eva"]:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// | 3     |         |
        /// </code></example>
        /// </remarks>
        public int AppendEmptyRecord()
        {
            string[] vs = new string[Headers.Count];
            CSV_Table.Add(vs.ToList());
            return CSV_Table.Count - 1;
        }
        /// <summary>
        /// Inserts a record at the given index.
        /// </summary>
        /// <remarks>
        /// Moves all affected records one slot up. <br/>
        /// If a record exists at the given index, the existing record and all subsequent records are moved to their current index + 1. <br/>
        /// The new record is then inserted at the specified index, in front of the existing record which has been shifted.
        /// <br/><br/>
        /// Example:<br/>
        /// <code>
        /// Given a CSV table:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// | 3     | David   |
        /// 
        /// Inserting a record ["Eva"] at index 2 results in:
        /// | Index | Name    |
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Eva     |
        /// | 3     | Charlie |
        /// | 4     | David   |
        /// </code>
        /// </remarks>
        /// <param name="record">The record to be inserted.</param>
        /// <param name="index">The index at which the record should be inserted.</param>
        public void InsertRecord(string[] record, int index)
        {
            CSV_Table.Insert(index, record.ToList());
            ContentChanged = true;
        }
        /// <summary>
        /// Inserts an empty record at the given index.
        /// </summary>
        /// <remarks>
        /// Moves all affected records one slot up. <br/>
        /// If a record exists at the given index, the existing record and all subsequent records are moved to their current index + 1. <br/>
        /// The new record is then inserted at the specified index, in front of the existing record which has been shifted.
        /// <br/><br/>
        /// Example:<br/>
        /// <code>
        /// Given a CSV table:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     | Charlie |
        /// | 3     | David   |
        /// 
        /// Inserting an empty record at index 2 results in:
        /// | Index | Name    |
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        /// | 2     |         |
        /// | 3     | Charlie |
        /// | 4     | David   |
        /// </code>
        /// </remarks>
        /// <param name="index">The index at which the record should be inserted.</param>
        public void InsertEmptyRecord(int index)
        {
            string[] vs = new string[Headers.Count];
            CSV_Table.Insert(index, vs.ToList());
            ContentChanged = true;
        }
        /// <summary>
        /// Adds a new column to the table with an optional default value for each row.
        /// </summary>
        /// <param name="columnName">The name of the new column. This is mandatory if the table has headers.</param>
        /// <param name="defaultValue">The default value to populate the new column with. Defaults to an empty string if not provided.</param>
        /// <remarks>
        /// This method appends a new column to the table. If the table has headers, a valid column name must be provided.
        /// If the column name is already present in the headers, an exception is thrown.
        /// <br/><br/>
        /// <example><code>
        /// Given:
        /// | Index | Name    | 
        /// |-------|---------|
        /// | 0     | Alice   |
        /// | 1     | Bob     |
        ///
        /// After AddColumn("Age", "25"):
        /// | Index | Name    | Age |
        /// |-------|---------|-----|
        /// | 0     | Alice   | 25  |
        /// | 1     | Bob     | 25  |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when no column name is provided for a table with headers, or when a column with the provided name already exists.</exception>
        public void AddColumn(string? columnName = null, string defaultValue = "")
        {
            if (this.HasHeaders)
            {
                if (columnName == null)
                {
                    throw new ArgumentException("Table has headers. A ColumnName must be set!");
                }
                if (GetColumnIndex(columnName) != -1)
                {
                    throw new ArgumentException($"Column with the name {columnName} Exists already!");
                }
                this.Headers.Add(columnName);
            }
            for(int i = 0; i < this.CSV_Table.Count; i++)
            {
                this.CSV_Table[i].Add(defaultValue);
            }
            ContentChanged = true;
        }
        /// <summary>
        /// Sets the value of a target cell, replacing any existing value in that cell.
        /// </summary>
        /// <param name="columnName">The selected column by its name.</param>
        /// <param name="row">The row index.</param>
        /// <param name="value">The value to set in the target cell.</param>
        /// <remarks>
        /// <example><code>
        /// Given a table:
        /// | Name    | Age |
        /// |---------|-----|
        /// | Alice   | 25  |
        /// 
        /// After SetCell("Age", 0, "66"):
        /// | Name    | Age |
        /// |---------|-----|
        /// | Alice   | 66  |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the specified column cannot be found.</exception>
        public void SetCell(string columnName, int row, string value)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Colummn {columnName} could not be found!");
            }
            this.CSV_Table[row][columnIndex] = value;
            ContentChanged = true;
        }
        /// <summary>
        /// Removes an entire column from the table.
        /// </summary>
        /// <param name="columnName">The name of the column to remove.</param>
        /// <remarks>
        /// This action is irreversible and can be resource-intensive for larger tables.
        /// <br/><br/>
        /// <example><code>
        /// Given a table:
        /// | Name    | Age |
        /// |---------|-----|
        /// | Alice   | 25  |
        /// 
        /// After RemoveColumn("Age"):
        /// | Name    |
        /// |---------|
        /// | Alice   |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the specified column cannot be found.</exception>
        public void RemoveColumn(string columnName)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Colummn {columnName} could not be found!");
            }
            this.Headers.RemoveAt(columnIndex);
            for(int i = 0; i < this.CSV_Table.Count; i++)
            {
                this.CSV_Table[i].RemoveAt(columnIndex);
            }
            ContentChanged = true;
        }
        /// <summary>
        /// Renames a given column.
        /// </summary>
        /// <param name="oldName">The current name of the column.</param>
        /// <param name="newName">The new name for the column.</param>
        /// <remarks>
        /// <example><code>
        /// Given a table:
        /// | Name    | Age |
        /// |---------|-----|
        /// | Alice   | 25  |
        /// 
        /// After RenameColumn("Age", "Years"):
        /// | Name    | Years |
        /// |---------|-------|
        /// | Alice   | 25    |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the old column cannot be found or a column with the new name already exists.</exception>
        public void RenameColumn(string oldName, string newName)
        {
            int oldColumnIndex = GetColumnIndex(oldName);
            if (oldColumnIndex == -1)
            {
                throw new ArgumentException($"Old Column with Name {oldName} was not found!");
            }
            int newColumnIndex = GetColumnIndex(newName);
            if (newColumnIndex != -1)
            {
                throw new ArgumentException($"New Column with Name {newName} exists already!");
            }
            this.Headers[oldColumnIndex] = newName;
            ContentChanged = true;
        }
        /// <summary>
        /// Searches for and replaces specific cells based on exact value matches.
        /// </summary>
        /// <param name="columnName">The column to search within.</param>
        /// <param name="oldValue">The value to be replaced.</param>
        /// <param name="newValue">The value to replace with.</param>
        /// <remarks>
        /// <example><code>
        /// Given a table:
        /// | Name    |
        /// |---------|
        /// | Alice   |
        /// 
        /// After ReplaceValue("Name", "Alice", "Eve"):
        /// | Name    |
        /// |---------|
        /// | Eve     |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the specified column cannot be found.</exception>
        public void ReplaceValue(string columnName, string oldValue, string newValue)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column with Name {columnName} was not found!");
            }
            for(int i = 0; i < this.CSV_Table.Count; i++)
            {
                if (this.CSV_Table[i][columnIndex].Equals(oldValue,StringComparison.InvariantCulture))
                {
                    this.CSV_Table[i][columnIndex] = newValue;
                }
            }
            ContentChanged = true;
        }
        /// <summary>
        /// Reorganizes the columns in the table based on a specified order.
        /// </summary>
        /// <param name="headers">An array of column headers defining the new order.</param>
        /// <remarks>
        /// The new column order must match the existing columns but can be in a different sequence.
        /// This can be resource-intensive depending on table size.
        /// <br/><br/>
        /// <example><code>
        /// Given a table:
        /// | Name    | Age |
        /// |---------|-----|
        /// | Alice   | 25  |
        /// 
        /// After ReorderColumnNames(["Age", "Name"]):
        /// | Age | Name    |
        /// |-----|---------|
        /// | 25  | Alice   |
        /// </code></example>
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the new headers do not match the existing headers or a column in the new headers is not found.</exception>
        public void ReorderColumnNames(string[] headers)
        {
            if (headers.Length != this.Headers.Count)
            {
                throw new ArgumentException($"new headers length {headers.Length} does not match old headers Length {this.Headers.Count}!");
            }
            int[] headerIndexes = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndexes[i] = GetColumnIndex(headers[i]);
                if (headerIndexes[i] == -1)
                {
                    throw new ArgumentException($"new header {headers[i]} was not found in existing headers");
                }
            }
            this.Headers = headers.ToList();
            for(int i = 0; i < this.CSV_Table.Count; i++)
            {
                List<string> newRecord = new List<string>();
                foreach(int index in headerIndexes)
                {
                    newRecord.Add(GetCell(row: i, column: index));
                }
                this.CSV_Table[i] = newRecord;
            }
            ContentChanged = true;
        }
        /// <summary>
        /// Reverses the row order of the table.
        /// </summary>
        /// <remarks>
        /// <example><code>
        /// Given a table:
        /// | Name    |
        /// |---------|
        /// | Alice   |
        /// | Bob     |
        /// 
        /// After Reverse():
        /// | Name    |
        /// |---------|
        /// | Bob     |
        /// | Alice   |
        /// </code></example>
        /// </remarks>
        public void Reverse()
        {
            this.CSV_Table.Reverse();
            ContentChanged = true;
        }
    }
}
