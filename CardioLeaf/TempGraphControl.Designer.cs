namespace CardioLeaf
{
    partial class TempGraphControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ChartDesign = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ChartDesign)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartDesign
            // 
            this.ChartDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartDesign.BackColor = System.Drawing.Color.Transparent;
            this.ChartDesign.BackImageTransparentColor = System.Drawing.Color.Transparent;
            this.ChartDesign.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.ChartDesign.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
            chartArea1.AxisX.LineWidth = 2;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorTickMark.Size = 5F;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Time (s)";
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(164)))), ((int)(((byte)(164)))));
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.Maximum = 45D;
            chartArea1.AxisY.Minimum = 15D;
            chartArea1.AxisY.MinorGrid.Enabled = true;
            chartArea1.AxisY.MinorGrid.Interval = 0.2D;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.MinorTickMark.Enabled = true;
            chartArea1.AxisY.MinorTickMark.Interval = 1D;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.Title = "Temperature (Celsius)";
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackImageTransparentColor = System.Drawing.Color.Transparent;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.ChartDesign.ChartAreas.Add(chartArea1);
            this.ChartDesign.Location = new System.Drawing.Point(0, 0);
            this.ChartDesign.Name = "ChartDesign";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(132)))), ((int)(((byte)(230)))));
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            this.ChartDesign.Series.Add(series1);
            this.ChartDesign.Size = new System.Drawing.Size(457, 342);
            this.ChartDesign.TabIndex = 0;
            this.ChartDesign.Text = "chart1";
            // 
            // TempGraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChartDesign);
            this.Name = "TempGraphControl";
            this.Size = new System.Drawing.Size(533, 394);
            ((System.ComponentModel.ISupportInitialize)(this.ChartDesign)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ChartDesign;
    }
}
