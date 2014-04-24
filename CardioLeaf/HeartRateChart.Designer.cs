namespace CardioLeaf
{
    partial class HeartRateChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        
        /// <summary> 
        /// changing chart modes
        /// </summary>

        //modes
        //1-> only graph 2
        //3-> all theree graphs
        public void SetChartDisplayMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    this.HRChart.ChartAreas[0].Visible = false;
                    this.HRChart.ChartAreas[1].Visible = true;
                    this.HRChart.ChartAreas[2].Visible = false;
                    break;
                case 3:
                    this.HRChart.ChartAreas[0].Visible = true;
                    this.HRChart.ChartAreas[1].Visible = true;
                    this.HRChart.ChartAreas[2].Visible = true;
                    break;
            }
        }
        

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeartRateChart));
            this.HRChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.HRChart)).BeginInit();
            this.SuspendLayout();
            // 
            // HRChart
            // 
            this.HRChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HRChart.BackColor = System.Drawing.Color.Transparent;
            this.HRChart.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.Maximum = 300D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY2.LabelStyle.Enabled = false;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.LineWidth = 2;
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea1";
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.Maximum = 300D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.AxisY.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.LineWidth = 2;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisY2.LabelStyle.Enabled = false;
            chartArea2.AxisY2.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY2.LineWidth = 2;
            chartArea2.AxisY2.MajorGrid.Enabled = false;
            chartArea2.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.BorderColor = System.Drawing.Color.Transparent;
            chartArea2.BorderWidth = 0;
            chartArea2.Name = "ChartArea2";
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisX.LabelStyle.Enabled = false;
            chartArea3.AxisX.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.Maximum = 300D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisY.LabelStyle.Enabled = false;
            chartArea3.AxisY.LineColor = System.Drawing.Color.Silver;
            chartArea3.AxisY.LineWidth = 2;
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea3.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisY2.LabelStyle.Enabled = false;
            chartArea3.AxisY2.LineColor = System.Drawing.Color.Silver;
            chartArea3.AxisY2.LineWidth = 2;
            chartArea3.AxisY2.MajorGrid.Enabled = false;
            chartArea3.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea3.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea3.BackColor = System.Drawing.Color.Transparent;
            chartArea3.BorderColor = System.Drawing.Color.Transparent;
            chartArea3.BorderWidth = 0;
            chartArea3.Name = "ChartArea3";
            this.HRChart.ChartAreas.Add(chartArea1);
            this.HRChart.ChartAreas.Add(chartArea2);
            this.HRChart.ChartAreas.Add(chartArea3);
            this.HRChart.Location = new System.Drawing.Point(0, 0);
            this.HRChart.Margin = new System.Windows.Forms.Padding(0);
            this.HRChart.Name = "HRChart";
            series1.BorderColor = System.Drawing.Color.Transparent;
            series1.BorderWidth = 0;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.Cyan;
            series1.Name = "Series1";
            series1.SmartLabelStyle.Enabled = false;
            series2.BorderColor = System.Drawing.Color.Transparent;
            series2.BorderWidth = 0;
            series2.ChartArea = "ChartArea2";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Color = System.Drawing.Color.Red;
            series2.Name = "series2";
            series2.SmartLabelStyle.Enabled = false;
            series3.BorderColor = System.Drawing.Color.Transparent;
            series3.BorderWidth = 0;
            series3.ChartArea = "ChartArea3";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Color = System.Drawing.Color.Green;
            series3.Name = "series3";
            series3.SmartLabelStyle.Enabled = false;
            this.HRChart.Series.Add(series1);
            this.HRChart.Series.Add(series2);
            this.HRChart.Series.Add(series3);
            this.HRChart.Size = new System.Drawing.Size(265, 260);
            this.HRChart.TabIndex = 0;
            this.HRChart.Text = "chart1";
            // 
            // HeartRateChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.HRChart);
            this.Name = "HeartRateChart";
            this.Size = new System.Drawing.Size(300, 300);
            ((System.ComponentModel.ISupportInitialize)(this.HRChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart HRChart;
    }
}
