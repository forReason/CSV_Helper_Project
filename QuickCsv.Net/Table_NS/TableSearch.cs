namespace QuickCsv.Net.Table_NS
{
    public partial class Table
    {
        /// <summary>
        /// Finds the first record based on a search string and the column name.
        /// </summary>
        /// <param name="lookup">The lookup value containing the search string and column name.</param>
        /// <returns>An array of string representing the record if found, otherwise null.</returns>
        /// <remarks>
        /// This method relies on the presence of headers in the table.
        /// </remarks>
        /// <exception cref="FieldAccessException">Thrown when the table does not have headers.</exception>
        public string[]? SearchRecord(LookupValue lookup)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(lookup.ColumnName);
            return SearchRecord(GetLookupIndexValue(lookup));
        }
        /// <summary>
        /// Finds the first record based on a search string and the column index.
        /// </summary>
        /// <param name="lookup">The lookup index value containing the search string and column index.</param>
        /// <returns>An array of string representing the record if found, otherwise null.</returns>
        /// <remarks>
        /// This method searches for an exact match of the provided search string in the specified column index.
        /// </remarks>
        public string[]? SearchRecord(LookupIndexValue lookup)
        {
            if (lookup.ColumnIndex < 0) return null;
            foreach (List<string> record in CSV_Table)
            {
                if (lookup.SearchString == record[lookup.ColumnIndex])
                {
                    return record.ToArray();
                }
            }
            return null;
        }
        /// <summary>
        /// Finds the first record that matches all the specified lookup index values.
        /// </summary>
        /// <param name="lookupValues">An array of lookup index values containing search strings and column indexes.</param>
        /// <returns>An array of string representing the record if all lookup values match, otherwise null.</returns>
        /// <remarks>
        /// All provided lookup values must match for a record to be returned.
        /// </remarks>
        public string[]? SearchRecord(LookupIndexValue[] lookupValues)
        {
            foreach (LookupIndexValue lookup in lookupValues)
            {
                if (lookup.ColumnIndex < 0)
                {
                    return null;
                }
            }

            foreach (List<string> record in CSV_Table)
            {
                bool correct = true;
                foreach (LookupIndexValue lookup in lookupValues)
                {
                    if (lookup.SearchString != record[lookup.ColumnIndex])
                    {
                        correct = false;
                        break;
                    }
                }
                if (correct) return record.ToArray();
            }
            return null;
        }
        /// <summary>
        /// Finds the first record that matches all the specified lookup values based on column names.
        /// </summary>
        /// <param name="lookupValues">An array of lookup values containing search strings and column names.</param>
        /// <returns>An array of string representing the record if all lookup values match, otherwise null.</returns>
        /// <remarks>
        /// This method converts column names to column indexes and then searches for a record that matches all lookup values.
        /// </remarks>
        public string[]? SearchRecord(LookupValue[] lookupValues)
        {
            List<LookupIndexValue> indexLookups = new List<LookupIndexValue>();
            foreach (LookupValue lookupVal in lookupValues)
            {
                int columnIndex = GetColumnIndex(lookupVal.ColumnName);
                if (columnIndex < 0) return null;
                indexLookups.Add(new LookupIndexValue(lookupVal.SearchString, columnIndex));
            }
            return SearchRecord(indexLookups.ToArray());
        }
    }
}
