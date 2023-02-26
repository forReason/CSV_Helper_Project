using System.Collections;
using System.Collections.Generic;

namespace QuickCsv.Net.Table_NameSpace
{
    class TableEnumerator : IEnumerator
    {
        List<List<string>> records;
        private int position = -1;

        public TableEnumerator(List<List<string>> records)
        {
            this.records = records;
        }

        public bool MoveNext()
        {
            position++;
            return (position < records.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                return records[position].ToArray();
            }
        }
    }
}
