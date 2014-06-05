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
        Log_Control parent_LogControl = null;
        private int row, col;

        public datatableControl(Log_Control parent)
        {
            InitializeComponent();
            parent_LogControl = parent;
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

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //if (row < 0 || col < 0)
            //    return;

            //int val = 0;
            //datagridSelectedData dataToSend = new datagridSelectedData();

            //if (dataGridView1.Rows.Count <= row + 300)
            //    dataToSend.isEnd = true;

            //if (row == 0)
            //    dataToSend.isBeginning = true;

            //for (int i = row; i < dataGridView1.Rows.Count && dataToSend.selectedPoints.Count < 300; i++)
            //{
            //    val = int.Parse(dataGridView1.Rows[i].Cells[col].Value.ToString());
            //    dataToSend.selectedPoints.Add(val);
            //}

            //parent_LogControl.drawGraph(dataToSend);

        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.StateChanged != System.Windows.Forms.DataGridViewElementStates.Selected)
                return;

            col = e.Cell.ColumnIndex;
            row = e.Cell.RowIndex;

            if (row < 0 || col < 0)
                return;

            int val = 0;
            datagridSelectedData dataToSend = new datagridSelectedData();

            if (dataGridView1.Rows.Count <= row + 300)
                dataToSend.isEnd = true;

            if (row == 0)
                dataToSend.isBeginning = true;

            for (int i = row; i < dataGridView1.Rows.Count && dataToSend.selectedPoints.Count < 300; i++)
            {
                val = int.Parse(dataGridView1.Rows[i].Cells[col].Value.ToString());
                dataToSend.selectedPoints.Add(val);
            }

            parent_LogControl.drawGraph(dataToSend);
        }



        internal void down()
        {
            if (row >= dataGridView1.Rows.Count-10)
                return;
            dataGridView1.CurrentCell = dataGridView1.Rows[row + 10].Cells[col];
        }

        internal void up()
        {
            if (row <= 10)
                return;
            dataGridView1.CurrentCell = dataGridView1.Rows[row - 10].Cells[col];
        }
    }
}
