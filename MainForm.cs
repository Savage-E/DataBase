using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DataBase
{
    public partial class MainForm : Form
    {
        private string openFileName, folderName;

        private bool fileOpened = false;

        public MainForm()
        {
            InitializeComponent();
            DataGridVievLoad();
        }

        private void DataGridVievLoad()
        {
            DataTable dt = new DataTable();
            try
            {
                string[] lines = File.ReadAllLines(@"F:\repos\DataBase\Resources\References.csv");
                //     string[] files= CheckFiles(lines);
                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(';');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    //For Data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] dataWords = lines[i].Split(';');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;

                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }

                        dt.Rows.Add(dr);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    dataGridViewMain.DataSource = dt;
                    dataGridViewMain.Columns[0].ReadOnly = true;
                    dataGridViewMain.Columns[1].ReadOnly = true;
                    dataGridViewMain.Columns[2].ReadOnly = false;
                    dataGridViewMain.AllowUserToAddRows = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось найти файл References.csv");
            }
        }

        private void _folderOpenBtn_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                var fileList = Directory.GetFiles(folderName, "*.mhtml");

                ParseFiles(fileList);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StringBuilder fileCSV = new StringBuilder("Title;filePath;Comments\t\n");
            int rows = dataGridViewMain.RowCount;
            int columns = dataGridViewMain.ColumnCount;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    fileCSV.Append(dataGridViewMain[j, i].Value + ";");
                }

                if (i == (rows - 1))
                {
                    fileCSV.Append("\t");
                }
                else
                {
                    fileCSV.Append("\t\n");
                }
            }

            StreamWriter wr = new StreamWriter(@"F:\repos\DataBase\Resources\References.csv", false,
                Encoding.GetEncoding("windows-1251"));
            wr.Write(fileCSV);
            wr.Close();
        }

        private void AppendDataToDGV()
        {
        }

        private void ParseFiles(string[] files)
        {
            string[] uniqFiles = CheckFiles(files);

            foreach (string f in uniqFiles)
            {
                StringBuilder sb = new StringBuilder("Аминокислотная последовательность:");

                int seq = 1;
                string[] file = File.ReadAllLines(f);

                string str = "translation=3D";
                bool origin = false;
                bool flag = false;
                foreach (string l in file)
                {
                    if (l.Contains(str))
                    {
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
                                    sb.Append(" "+match.Value.Trim('>', ' ', '='));
                                }
                            }

                            if (collection.Count > 5)
                            {
                                sb.Append("\n");
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }

                DataRow dr;
                string filename = Path.GetFileName(f);
                string[] title = filename.Split('.');
                string filePath = @"F:\repos\DataBase\Resources\Data\" + title[0] + ".txt";

                if (dataGridViewMain.RowCount == 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add(new DataColumn("Title"));
                    dt.Columns.Add(new DataColumn("filePath"));
                    dt.Columns.Add(new DataColumn("Comments"));

                    dr = dt.NewRow();
                    dr["Title"] = title[0];
                    dr["filePath"] = filePath;
                    dr[2] = "";

                    dt.Rows.Add(dr);
                    dataGridViewMain.DataSource = dt;
                    dataGridViewMain.AllowUserToAddRows = false;
                }
                else
                {
                    DataTable dataTable = (DataTable)dataGridViewMain.DataSource;
                    dr = dataTable.NewRow();
                    dr["Title"] = title[0];
                    dr["filePath"] = filePath;
                    dr[2] = "";

                    dataGridViewMain.DataSource = dataTable;
                }

                WriteToFile(sb, title[0]);
            }
        }

        private string[] CheckFiles(String[] files)
        {
            ArrayList filesArrayList = new ArrayList(files);
            ArrayList resultList = new ArrayList(files);
            string references = File.ReadAllText(@"F:\repos\DataBase\Resources\References.csv");

            foreach (var el in filesArrayList)
            {
                if (references.Contains(el.ToString()))
                {
                    resultList.Remove(el);
                }
            }

            string[] result = resultList.ToArray(typeof(string)) as string[];

            return result;
        }

        private void _deleteRowBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count > 0)
            {
                dataGridViewMain.Rows.RemoveAt(dataGridViewMain.SelectedRows[0].Index);
            }
        }

        private void WriteToFile(StringBuilder sb, string title)
        {
            string filePath = @"F:\repos\DataBase\Resources\Data\" + title + ".txt";
            //            String str = sb.ToString();

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine(sb.ToString());
            }
        }
    }
}