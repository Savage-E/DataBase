using System;
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
            /*string[] title = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                title[i] = Path.GetFileName(files[i]).Split('.')[0];
            }*/

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
        public static StringBuilder ParseFiles(string[] files)
        {
            string[] uniqFiles = CheckFiles(files);
            StringBuilder sb = new StringBuilder();
            foreach (string f in uniqFiles)
            {
                sb = new StringBuilder("Аминокислотная последовательность:");

                int seq = 1;
                string[] file = File.ReadAllLines(f);

                string str = "translation=3D";
                bool origin = false;
                bool flag = false;
                foreach (string l in file)
                {
                    if (l.Contains(str))
                    {
                        // write title of the sequence
                        switch (seq)
                        {
                            case 1:
                                sb.Append("\n\nN : ");
                                seq++;
                                break;

                            case 2:
                                sb.Append("\n\nP : ");
                                seq++;
                                break;

                            case 3:
                                sb.Append("\n\nM : ");
                                seq++;
                                break;

                            case 4:
                                sb.Append("\n\nG : ");
                                seq++;
                                break;

                            case 5:
                                sb.Append("\n\nL : ");
                                seq++;
                                break;
                        }

                        sb.Append(l.Trim().Substring(15).Trim('='));

                        flag = true;
                    }
                    else if (flag && !l.Contains("</span>"))
                    {
                        if (l.Trim().Length > 10)
                            sb.Append("\n" + (l.Trim('=', ' ')));
                        else
                        {
                            sb.Append(l.Trim());
                        }
                    }
                    else if (l.Contains("ORIGIN"))
                    {
                        origin = true;
                        flag = false;
                        sb.Append("\nНуклеотидная последовательность : \n");
                    }
                    else if (origin)
                    {
                        Regex regex = new Regex(@"([acbtg=]{2,})|((>|^)\s*\d*)");
                        MatchCollection collection = regex.Matches(l);

                        if (collection.Count > 0)
                        {
                            foreach (Match match in collection)
                            {
                                int number;
                                if (Int32.TryParse(match.Value, out number))
                                    sb.Append(number + " ");
                                else if (match.Value.Length < 6)
                                    sb.Append(match.Value.Trim('>', ' ', '=') + " ");
                                else if (match.Value.Length >= 6)
                                {
                                    sb.Append(" " + match.Value.Trim('>', ' ', '='));
                                }
                            }

                            if (collection.Count > 5)
                            {
                                sb.Append("\n");
                            }
                        }

                        if (l.Contains("//</pre>"))
                            break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }

            return sb;
        }
    }
}