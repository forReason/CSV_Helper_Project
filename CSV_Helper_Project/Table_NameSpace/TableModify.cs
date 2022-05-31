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
        /// sets the Column names from array. usually for initial creation
        /// </summary>
        /// <param name="names"></param>
        public void SetColumnNames(string[] names)
        {
            this._HasHeaders = true;
            this.Headers = names.ToList();
        }

        /// <summary>
        /// searches for a record, matched by a search string and a columnName which should be matched. If found, removes the record.
        /// </summary>
        /// <param name="fieldToMatch"></param>
        /// <param name="columnName"></param>
        public void RemoveRecordIfExists(LookupValue lookup)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(lookup.ColumnName);
            RemoveRecordIfExists(GetLookupIndexValue(lookup));
        }
        public void RemoveRecord(int index)
        {
            if (index < 0) throw new FieldAccessException($"index {index} is smaller than 0!");
            if (index > CSV_Table.Count) throw new FieldAccessException($"index {index} is bigger than table length {CSV_Table.Count}!");
            CSV_Table.RemoveAt(index);
        }
        /// <summary>
        /// searches for a record, matched by a search string and a columnindex which should be matched. If found, removes the record.
        /// </summary>
        /// <param name="fieldToMatch"></param>
        /// <param name="columnIndex"></param>
        public void RemoveRecordIfExists(LookupIndexValue lookup)
        {
            int index = GetRecordIndex(lookup);
            if (index >= 0)
            {
                CSV_Table.RemoveAt(index);
            }
        }
        /// <summary>
        /// if the same record is found, it gets overwritten. If not inserted into the table.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="columnToMatch"></param>
        public void OverwriteOrInsertRecord(string[] record, string columnToMatch)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(columnToMatch);
            OverwriteOrInsertRecord(record, columnIndex);
        }
        /// <summary>
        /// if the same record is found, it gets overwritten. If not inserted into the table.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="columnToMatch"></param>
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
        }
        /// <summary>
        /// plain append, even if the record is a duplicate
        /// </summary>
        /// <param name="record"></param>
        public void AppendRecord(string[] record)
        {
            CSV_Table.Add(record.ToList());
            { }
        }
        public int AppendEmptyRecord()
        {
            string[] vs = new string[Headers.Count];
            CSV_Table.Add(vs.ToList());
            return CSV_Table.Count - 1;
        }
        public void InsertRecord(string[] record, int index)
        {
            CSV_Table.Insert(index, record.ToList());
        }
        public void InsertEmptyRecord(int index)
        {
            string[] vs = new string[Headers.Count];
            CSV_Table.Insert(index, vs.ToList());
        }
        /// <summary>
        /// adds a new column to the Table
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddColumn(string columnName = null, string defaultValue = "")
        {
            if (this.HasHeaders)
            {
                if (columnName == null)
                {
                    throw new ArgumentException("Table has headers. A ColumnName must be set!");
                }
                if (GetColumnIndex(columnName) != -1)
                {
                    throw new ArgumentException("Column with the same name Exists already!");
                }
                this.Headers.Add(columnName);
            }
            for(int i = 0; i < this.CSV_Table.Count; i++)
            {
                this.CSV_Table[i].Add(defaultValue);
            }
        }
        public void SetCell(string columnName, int row, string value)
        {
            int columnIndex = GetColumnIndex(columnName);
            if (columnIndex == -1)
            {
                throw new Exception("column could not be matched!");
            }
            this.CSV_Table[row][columnIndex] = value;
        }
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
        }
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
        }
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
        }
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
        }
    }
}
