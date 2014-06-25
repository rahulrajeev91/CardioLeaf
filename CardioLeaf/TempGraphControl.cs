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
    public partial class TempGraphControl : UserControl
    {
        public TempGraphControl()
        {
            InitializeComponent();
            resetGraph();
        }

        internal void resetGraph()
        {
            ChartDesign.Series[0].Points.Clear();
            ChartDesign.Series[0].Points.AddY(0);
        }

        internal void AddToGraph(double convertedTemp)
        {
            ChartDesign.Series[0].Points.AddY(convertedTemp);
            ScrollGraph();
        }

        private void ScrollGraph()
        {
            while (ChartDesign.Series[0].Points.Count > CLSettings.tempGraphWidth)
                ChartDesign.Series[0].Points.RemoveAt(0);
        }
    }
}
