using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DataBase
{
    public partial class MainForm : Form
    {
        private string folderName;

        public MainForm()
        {
            InitializeComponent();
            DataGridViewLoad();
        }

        /// <summary>
        /// Loads data from specified file to DataGridView
        /// </summary>
        private void DataGridViewLoad()
        {
            dataGridViewMain.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.AllCells;
            DataTable dt = new DataTable();
            try
            {
                string[] lines = File.ReadAllLines("Resources/References.csv");

                if (lines.Length > 0)
                {
                    //First line to create header
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

        /// <summary>
        /// Takes folder name with specified files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Unloads data from DataGridView to file References
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            int rows = dataGridViewMain.RowCount;
            int columns = dataGridViewMain.ColumnCount;
            StringBuilder fileCSV = new StringBuilder("Title;filePath;Comments\t\n");

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

            Tools.WriteToFile(fileCSV);
        }

        /// <summary>
        /// Appends data to DataGridView
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="files"></param>
        private void AppendDataToDGV(string[] files)
        {
            string[] uniqFiles = Tools.CheckFiles(files);

            foreach (string f in uniqFiles)
            {
                DataRow dr;
                string filename = Path.GetFileName(f);
                string[] title1 = filename.Split('.');
                string title2 = title1[0] + "_NK";
                title1[0] += "_AMK";
                string filePath1 = Directory.GetCurrentDirectory() + "/Resources/Data/" + title1[0] + ".txt";
                string filePath2 = Directory.GetCurrentDirectory() + "/Resources/Data/" + title2 + ".txt";
                int count = 0;
                while (count < 2)
                {
                    if (dataGridViewMain.RowCount == 0)
                    {
                        DataTable dt = new DataTable();

                        dt.Columns.Add(new DataColumn("Title"));
                        dt.Columns.Add(new DataColumn("filePath"));
                        dt.Columns.Add(new DataColumn("Comments"));

                        dr = dt.NewRow();
                        if (count == 0)
                        {
                            dr["Title"] = title1[0];
                            dr["filePath"] = filePath1;
                            dr[2] = "";
                            count++;
                        }
                        else
                        {
                            dr["Title"] = title2;
                            dr["filePath"] = filePath2;
                            dr[2] = "";
                            count++;
                        }

                        dt.Rows.Add(dr);
                        dataGridViewMain.DataSource = dt;
                    }
                    else
                    {
                        DataTable dataTable = (DataTable)dataGridViewMain.DataSource;
                        dr = dataTable.NewRow();
                        if (count == 0)
                        {
                            dr["Title"] = title1[0];
                            dr["filePath"] = filePath1;
                            dr[2] = "";
                            count++;
                        }
                        else
                        {
                            dr["Title"] = title2;
                            dr["filePath"] = filePath2;
                            dr[2] = "";
                            count++;
                        }
                        dataTable.Rows.Add(dr);
                        dataGridViewMain.DataSource = dataTable;
                    }
                }
            }

            dataGridViewMain.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Parses file on specified sequences
        /// </summary>
        /// <param name="files">the files to extract</param>
        private void ParseFiles(string[] files)
        {
            bool isParsed = Tools.ParseFiles((files));

            if (isParsed)
            {
                AppendDataToDGV(files);
            }
        }

        /// <summary>
        /// Removes selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _deleteRowBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count > 0)
            {
                dataGridViewMain.Rows.RemoveAt(dataGridViewMain.SelectedRows[0].Index);
            }
        }

        /// <summary>
        /// Show data that match to input in TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _nameTbx_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewMain.DataSource as DataTable).DefaultView.RowFilter =
                $"Title like '{_nameTbx.Text}%'";
        }
    }
}