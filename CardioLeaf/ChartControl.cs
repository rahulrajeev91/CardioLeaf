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

        internal void resetChart()
        {
            foreach (var series in this.modularChart.Series)
            {
                series.Points.Clear();
                series.Points.Add(0);
            }
        }

        internal void AddToChart(int[] points)
        {
            foreach ( int val in points)
                this.modularChart.Series[0].Points.AddY(val);
            ScrollCharts(1);
            UpdateChartScale();
        }

        internal void AddToChart(int[][] values,int type)
        {
            if (type != 3 && type != 12)
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
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.GraphWidth)
                    {
                        this.modularChart.Series[0].Points.RemoveAt(0);
                    }
                    break;

                case 3:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.GraphWidth)
                    {
                        for (int i=0;i<3;i++)
                            this.modularChart.Series[i*2].Points.RemoveAt(0);
                    }
                    break;

                case 12:
                    while (this.modularChart.Series[0].Points.Count >= CLSettings.GraphWidth)
                    {
                        for (int i=0;i<12;i++)
                            this.modularChart.Series[i*2].Points.RemoveAt(0);
                    }
                    break;

                default:
                    foreach (var series in this.modularChart.Series)
                    {
                        if (series.Points.Count >= CLSettings.GraphWidth)
                        {
                            series.Points.RemoveAt(0);
                        }
                    }
                    break;

            }
            
        }
    }
}
