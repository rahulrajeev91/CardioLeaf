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
    public partial class HearRate_Control : UserControl, ChildControl
    {
        

        #region Plot data variables
        
        ChartControl ECGChartControl = new ChartControl(12);
        private System.Collections.ArrayList points = new System.Collections.ArrayList();
        #endregion

        public HearRate_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ChartHost.Child = ECGChartControl;
            ECGChartControl.resetChart();
        }

        public void AddToChart(int val1, int val2, int val3 )    //for 3 lead data
        {
            //chart1.Series[0].Points.AddY(val1);
            //chart1.Series[1].Points.AddY(val2);
            //chart1.Series[2].Points.AddY(val3);
            //ScrollCharts(3);
        }

        public void AddToChart(int[][] values)    //for 12 lead data
        {
            ECGChartControl.AddToChart(values,12);
        }

        public void Reset() { }
    }
}
