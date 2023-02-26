using QuickCsv.Net.Table_NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_UnitTests.Parser
{
    public class GetContent_Tests
    {
        [Fact]
        public void TestTableIterator()
        {
            Table myTable = new Table(hasHeaders: true);
            myTable.LoadFromFile("TestAssets\\addresses.csv",hasHeaders: true, delimiter:',');
            int index = 0;
            List<string[]> records = new List<string[]>();
            foreach (string[] record in myTable)
            {
                records.Add(record);
                index++;
            }
            Assert.Equal(myTable.Length, records.Count);
            Assert.Equal(records[0][0], "John");
        }
    }
}
