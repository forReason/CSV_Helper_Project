﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Helper_Project
{
    public class Parser
    {
        String_Helper_Project.TextFileFunctions String_Helper = new String_Helper_Project.TextFileFunctions();
        public string EncodeCSVLine(string[] content, char delimiter = ';', bool trim = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                // quoting rules
                stringBuilder.Append('"');
                foreach (char c in content[i].Trim())
                {
                    if (c == '"') stringBuilder.Append("\"\"");
                    else stringBuilder.Append(c);
                }
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
            Encoding selectedEncoding = Encoding.Default;
            if (enc != null) selectedEncoding = enc;
            String_Helper_Project.InvariantString stringHelper = new String_Helper_Project.InvariantString();
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
                    {
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
                            string cell = stringHelper.InvaryString(sb.ToString());
                            rowEntries.Add(cell);
                            sb.Clear();
                            quotedCell = false;
                        }
                        else if (c == '\n')
                        {
                            string cell = stringHelper.InvaryString(sb.ToString());
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