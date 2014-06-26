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
    /// Interaction logic for Settings_Control.xaml
    /// </summary>
    public partial class Settings_Control : UserControl, ChildControl
    {
        private MainWindow mainWindow;

        public Settings_Control()
        {
            InitializeComponent();
        }

        public void updateMainWindowInstance(MainWindow mainWindow)
        {
            // TODO: Complete member initialization
            this.mainWindow = mainWindow;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        private void cbDevMode_Click(object sender, RoutedEventArgs e)
        {
            if (cbDevMode.IsChecked == true)
            {
                mainWindow.DebugTab.IsEnabled = true;
                mainWindow.DebugTab.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.DebugTab.IsEnabled = false;
                mainWindow.DebugTab.Visibility = Visibility.Collapsed;
            }

        }

        private void btnChangeDataRate_Click(object sender, RoutedEventArgs e)
        {
            int dataRate;
            try
            {
                dataRate = int.Parse(tbDataRate.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input. Please enter an integer.");
                tbDataRate.Text = CLSettings.DataRate.ToString();
                return;
            }

            if (dataRate <= 0 || dataRate >= 500)
            {
                MessageBox.Show("Invalid input. Please enter an integer between 0 and 500.");
                tbDataRate.Text = CLSettings.DataRate.ToString();
                return;
            }
            CLSettings.DataRate = dataRate;
        }

        private void cbHRWidth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdateHRGraphWidth.Visibility = Visibility.Visible;
        }

        private void cbTempWidth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdateTempGraphWidth.Visibility = Visibility.Visible;
                
        }

        private void cbHRLead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdateHRLead.Visibility = Visibility.Visible;
        }

        private void cbPPGLead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdatePPGLead.Visibility = Visibility.Visible;
        }

        private void cbRRLead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdateRRLead.Visibility = Visibility.Visible;
        }

        private void cdECGWidth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                btnUpdateECGWidth.Visibility = Visibility.Visible;
        }

        /*
         * update buttons
         */
        private void btnUpdateHRGraphWidth_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.HRGraphWidth = int.Parse(cbHRWidth.Text);
            btnUpdateHRGraphWidth.Visibility = Visibility.Hidden;
        }

        private void btnUpdateTempGraphWidth_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.tempGraphWidth = int.Parse(cbTempWidth.Text);
            btnUpdateTempGraphWidth.Visibility = Visibility.Hidden;
        }

        private void btnUpdateHRLead_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.HRLead = cbHRLead.SelectedIndex;
            btnUpdateHRLead.Visibility = Visibility.Hidden;
        }

        private void btnUpdatePPGLead_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.PPGLead = cbPPGLead.SelectedIndex;
            btnUpdatePPGLead.Visibility = Visibility.Hidden;
        }

        private void btnUpdateRRLead_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.RRLead = cbRRLead.SelectedIndex;
            btnUpdateRRLead.Visibility = Visibility.Hidden;
        }

        private void btnFolderSelect_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbLogPath.Text = fbd.SelectedPath;
                CLSettings.logFilePath = fbd.SelectedPath;
            }
        }

        private void btnUpdateECGWidth_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.ChartWidth = int.Parse(cdECGWidth.Text);
            btnUpdateECGWidth.Visibility = Visibility.Hidden;
            mainWindow.UpdateChartWidth();
        }

        private void cbLogging_Click(object sender, RoutedEventArgs e)
        {
            CLSettings.loggingEnabled = (bool)cbLogging.IsChecked;
        }
    }
}
