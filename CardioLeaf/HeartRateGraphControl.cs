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
    public partial class HeartRateGraphControl : UserControl
    {
        public HeartRateGraphControl()
        {
            InitializeComponent();
            resetGraph();
        }

        public HeartRateGraphControl(int i)
        {
            InitializeComponent();
            if (i == 1)
            {
                ChartDesign.Series[1].IsVisibleInLegend = false;
                ChartDesign.Series[2].IsVisibleInLegend = false;
            }
            resetGraph();
        }

        internal void resetGraph()
        {
            ChartDesign.Series[0].Points.Clear();
            ChartDesign.Series[1].Points.Clear();
            ChartDesign.Series[2].Points.Clear();
            ChartDesign.Series[0].Points.AddY(0);
            ChartDesign.Series[1].Points.AddY(0);
            ChartDesign.Series[2].Points.AddY(0);
        }

        internal void AddToGraph(int HRVal)
        {
            ChartDesign.Series[0].Points.AddY(HRVal);
            ScrollGraph();
        }

        private void ScrollGraph()
        {
            while (ChartDesign.Series[0].Points.Count > CLSettings.HRGraphWidth)
                ChartDesign.Series[0].Points.RemoveAt(0);
        }
    }
}
