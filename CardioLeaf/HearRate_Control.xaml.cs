using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardioLeaf
{
    /// <summary>
    /// Interaction logic for HearRate_Control.xaml
    /// </summary>
    public partial class HearRate_Control : UserControl
    {
        

        #region Plot data variables
        
        HeartRateChart HRChartControl = new HeartRateChart();
        private System.Collections.ArrayList points = new System.Collections.ArrayList();
        const int MAX_POINTS = 300;

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();

        #endregion

        public HearRate_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            HRChartControl.SetChartDisplayMode(3);     //show all 3 leads
            ChartHost.Child = HRChartControl;

            chart1 = HRChartControl.HRChart;
            resetChart();
        }

        public void resetChart()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

            chart1.Series[0].Points.Add(0);
            chart1.Series[1].Points.Add(0);
            chart1.Series[2].Points.Add(0);
        }


        #region Chart Functions

        public void AddToChart(uint val)
        {
            chart1.Series[0].Points.AddY(val);
            ScrollCharts(1);
        }

        public void AddToChart(uint val1, uint val2, uint val3 )    //for 3 lead data
        {
            chart1.Series[0].Points.AddY(val1);
            chart1.Series[1].Points.AddY(val2);
            chart1.Series[2].Points.AddY(val3);
            ScrollCharts(3);
        }

        private void ScrollCharts(int cnt)
        {
            while (chart1.Series[0].Points.Count >= MAX_POINTS)
            {
                chart1.Series[0].Points.RemoveAt(0);
                if (cnt == 3)
                {
                    chart1.Series[1].Points.RemoveAt(0);
                    chart1.Series[2].Points.RemoveAt(0);
                }
            }
        }

        #endregion
    }
}
