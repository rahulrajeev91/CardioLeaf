using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardioLeaf
{
    public partial class datatableControl : UserControl
    {
        public datatableControl()
        {
            InitializeComponent();
        }

        public Boolean ClearDT()
        {
            dataGridView1.Rows.Clear();
            return true;
        }

        public Boolean importCsv(string csvPath)
        {
            dataGridView1.Rows.Clear();
            try
            {
                if (System.IO.File.Exists(csvPath))
                {
                    System.IO.StreamReader fileReader = new System.IO.StreamReader(csvPath, false);


                    //Reading Data
                    while (fileReader.Peek() != -1)
                    {
                        string fileRow = fileReader.ReadLine();
                        string[] fileDataField = fileRow.Split(',');
                        dataGridView1.Rows.Add(fileDataField);
                    }
                    fileReader.Dispose();
                    fileReader.Close();
                }
                else
                {
                    MessageBox.Show("Log file doesnt exist.");
                    return false;
                }

                return true;

            }
            catch (Exception)
            {
                MessageBox.Show("Load Error");
                return false;
            }
        }

        internal Boolean ExportCsv(string CsvFpath)
        {
            try
            {
                System.IO.StreamWriter csvFileWriter = new System.IO.StreamWriter(CsvFpath, false);
                int countColumn = dataGridView1.ColumnCount - 1;
                int iColCount = dataGridView1.Columns.Count;

                foreach (DataGridViewRow dataRowObject in dataGridView1.Rows)
                {
                    //Checking for New Row in DataGridView
                    if (!dataRowObject.IsNewRow)
                    {
                        string dataFromGrid = "";

                        dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                        for (int i = 1; i <= countColumn; i++)
                        {
                            dataFromGrid = dataFromGrid + ',' + dataRowObject.Cells[i].Value.ToString();
                        }

                        //Writing Data Rows in File
                        csvFileWriter.WriteLine(dataFromGrid);
                    }
                }


                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
            return true;
        }
    }
}
