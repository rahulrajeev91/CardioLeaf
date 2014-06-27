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
    /// Interaction logic for Ppg_Control.xaml
    /// </summary>
    public partial class Ppg_Control : UserControl, ChildControl
    {
        #region Plot data variables
        ChartControl PpgChartControl = new ChartControl(2);
        GraphControl PpgGraphControl = new GraphControl(0);
        private System.Collections.ArrayList points = new System.Collections.ArrayList();        
        #endregion

        public Ppg_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ChartHost.Child = PpgChartControl;
            PpgChartControl.resetChart();

            PpgGraphHost.Child = PpgGraphControl;
            PpgGraphControl.resetGraph();

            PpgGraphControl.EnableGraphLabel(Page.Ppg);
        }

        public void AddToChart(int[][] values)    //for 2 PPG
        {
            PpgChartControl.AddToChart(values, 2);
        }

        public void AddToPpgGraph(int PpgVal)
        {
            PpgGraphControl.AddToGraph(PpgVal, 0);
        }

        public void Reset()
        {
            PpgChartControl.resetChart();
            PpgGraphControl.resetGraph();
        }

        internal void UpdateChartWidth()
        {
            PpgChartControl.UpdateChartWidth();
        }
    }
}
