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

        public GraphControl(int i)
        {
            InitializeComponent();
            resetGraph();

            if (i<0 || i>2)
                return;
            for (int x = 0; x < 3; x++)
            {
                if (x == i)
                    continue;
                ChartDesign.ChartAreas[x].Visible = false;
            }
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

        internal void AddToGraph(int val,int mode)
        {
            if (mode < 0 || mode > 2)
                return;
            ChartDesign.Series[mode].Points.AddY(val);
            ScrollGraph(mode);
        }

        internal void AddToGraph(int[] val)
        {
            for (int i = 0; i < 3; i++)
                ChartDesign.Series[i].Points.AddY(val[i]);
            ScrollGraph();
        }

        private void ScrollGraph()
        {
            while (ChartDesign.Series[0].Points.Count > CLSettings.HRGraphWidth)
            {
                for(int i=0;i<3;i++)
                    ChartDesign.Series[i].Points.RemoveAt(0);
            }
        }

        private void ScrollGraph(int mode)
        {
            while (ChartDesign.Series[mode].Points.Count > CLSettings.HRGraphWidth)
                ChartDesign.Series[mode].Points.RemoveAt(0);
        }
    }
}
