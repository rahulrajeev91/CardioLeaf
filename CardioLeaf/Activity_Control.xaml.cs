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
        //private double[] acc = new double[3];

        

        public Activity_Control()
        {
            InitializeComponent();
            ActivityChartHost.Child = ActivityChart;
        }

        internal void AddToChart(double[][] activityData)
        {
            ActivityChart.AddToChart(activityData,3);
        }

        public void Reset()
        {
        }

        
    }
}
 