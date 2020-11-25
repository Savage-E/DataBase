using System;
using System.Data;
using System.IO;
using System.Text;
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
            catch (Exception e)
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


                var fileList = Directory.GetFiles(folderName, " *.mhtml");

                ParseFiles(fileList);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StringBuilder fileCSV = new StringBuilder("Title;filePath;Comments\t\n");
            int rows = dataGridViewMain.RowCount - 1;
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

        private void PrintDataToDGV()
        {

        }

        private void ParseFiles(string[] files)
        {


        }

    }
}