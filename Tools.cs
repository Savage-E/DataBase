using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataBase
{
    /// <summary>
    /// Class provides methods to check, write, parse on specified sequences files
    /// </summary>
    ///
    public static class Tools

    {
        /// <summary>
        /// Checks files on duplicates
        /// </summary>
        /// <param name="files">the array of files to check for duplicates</param>
        /// <returns></returns>
        public static string[] CheckFiles(string[] files)
        {
            ArrayList filesArrayList = new ArrayList(files);
            ArrayList resultList = new ArrayList(files);
            string references = File.ReadAllText("Resources/References.csv");

            foreach (var el in filesArrayList)
            {
                if (references.Contains(Path.GetFileName(el.ToString()).Split('.')[0]))
                {
                    resultList.Remove(el);
                }
            }

            string[] result = resultList.ToArray(typeof(string)) as string[];

            return result;
        }

        /// <summary>
        /// Writes extracted sequences to another file
        /// </summary>
        /// <param name="sb"> the StringBuilder contains specified sequences</param>
        /// <param name="title"> the name of the file to write</param>
        public static void WriteToFile(StringBuilder sb, string title)
        {
            string filePath = "Resources/Data/" + title + ".txt";

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Writes data from DataGridView to file
        /// </summary>
        /// <param name="fileCSV">the csv file to write</param>
        public static void WriteToFile(StringBuilder fileCSV)
        {
            using (StreamWriter file = new StreamWriter("Resources/References.csv", false, Encoding.GetEncoding("windows-1251")))
            {
                file.WriteLine(fileCSV);
            }
        }

        /// <summary>
        /// Parses files on specified sequences
        /// </summary>
        /// <param name="files">the array of files to extract</param>
        public static bool ParseFiles(string[] files)
        {
            bool result = false;
            string[] uniqFiles = CheckFiles(files);
            StringBuilder sb = new StringBuilder();
            foreach (string f in uniqFiles)
            {
                sb = new StringBuilder();
                string title = Path.GetFileName(f).Split('.')[0];
                int seq = 1;
                string[] file = File.ReadAllLines(f);

                string str = "translation=3D";
                bool origin = false;
                bool flag = false;
                string line;
                foreach (string l in file)
                {
                    if (l.Contains(str))
                    {

                         line = l.Trim().Substring(15).Trim('=').Trim('"').Trim('\n');
                        AddToStringBuilder(line,sb);
                        flag = true;

                    }
                    else if (flag && !l.Contains("</span>"))
                    {
                        if (l.Trim().Length > 10)
                        {
                            line = l.Trim('=', ' ','"','\n');
                            AddToStringBuilder(line,sb);
                            
                        }
                        else
                        {
                            line = l.Trim();
                            AddToStringBuilder(line,sb);
                        }
                    }
                    else if (l.Contains("ORIGIN"))
                    {
                        WriteToFile(sb, title +"_AMK");
                        sb = new StringBuilder();
                        origin = true;
                        flag = false;
                    }
                    //Parse AMK Sequence.
                    else if (origin)
                    {
                        Regex regex = new Regex(@"(^[acbtg]{1,})|([acbtg]{2,})");
                        MatchCollection collection = regex.Matches(l);

                        if (collection.Count > 0)
                        {
                            foreach (Match match in collection)
                            {

                                line = match.Value.Trim('>', ' ', '=').Trim('\n');
                                    AddToStringBuilder(line,sb);
                                    
                            }
                            
                        }

                        if (l.Contains("//</pre>"))
                        {
                            WriteToFile(sb, title + "_NK");
                            break;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }

                result = true;
            }

            return result;
        }

        private static void AddToStringBuilder(string line,StringBuilder sb)
        {
            char[] letters = line.ToCharArray();
            foreach (char c in letters)
            {
                sb.AppendLine(c.ToString());
            }
        }
    }
}