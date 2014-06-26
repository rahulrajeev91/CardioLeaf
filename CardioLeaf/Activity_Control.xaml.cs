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
    /// Interaction logic for Activity_Control.xaml
    /// </summary>
    public partial class Activity_Control : UserControl, ChildControl
    {
        ChartControl ActivityChart = new ChartControl(3);
        GraphControl ActGraphControl = new GraphControl(0);
        //private double[] acc = new double[3];

        public Activity_Control()
        {
            InitializeComponent();
        }

        internal void AddToChart(double[][] activityData)
        {
            ActivityChart.AddToChart(activityData,3);
        }

        public void AddToActGraph(int ActVal)
        {
            ActGraphControl.AddToGraph(ActVal, 0);
        }

        public void Reset() { }


        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ChartHost.Child = ActivityChart;
            ActivityChart.resetChart();
            ActivityChart.EnableAccLabels();

            ActGraphHost.Child = ActGraphControl;
            ActGraphControl.resetGraph();

            ActGraphControl.EnableGraphLabel(Page.Activity);
        }

        internal void UpdateChartWidth()
        {
            ActivityChart.UpdateChartWidth();
        }
    }
}
 