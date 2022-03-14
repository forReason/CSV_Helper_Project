using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_Project
{
    public class Table
    {
        public Table(bool hasHeaders = false)
        {
            this.HasHeaders = hasHeaders;
        }
        Parser parser = new Parser();
        List<string> Headers = new List<string>();
        List<List<string>> CSV_Table = new List<List<string>>();
        /// <summary>
        /// the number of records in the table
        /// </summary>
        public int Length { get { return CSV_Table.Count; } }
        /// <summary>
        /// the number of columns in the table
        /// </summary>
        public int ColumnLength
        {
            get
            {
                if (this.HasHeaders) return Headers.Count;
                else if (CSV_Table.Count == 0) return 0;
                else return CSV_Table[0].Count;
            }
        }
        private bool _HasHeaders;
        /// <summary>
        /// returns if the table has headers. When setting, Header row will be automatically removed from Data and moved to header variable or vice versa.
        /// </summary>
        public bool HasHeaders
        {
            get
            {
                return _HasHeaders;
            }
            set
            {
                _HasHeaders = value;
                if (value == true && _HasHeaders == false)
                {
                    if (CSV_Table.Count > 0)
                    {
                        Headers = CSV_Table[0];
                        CSV_Table.RemoveAt(0);
                    }
                }
                else if (value == false && _HasHeaders == true)
                {
                    if (Headers.Count > 0)
                    {
                        CSV_Table.Insert(0, Headers);
                    }
                    Headers = new List<string>();

                }
            }
        }
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
        /// sets the Column names from array. usually for initial creation
        /// </summary>
        /// <param name="names"></param>
        public void SetColumnNames(string[] names)
        {
            this._HasHeaders = true;
            this.Headers = names.ToList();
        }
        /// <summary>
        /// Loads a csv table from file. Dont forget to specify before if the table has headers or not for better performance
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeaders"></param>
        /// <param name="delimiter"></param>
        /// <param name="append">determines if the table should be cleared before loading or data should be appended</param>
        public void LoadFromFile(string path, bool hasHeaders = false, char delimiter = ';', bool append = false)
        {
            if (!File.Exists(path)) return;
            if (!append)
            {
                CSV_Table = new List<List<string>>();
            }
            List<string[]> parsedCells = parser.ParseCSVFile(path, delimiter);
            if (parsedCells == null || parsedCells.Count <= 0) return;
            int startIndex = 0;
            if (HasHeaders)
            {
                this.Headers = parsedCells[0].ToList();
                startIndex = 1;
            }
            for (int i = startIndex; i < parsedCells.Count; i++)
            {
                List<string> cells = parsedCells[i].ToList();
                CSV_Table.Add(cells);
            }
            { }
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
        public void InsertRecord(string[] record, int index)
        {
            CSV_Table.Insert(index, record.ToList());
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
        /// <summary>
        /// Finds the first record based on a search string and the column to search after
        /// </summary>
        /// <param name="lookupString"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string[] SearchRecord(LookupValue lookup)
        {
            if (!HasHeaders) throw new FieldAccessException("Table has no Headers!");
            int columnIndex = GetColumnIndex(lookup.ColumnName);
            return SearchRecord(GetLookupIndexValue(lookup));
        }
        /// <summary>
        /// Finds the first record based on a search string and the column index which should be matched
        /// </summary>
        /// <param name="lookupString"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public string[] SearchRecord(LookupIndexValue lookup)
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
        public string[] SearchRecord(LookupIndexValue[] lookupValues)
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
                foreach(LookupIndexValue lookup in lookupValues)
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
        public string[] SearchRecord(LookupValue[] lookupValues)
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
        //public string[][] GetUniqueRecords(string UniqueIdentifierColumnName)
        //{
        //    List<string[]> records = new List<string[]>();
        //    int columnIndex = GetColumnIndex(UniqueIdentifierColumnName);
        //    int count = CSV_Table.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        bool isContained = false;

        //        foreach (string[] record in records)
        //        {
        //            if (record[columnIndex] == CSV_Table[i][columnIndex])
        //            {
        //                isContained = true;
        //                break;
        //            }
        //        }
        //        if (!isContained)
        //        {
        //            records.Add(CSV_Table[i].ToArray());
        //        }
        //    }
        //}
        /// <summary>
        /// Gets the index of a column by column name (only if the table has headers)
        /// </summary>
        /// <param name="columnName">name of the column to look up</param>
        /// <returns>index or -1</returns>
        public int GetColumnIndex(string columnName)
        {
            if (!HasHeaders)
            {
                return -1;
            }
            for (int i = 0; i < Headers.Count; i++)
            {
                if (Headers[i] == columnName)
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
        /// <summary>
        /// writes the csv table with headers to List
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimiter"></param>
        public void WriteTableToFile(string path, char delimiter = ';')
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            StreamWriter stream = File.AppendText(path);
            if (HasHeaders)
            {
                stream.WriteLine(parser.EncodeCSVLine(Headers.ToArray()), delimiter);
            }
            foreach (List<string> line in CSV_Table)
            {
                stream.WriteLine(parser.EncodeCSVLine(line.ToArray()), delimiter);
            }
            stream.Close();
            stream.Dispose();
        }
    }
}
