using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_Project
{
    public struct LookupValue
    {
        public LookupValue (string searchString, string columnToLookup)
        {
            this.SearchString = searchString;
            this.ColumnName = columnToLookup;
        }
        public string SearchString { get; set; }
        public string ColumnName { get; set; }
    }
    public struct LookupIndexValue
    {
        public LookupIndexValue(string searchString, int columnIndexToLookup)
        {
            this.SearchString = searchString;
            this.ColumnIndex = columnIndexToLookup;
        }
        public string SearchString { get; set; }
        public int ColumnIndex { get; set; }
    }
}
