﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardioLeaf
{
    /// <summary>
    /// Interaction logic for Log_Control.xaml
    /// </summary>
    public partial class Log_Control : UserControl
    {

        datatableControl dtController;
        ChartControl LogChartControl = new ChartControl(1);
        private System.Collections.ArrayList points = new System.Collections.ArrayList();
        const int MAX_POINTS = 300;

        Boolean forwardChartMove = false;
        Boolean reverseChartMove = false;


        private System.Windows.Forms.DataVisualization.Charting.Chart logChart = null;

        public Log_Control()
        {
            InitializeComponent();
            dtController = new datatableControl(this); //passing in the pointer to the current class
            datagridFormHost.Child = dtController;
            //LogChartControl.SetChartDisplayMode(1);     //show 1 lead
            ECGFormHost.Child = LogChartControl;
            resetLogChart();
        }

        public void resetLogChart()
        {
            LogChartControl.resetChart();

            btnReverse.IsEnabled = true;
            btnForward.IsEnabled = true;
        }


        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            // Process input if the user clicked OK.
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "CardioLeaf Log files (*.cll)|*.cll";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = openFileDialog1.FileName;
                dtController.importCsv(filepath);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "CardioLeaf Log files (*.cll)|*.cll";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = saveFileDialog1.FileName;
                dtController.ExportCsv(filepath);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dtController.ClearDT();
        }

        private void btnReverse_MouseEnter(object sender, MouseEventArgs e)
        {
            startReverse();
        }

        private void btnReverse_MouseLeave(object sender, MouseEventArgs e)
        {
            stopChartMove();
        }

        private void btnForward_MouseEnter(object sender, MouseEventArgs e)
        {
            startForward();
        }

        private void btnForward_MouseLeave(object sender, MouseEventArgs e)
        {
            stopChartMove();
        }

        private void startForward()
        {
            forwardChartMove = true;
            reverseChartMove = false;
            timerStart();
        }

        private void startReverse()
        {
            reverseChartMove = true;
            forwardChartMove = false;
            timerStart();
        }

        private void stopChartMove()
        {
            forwardChartMove = false;
            reverseChartMove = false;
            timerStop();
        }

        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private void timerStart(){
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (forwardChartMove)
                dtController.down();
            else if (reverseChartMove)
                dtController.up();
            else
                timerStop();
        }

        private void timerStop()
        {
 	        timer.Stop();
        }

        internal void drawGraph(datagridSelectedData dataReceived)
        {
            resetLogChart();
            if (dataReceived.isBeginning)
                btnReverse.IsEnabled = false;
            if (dataReceived.isEnd)
                btnForward.IsEnabled = false;

            LogChartControl.AddToChart(dataReceived.selectedPoints.ToArray<int>());
        }
    }
}


/*
 * todos :
 *  - add a timer function that fires 100 times/sec (aprox rate)
 *  - read choice from table
 *  - load L<=300points at start
 *  - disable buttons that are not useful
*/