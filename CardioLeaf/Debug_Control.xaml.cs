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
    /// Interaction logic for Debug_Control.xaml
    /// </summary>
    public partial class Debug_Control : UserControl, ChildControl
    {
        ChartControl debugChart = new ChartControl(1);

        public Debug_Control()
        {
            InitializeComponent();
            debugChartHost.Child = debugChart;
        }

        public void Reset()
        {
            //do something
        }

        public void AddToChart(int val)
        {
            debugChart.AddToChart(val);
        }

        public void AddToChart(int[] val)
        {
            debugChart.AddToChart(val);
        }

        internal void UpdateChartWidth()
        {
            debugChart.UpdateChartWidth();
        }
    }
}
