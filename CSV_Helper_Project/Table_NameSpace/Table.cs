using CSV_Helper_Project.Table_NameSpace;
using System.Collections;
using System.Collections.Generic;

namespace CSV_Helper_Project
{
    public partial class Table : IEnumerable
    {
        public Table(bool hasHeaders = false)
        {
            this.HasHeaders = hasHeaders;
        }
        
        List<string> Headers = new List<string>();
        List<List<string>> CSV_Table = new List<List<string>>();
        public bool ContentChanged { get; private set; }
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
        public string[] GetHeaders()
        {
            return this.Headers.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)new TableEnumerator(CSV_Table);
        }

    }
}
