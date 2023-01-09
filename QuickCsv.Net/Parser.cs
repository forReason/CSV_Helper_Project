using System.Text;
using StringHelper.Net;

namespace QuickCsv.Net
{
    public class Parser
    {
        public string EncodeCSVLine(string[] content, char delimiter = ';',bool emptyCellsAsNull = false, bool trim = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                // quoting rules
                stringBuilder.Append('"');
                if (content[i]!= null)
                {
                    foreach (char c in content[i].Trim())
                    {
                        if (c == '"') stringBuilder.Append("\"\"");
                        else stringBuilder.Append(c);
                    }
                }
                else if (emptyCellsAsNull) stringBuilder.Append("null");
                stringBuilder.Append('"');
                if (i != content.Length - 1)
                {
                    stringBuilder.Append(delimiter);
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// this should be rfc4180 compliant!
        /// </summary>
        /// <param name="path"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public List<string[]> ParseCSVFile(string path, char splitChar = ';', Encoding enc = null, int skiplines = 0)
        {
            Encoding selectedEncoding = Encoding.UTF8;
            if (enc != null) selectedEncoding = enc;
            StringBuilder sb = new StringBuilder();
            List<string[]> table = new List<string[]>();
            List<string> rowEntries = new List<string>();
            bool openQuotes = false;
            bool quotedCell = false;
            bool lastCharWasQuote = false;
            using (StreamReader sr = new StreamReader(path, selectedEncoding))
            {
                for(int i = 0; i < skiplines; i++)
                {
                    string line = sr.ReadLine();
                    if (line == null) return null;
                }
                while (sr.Peek() >= 0)
                {
                    char c = (char)sr.Read();
                    if (c == '"')
                    { // quoting mechanic (encapsulating cells)
                        if (openQuotes)
                        {
                            openQuotes = false;
                            lastCharWasQuote = true;
                        }
                        else if (sb.Length == 0 || quotedCell)
                        {
                            if (lastCharWasQuote)
                            {
                                lastCharWasQuote = false;
                                sb.Append('"');
                            }
                            openQuotes = true;
                            quotedCell = true;
                        }
                        else sb.Append('"');
                    }
                    else if (!openQuotes)
                    {
                        lastCharWasQuote = false;
                        if (c == splitChar)
                        {
                            string cell = InvariantString.InvaryString(input: sb.ToString(),cleanUmlaute: false);
                            rowEntries.Add(cell);
                            sb.Clear();
                            quotedCell = false;
                        }
                        else if (c == '\n' || c == '\r')
                        {
                            // edgecase windows '\r\n' if c == r, check if next char is actually newline (proceed), otherwise append to sting and continue with next loop iteration
                            if (c == '\r')
                            { // we have an '\r', check if next comes an '\n'!
                                if (sr.Peek() >= 0)
                                { // actually make sure, there is still data to read and the file is not over
                                    c = (char)sr.Read();
                                    if (c != '\n')
                                    { // not a newline, append and continue loop with next char
                                        sb.Append('\r');
                                        sb.Append(c);
                                        continue;
                                    }
                                }
                            }
                            // c must be n, as previously checked in \r\n test. this line is over
                            string cell = InvariantString.InvaryString(input: sb.ToString(), cleanUmlaute: false);
                            rowEntries.Add(cell);
                            sb.Clear();
                            table.Add(rowEntries.ToArray());
                            rowEntries = new List<string>();
                            quotedCell = false;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    else
                    {
                        lastCharWasQuote = false;
                        sb.Append(c);
                    }
                }
            }
            return table;
        }
    }
    public enum Encodings
    {
        Default,
        UTF8,
        ASCII,
        Unicode,
        UTF7,
        UTF32
    }
}
