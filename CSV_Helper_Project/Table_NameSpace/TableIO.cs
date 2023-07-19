using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_Project
{
    public partial class Table
    {
        Parser parser = new Parser();
        /// <summary>
        /// Loads a csv table from file. Dont forget to specify before if the table has headers or not for better performance
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeaders"></param>
        /// <param name="delimiter"></param>
        /// <param name="append">determines if the table should be cleared before loading or data should be appended</param>
        public void LoadFromFile(string path, bool hasHeaders = false, char delimiter = ';', bool append = false, Encoding enc = null, int skipLines = 0)
        {
            if (!File.Exists(path)) return;
            if (!append)
            {
                CSV_Table = new List<List<string>>();
            }
            List<string[]> parsedCells = parser.ParseCSVFile(path, delimiter, enc, skipLines);
            if (parsedCells == null || parsedCells.Count <= 0) return;
            int startIndex = 0;
            this.HasHeaders = hasHeaders;
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
            ContentChanged = false;
        }
        /// <summary>
        /// writes the csv table with headers to List
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimiter"></param>
        public void WriteTableToFile(string path, char delimiter = ';',bool emptyCellsAsNull = false)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            StreamWriter stream = File.AppendText(path);
            if (HasHeaders)
            {
                stream.WriteLine(parser.EncodeCSVLine(Headers.ToArray(), delimiter, emptyCellsAsNull: emptyCellsAsNull), delimiter);
            }
            foreach (List<string> line in CSV_Table)
            {
                stream.WriteLine(parser.EncodeCSVLine(line.ToArray(),delimiter, emptyCellsAsNull: emptyCellsAsNull), delimiter);
            }
            stream.Close();
            stream.Dispose();
            ContentChanged = false;
        }
    }
}
