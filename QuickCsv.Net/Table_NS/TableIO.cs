using System.Text;
using System.Xml.Linq;

namespace QuickCsv.Net.Table_NS
{
    public partial class Table
    {
        Parser parser = new Parser();
        /// <summary>
        /// Loads a csv table from file. Dont forget to specify before if the table has headers or not for better performance
        /// </summary>
        /// <remarks>
        /// automatically sets TargetFile for <see cref="Table.Save(char, bool)"/>
        /// </remarks>
        /// <param name="path"></param>
        /// <param name="hasHeaders"></param>
        /// <param name="delimiter"></param>
        /// <param name="append">determines if the table should be cleared before loading or data should be appended</param>
        public void LoadFromFile(string path, bool hasHeaders = false, char delimiter = ';', bool append = false, Encoding? enc = null, int skipLines = 0)
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
            ContentChanged = false;
            TargetFile = new FileInfo(path);
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
        /// <summary>
        /// saves the Table to the file information stored in this.TargetFile
        /// </summary>
        /// <param name="delimiter">the delimiting character between the cells</param>
        /// <param name="emptyCellsAsNull">wether empty cells should be `null` or `""`</param>
        /// <exception cref="NullReferenceException">if this.TargetFile is not set</exception>
        public void Save(char delimiter = ';', bool emptyCellsAsNull = false)
        {
            if (TargetFile == null)
            {
                throw new NullReferenceException(nameof(TargetFile));
            }
            WriteTableToFile(TargetFile.FullName, delimiter, emptyCellsAsNull);
        }
    }
}
