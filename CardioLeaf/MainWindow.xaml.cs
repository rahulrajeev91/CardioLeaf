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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Local Variables
        Summary_Control SummaryPage = null;
        HearRate_Control HRPage = null;
        Temperature_Control TempPage = null;
        Activity_Control ActivityPage = null;
        Settings_Control SettingsPage = null;

        enum Page { Summary, HeartRate, Activity, Temp, Settings, Unknown };     //models the page tabs
        Page CurrentPage;

        #endregion

        #region initialization functions
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentPage = Page.Unknown;
            ChangePage(Page.Summary);
            
        }

        #endregion

        #region GUI Event Trigers

        private void cbPort_DropDownOpened(object sender, EventArgs e)
        {

        }

        private void ConnectDisconnectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region onclose function

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
		
		#endregion

        #region TabControl Functions

        private bool ChangePage(Page topage)
        {
            if (CurrentPage != Page.Unknown && CurrentPage == topage) return false;

            DataGrid.Children.Clear();

            SetTabStyle(CurrentPage,true); //set to default
            SetTabStyle(topage, false); //set to default

            switch (topage)
            {
                case Page.Summary:
                    if (SummaryPage == null)
                        SummaryPage = new Summary_Control();
                    DataGrid.Children.Add(SummaryPage);
                    break;
                case Page.HeartRate:
                    if (HRPage == null)
                        HRPage = new HearRate_Control();
                    DataGrid.Children.Add(HRPage);
                    break;
                case Page.Activity:
                    if (ActivityPage == null)
                        ActivityPage = new Activity_Control();
                    DataGrid.Children.Add(ActivityPage);
                    break;
                case Page.Temp:
                    if (TempPage == null)
                        TempPage = new Temperature_Control();
                    DataGrid.Children.Add(TempPage);
                    break;
                case Page.Settings:
                    if (SettingsPage == null)
                        SettingsPage = new Settings_Control();
                    DataGrid.Children.Add(SettingsPage);
                    break;
                default:
                    ChangePage(Page.Summary);
                    break;
            }
            CurrentPage = topage;
            return true;
        }

        //
        // Summary:
        //     Changes the tab style to default or selected 
        //
        // Parameters:
        //   page:
        //     The page og enum Page type
        //   toDefault:
        //     true to change to default or false if change to selected  
        //
        // Returns:
        //     The clipping geometry.
        private bool SetTabStyle(Page page, Boolean toDefault)
        {
            
            switch (page)
            {
                case Page.Summary:
                    SetTabStyle(SummaryTab, toDefault);
                    break;
                case Page.HeartRate:
                    SetTabStyle(HeartRateTab, toDefault);
                    break;
                case Page.Activity:
                    SetTabStyle(ActivityTab, toDefault);
                    break;
                case Page.Temp:
                    SetTabStyle(TempTab, toDefault);
                    break;
                case Page.Settings:
                    SetTabStyle(SettingsTab, toDefault);
                    break;

            }
            return true;
        }

        //sets the style of the grid
        private bool SetTabStyle(Grid tab, Boolean toDefault)  
        {   
            if (toDefault)
                tab.SetResourceReference(Control.StyleProperty, "GridStyle_hover");
            else
                tab.SetResourceReference(Control.StyleProperty, "GridStyle_Selected");
            return true;
        }

        private void SummaryTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Summary);
        }

        private void HeartRateTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.HeartRate);
        }

        private void ActivityTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Activity);
        }

        private void TempTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Temp);
        }

        private void SettingsTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Settings);
        }
		
		
        #endregion


    }
}
