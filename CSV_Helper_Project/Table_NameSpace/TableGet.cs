using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_Project
{
    public partial class Table
    {
        /// <summary>
        /// returns an individual record identified by record index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string[] GetRecord(int index)
        {
            return CSV_Table[index].ToArray();
        }
        /// <summary>
        /// gets a single cell, identified by record index and column index
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetCell(int row, int column)
        {
            return CSV_Table[row][column];
        }
        /// <summary>
        /// gets a single cell, identified by record index and column index
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string GetCell(int row, string columnName)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex == -1)
            {
                throw new ArgumentException($"Column {columnName} could not be found!");
            }
            return GetCell(row, columnIndex);
        }
        /// <summary>
        /// returns a single column but of all records
        /// </summary>
        /// <param name="columnToExtract"></param>
        /// <returns></returns>
        /// <exception cref="FieldAccessException"></exception>
        public string[] GetColumnOfAllRecords(string columnToExtract)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(columnToExtract);
            return GetColumnOfAllRecords(columnIndex);
        }
        /// <summary>
        /// returns one single column but of all records
        /// </summary>
        /// <param name="ColumnIndexToExtract"></param>
        /// <returns></returns>
        public string[] GetColumnOfAllRecords(int ColumnIndexToExtract)
        {
            if (ColumnIndexToExtract < 0) return null;
            int count = CSV_Table.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = CSV_Table[i][ColumnIndexToExtract];
            }
            return result;
        }
        /// <summary>
        /// Finds the index of a record by column name
        /// </summary>
        /// <param name="lookupString"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private int GetRecordIndex(LookupValue lookup)
        {
            if (!HasHeaders) return -1;
            return GetRecordIndex(GetLookupIndexValue(lookup));
        }
        /// <summary>
        /// Finds the index of a record by column index
        /// </summary>
        /// <param name="lookupString"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private int GetRecordIndex(LookupIndexValue lookup)
        {
            if (lookup.ColumnIndex < 0) return -1;
            for (int i = 0; i < CSV_Table.Count; i++)
            {
                string comparison = CSV_Table[i][lookup.ColumnIndex];
                if (lookup.SearchString == comparison)
                {
                    return i;
                }
            }
            return -1;
        }
        public string[] GetUniqueColumnValues(string columnName)
        {
            List<string> records = new List<string>();
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex < 0) return null;
            int count = CSV_Table.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                string value = CSV_Table[i][columnIndex];
                if (!records.Contains(columnName))
                {
                    records.Add(value);
                }
            }
            return records.ToArray();
        }
        public string[] GetUniqueColumnValues(string columnName, LookupValue filter)
        {
            List<string> records = new List<string>();
            int columnIndex = GetColumnIndex(columnName);
            int columnFilterIndex = GetColumnIndex(filter.ColumnName);
            if (columnIndex < 0) return null;
            int count = CSV_Table.Count;
            string[] result = new string[count];

            for (int i = 0; i < count; i++)
            {
                if (CSV_Table[i][columnFilterIndex] == filter.SearchString)
                {
                    string value = CSV_Table[i][columnIndex];
                    if (!records.Contains(columnName))
                    {
                        records.Add(value);
                    }
                }
            }
            return records.ToArray();
        }
        /// <summary>
        /// if column was not found, return -1 otherwise the column index
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int GetColumnIndex(string columnName)
        {
            if (!HasHeaders)
            {
                return -1;
            }
            for (int i = 0; i < Headers.Count; i++)
            {
                string header = Headers[i];
                bool equals = header.Equals(columnName, StringComparison.InvariantCulture);
                if (equals)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// converts a lookup value into a lookup index value in order for lookup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public LookupIndexValue GetLookupIndexValue(LookupValue input)
        {
            int columnIndex = GetColumnIndex(input.ColumnName);
            LookupIndexValue returnValue = new LookupIndexValue(input.SearchString, columnIndex);
            return returnValue;
        }
    }
}
