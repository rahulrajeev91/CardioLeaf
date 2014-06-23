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
    /// Interaction logic for Summary_Control.xaml
    /// </summary>
    public partial class Summary_Control : UserControl, ChildControl
    {
        ChartControl SummaryChartControl = new ChartControl();
        HeartRateGraphControl SummaryGraphControl = new HeartRateGraphControl();

        public Summary_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            SummaryChartControl.SetChartDisplayMode(3);     //show only 1 lead
            ChartHost.Child = SummaryChartControl;

            GraphHost.Child = SummaryGraphControl;
        }

        public void AddToChart(int[][] values)    //for 12 lead data
        {
            SummaryChartControl.AddToChart(values, 3);
        }

        public void Reset() { }
    }
}
