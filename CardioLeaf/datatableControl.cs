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

        private bool loadingFlag = false;

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
            loadingFlag = true;

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
                    
                    loadingFlag = false;
                    return true;
                }
                else
                    MessageBox.Show("Log file doesnt exist.");
            }
            catch (Exception)
            {
                MessageBox.Show("Load Error");
                
            }
            
            loadingFlag = false; 
                return false;
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

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (loadingFlag)
                return;
            if (e.StateChanged != System.Windows.Forms.DataGridViewElementStates.Selected)
                return;

            col = e.Cell.ColumnIndex;
            row = e.Cell.RowIndex;

            if (row < 0 || col < 0)
                return;

            int val = 0;
            datagridSelectedData dataToSend = new datagridSelectedData();

            if (dataGridView1.Rows.Count <= row + CLSettings.GraphWidth)
                dataToSend.isEnd = true;

            if (row == 0)
                dataToSend.isBeginning = true;

            for (int i = row; i < dataGridView1.Rows.Count && dataToSend.selectedPoints.Count < CLSettings.GraphWidth; i++)
            {
                try
                {
                    val = int.Parse(dataGridView1.Rows[i].Cells[col].Value.ToString());
                }
                catch (Exception) { }

                dataToSend.selectedPoints.Add(val);
            }

            parent_LogControl.drawGraph(dataToSend);
        }



        internal void down()
        {
            if (row >= dataGridView1.Rows.Count || dataGridView1.Rows.Count==0)
                return;
            dataGridView1.CurrentCell = dataGridView1.Rows[row + 1].Cells[col];
        }

        internal void up()
        {
            if (row <= 0 || dataGridView1.Rows.Count == 0)
                return;
            dataGridView1.CurrentCell = dataGridView1.Rows[row - 1].Cells[col];
        }
    }
}
