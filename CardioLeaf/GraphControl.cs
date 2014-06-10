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
    public partial class GraphControl : UserControl
    {
        public GraphControl()
        {
            InitializeComponent();
            resetGraph();
        }

        internal void resetGraph()
        {
            ChartDesign.Series[0].Points.Clear();
            ChartDesign.Series[0].Points.AddY(0);
            //ChartDesign.Series[0].Points.AddY(32.5);
            //ChartDesign.Series[0].Points.AddY(33.5);
            //ChartDesign.Series[0].Points.AddY(37);
            //ChartDesign.Series[0].Points.AddY(37.25);
            //ChartDesign.Series[0].Points.AddY(36.5);
            //ChartDesign.Series[0].Points.AddY(42.5);
            //ChartDesign.Series[0].Points.AddY(40.5);
        }
    }
}
