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
    /// Interaction logic for Imp_Control.xaml
    /// </summary>
    public partial class Imp_Control : UserControl, ChildControl
    {
        #region Plot data variables
        ChartControl ImpChartControl = new ChartControl(2);
        private System.Collections.ArrayList points = new System.Collections.ArrayList();
        #endregion

        public Imp_Control()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            ChartHost.Child = ImpChartControl;
            ImpChartControl.resetChart();
        }

        public void AddToChart(int[][] values)    //for 2 PPG
        {
            ImpChartControl.AddToChart(values, 2);
        }

        public void Reset() { }
    }
}
