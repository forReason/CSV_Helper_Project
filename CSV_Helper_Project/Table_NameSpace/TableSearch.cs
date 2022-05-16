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
    }
}
