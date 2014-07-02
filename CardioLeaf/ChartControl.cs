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
    public partial class ChartControl : UserControl
    {
        private int ChartMode;
        private int ChartCursor;
        private int FlagLocation;

        public ChartControl()
        {
            InitializeComponent();
            //throw new Exception("improper Chart initialization. No Mode defined.");
            //TODO : uncomment this section to make sure that the code has been completely ported over to the new app settings
        }

        public ChartControl(int mode)
        {
            InitializeComponent();
            this.ChartMode = mode;
            this.SetChartDisplayMode(mode);
            ChartCursor = 0;
        }
        
        
        /// <summary> 
        /// changing chart modes
        /// </summary>

        //modes
        //1     -> only graph 1
        //3     -> first three graphs
        //12    -> all twelve graphs
        internal void SetChartDisplayMode(int mode)
        {
            DisableLabels();
            switch (mode)
            {
                case 1:
                    this.modularChart.ChartAreas[0].Visible = true;
                    this.modularChart.ChartAreas[1].Visible = false;
                    this.modularChart.ChartAreas[2].Visible = false;
                    this.modularChart.ChartAreas[3].Visible = false;
                    this.modularChart.ChartAreas[4].Visible = false;
                    this.modularChart.ChartAreas[5].Visible = false;
                    this.modularChart.ChartAreas[6].Visible = false;
                    this.modularChart.ChartAreas[7].Visible = false;
                    this.modularChart.ChartAreas[8].Visible = false;
                    this.modularChart.ChartAreas[9].Visible = false;
                    this.modularChart.ChartAreas[10].Visible = false;
                    this.modularChart.ChartAreas[11].Visible = false;

                    break;

                case 2:
                    this.modularChart.ChartAreas[0].Visible = true;
                    this.modularChart.ChartAreas[1].Visible = true;
                    this.modularChart.ChartAreas[2].Visible = false;
                    this.modularChart.ChartAreas[3].Visible = false;
                    this.modularChart.ChartAreas[4].Visible = false;
                    this.modularChart.ChartAreas[5].Visible = false;
                    this.modularChart.ChartAreas[6].Visible = false;
                    this.modularChart.ChartAreas[7].Visible = false;
                    this.modularChart.ChartAreas[8].Visible = false;
                    this.modularChart.ChartAreas[9].Visible = false;
                    this.modularChart.ChartAreas[10].Visible = false;
                    this.modularChart.ChartAreas[11].Visible = false;

                    break;

                case 3:
                    this.modularChart.ChartAreas[0].Visible = true;
                    this.modularChart.ChartAreas[1].Visible = true;
                    this.modularChart.ChartAreas[2].Visible = true;
                    this.modularChart.ChartAreas[3].Visible = false;
                    this.modularChart.ChartAreas[4].Visible = false;
                    this.modularChart.ChartAreas[5].Visible = false;
                    this.modularChart.ChartAreas[6].Visible = false;
                    this.modularChart.ChartAreas[7].Visible = false;
                    this.modularChart.ChartAreas[8].Visible = false;
                    this.modularChart.ChartAreas[9].Visible = false;
                    this.modularChart.ChartAreas[10].Visible = false;
                    this.modularChart.ChartAreas[11].Visible = false;

                    break;

                case 12:
                    this.modularChart.ChartAreas[0].Visible = true;
                    this.modularChart.ChartAreas[1].Visible = true;
                    this.modularChart.ChartAreas[2].Visible = true;
                    this.modularChart.ChartAreas[3].Visible = true;
                    this.modularChart.ChartAreas[4].Visible = true;
                    this.modularChart.ChartAreas[5].Visible = true;
                    this.modularChart.ChartAreas[6].Visible = true;
                    this.modularChart.ChartAreas[7].Visible = true;
                    this.modularChart.ChartAreas[8].Visible = true;
                    this.modularChart.ChartAreas[9].Visible = true;
                    this.modularChart.ChartAreas[10].Visible = true;
                    this.modularChart.ChartAreas[11].Visible = true;
                    EnableECGLabels();
                    break;

                default:
                    //by default enable all the charts.
                    this.modularChart.ChartAreas[0].Visible = true;
                    this.modularChart.ChartAreas[1].Visible = true;
                    this.modularChart.ChartAreas[2].Visible = true;
                    this.modularChart.ChartAreas[3].Visible = true;
                    this.modularChart.ChartAreas[4].Visible = true;
                    this.modularChart.ChartAreas[5].Visible = true;
                    this.modularChart.ChartAreas[6].Visible = true;
                    this.modularChart.ChartAreas[7].Visible = true;
                    this.modularChart.ChartAreas[8].Visible = true;
                    this.modularChart.ChartAreas[9].Visible = true;
                    this.modularChart.ChartAreas[10].Visible = true;
                    this.modularChart.ChartAreas[11].Visible = true;

                    break;

            }
        }

        private void DisableLabels()
        {
            foreach (var chartArea in this.modularChart.ChartAreas)
            {
                chartArea.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            }
        }

        private void EnableECGLabels()
        {
            foreach (var chartArea in this.modularChart.ChartAreas)
            {
                chartArea.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            }
            this.scaleLabel.Visible = true;
        }

        internal void EnableAccLabels()
        {
            for(int i=0;i<3;i++)
                this.modularChart.ChartAreas[i].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            this.modularChart.ChartAreas[0].AxisX.Title = "X Axis";
            this.modularChart.ChartAreas[1].AxisX.Title = "Y Axis";
            this.modularChart.ChartAreas[2].AxisX.Title = "Z Axis";
        }

        internal void EnableSummaryLabels()
        {
            for (int i = 0; i < 3; i++)
                this.modularChart.ChartAreas[i].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            this.modularChart.ChartAreas[0].AxisX.Title = "ECG";
            this.modularChart.ChartAreas[1].AxisX.Title = "Impedence";
            this.modularChart.ChartAreas[2].AxisX.Title = "PPG";
            this.scaleLabel.Visible = true;
        }

        internal void resetChart()
        {
            foreach (var series in this.modularChart.Series)
            {
                series.Points.Clear();
            }
            StaticVariables.osciloscopeCnt = 0;
            ChartCursor = 0;
        }

        internal void AddToChart(int val)
        {
            this.modularChart.Series[0].Points.AddY(val);
            ScrollCharts(1);
            UpdateChartScale();
        }

        internal void AddToChart(int[] points)
        {
            foreach ( int val in points)
                this.modularChart.Series[0].Points.AddY(val);
            ScrollCharts(1);
            UpdateChartScale();
        }

        internal void AddToChart(int[][] values,int type)   //in osciloscope style
        {
            if (type != 2 && type != 3 && type != 12)
                return;
            foreach (int[] points in values)
            {
                for (int i = 0; i < type; i++)
                    this.modularChart.Series[i * 2].Points.Add(points[i]);              //add to primary series
                
                if (this.modularChart.Series[0].Points.Count() >= CLSettings.ChartWidth)
                {
                    for (int i = 0; i < type; i++)
                    {
                        this.modularChart.Series[(i * 2) + 1].Points.Clear();           //clear secondary series - just as a precaution
                        for (int j = 50; j < CLSettings.ChartWidth; j++)
                        {
                            double yValue = this.modularChart.Series[i * 2].Points[j].YValues[0];
                            this.modularChart.Series[(i * 2) + 1].Points.AddXY(j,yValue);
                            //copy over the 50th to the last element of the primary series within the specified rande to the secondary series
                        }
                        this.modularChart.Series[i * 2].Points.Clear();     //clear primary series
                    }
                }
                if (this.modularChart.Series[1].Points.Count() > 0)
                {
                    for (int i = 0; i < type; i++)
                        this.modularChart.Series[i * 2 + 1].Points.RemoveAt(0);         //clear out one point from the secondary series
                }

                
                ChartCursor++;
                if (ChartCursor > CLSettings.ChartWidth)
                {
                    ChartCursor = 0;
                }

                if (ChartCursor == FlagLocation) 
                {
                    RemoveUserMarker();
                }

            }
            UpdateChartScale();
        }

        internal void AddToChart_sliding(int[][] values, int type)
        {
            if (type != 2 && type != 3 && type != 12)
                return;
            foreach (int[] points in values)
            {
                //if (points.Length != type)
                //    continue;
                for (int i = 0; i < type; i++)
                    this.modularChart.Series[i * 2].Points.AddY(points[i]);
                ScrollCharts(type);
            }
            UpdateChartScale();
        }

        internal void AddToChart(double[][] activityData, int type)
        {
            if (type != 3)
                return;
            foreach (double[] points in activityData)
            {
                for (int i = 0; i < type; i++)
                    this.modularChart.Series[i * 2].Points.AddY(points[i]);
                ScrollCharts(type);
            }
            UpdateChartScale();
        }

        internal void UpdateChartScale()
        {
            foreach (var chartarea in this.modularChart.ChartAreas)
                chartarea.RecalculateAxesScale();
        }

        private void ScrollCharts(int Mode)
        {

            switch (Mode)
            {
                case 1:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.ChartWidth)
                    {
                        this.modularChart.Series[0].Points.RemoveAt(0);
                    }
                    break;

                case 2:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.ChartWidth)
                    {
                        for (int i = 0; i < 2; i++)
                            this.modularChart.Series[i * 2].Points.RemoveAt(0);
                    }
                    break;

                case 3:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.ChartWidth)
                    {
                        for (int i=0;i<3;i++)
                            this.modularChart.Series[i*2].Points.RemoveAt(0);
                    }
                    break;

                case 12:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.ChartWidth)
                    {
                        for (int i=0;i<12;i++)
                            this.modularChart.Series[i*2].Points.RemoveAt(0);
                    }
                    break;

                default:
                    foreach (var series in this.modularChart.Series)
                    {
                        if (series.Points.Count >= CLSettings.ChartWidth)
                        {
                            series.Points.RemoveAt(0);
                        }
                    }
                    break;

            }
        }

        internal void UpdateChartWidth()
        {
            foreach (var chartarea in this.modularChart.ChartAreas)
                chartarea.AxisX.Maximum = CLSettings.ChartWidth;
        }

        internal void SetUserMarker()
        {
            int CurrrentFlag;

            FlagLocation = ChartCursor;
            CurrrentFlag = ((ChartCursor * (this.modularChart.Width - 120)) / CLSettings.ChartWidth) + 60;
            
            userMarker.Visible = true;
            userMarker.Location = new System.Drawing.Point(CurrrentFlag, 20);
            flag.Visible = true;
            flag.Location = new System.Drawing.Point(CurrrentFlag, 2);

        }

        internal void RemoveUserMarker()
        {
            userMarker.Visible = false;
            flag.Visible = false;
        }
    }
}
