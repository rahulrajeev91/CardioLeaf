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
        GraphControl HRGraphControl = new GraphControl(0);
        #endregion

        public HearRate_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ChartHost.Child = ECGChartControl;
            ECGChartControl.resetChart();

            HRGraphHost.Child = HRGraphControl;
            HRGraphControl.resetGraph();
        }

        public void AddToChart(int[][] values)    //for 12 lead data
        {
            ECGChartControl.AddToChart(values,12);
        }

        public void AddToHRGraph(int HRVal)
        {
            HRGraphControl.AddToGraph(HRVal,0);
        }

        public void Reset() { }
    }
}
