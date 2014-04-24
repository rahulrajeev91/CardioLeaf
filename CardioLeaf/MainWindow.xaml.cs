using System;
using System.IO.Ports;
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
        // Variables

        #region Tab Control variables
        Summary_Control SummaryPage = null;
        HearRate_Control HRPage = null;
        Temperature_Control TempPage = null;
        Activity_Control ActivityPage = null;
        Settings_Control SettingsPage = null;

        enum Page { Summary, HeartRate, Activity, Temp, Settings, Unknown };     //models the page tabs
        Page CurrentPage;
        #endregion

        #region timer variables
        private int counter = 0;
        //private int datacnt = 0;
        //private int fallCounter = 0;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Threading.DispatcherTimer oneSecStep = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region connection variables

        private System.IO.Ports.SerialPort serialPort = new SerialPort();
        private const int BAUD_RATE = 115200;

        enum connectionStatus
        {
            connected,
            disconnected
        }
        connectionStatus connection = connectionStatus.disconnected;

        #endregion

        #region parsing variables

        enum ParseStatus            //following PacketformatV2
        {
            idle,
            header2,
            length,
            type,
            subType,
            control,
            read,
            alert,
            contDataPayload,
        }
        ParseStatus parseStep = ParseStatus.idle;
        #endregion

        // Methods

        #region initialization functions
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentPage = Page.Unknown;
            ChangePage(Page.Summary);
            InitializeTimer();
            UpdateComPortList();
        }

        private void InitializeTimer()
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
            oneSecStep.Tick += new EventHandler(oneSecStep_Tick);
            oneSecStep.Interval = new TimeSpan(0, 0, 1);
            oneSecStep.Start();
        }
        #endregion

        #region Connection Functions

        private void UpdateComPortList()
        {
            cbPort.Items.Clear();
            cbPort.Items.Add("None");
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cbPort.Items.Add(s);
            }
            cbPort.SelectedIndex = 0;
        }

        private Boolean Disconnect()
        {
            if (comPortClose())
            {
                connection = connectionStatus.disconnected;
                return true;
            }
            else
                return false;
        }
        
        private bool comPortClose()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            else
                return true;
        }

        private Boolean Connect()
        {
            if (ComPortOpen())
            {
                if (serialPort.IsOpen)
                {
                    connection = connectionStatus.connected;
                    return true;
                }
            }
            return false;
        }

        private bool ComPortOpen()
        {
            string portName = cbPort.Text;
            if (portName.CompareTo("None") == 0)
                return false;
            serialPort.PortName = portName;
            serialPort.BaudRate = BAUD_RATE;

            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region Timer functions

        private void timer_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter >= 10000)
                //prevent overflow
                resetCounter();
            timerTick.Content = counter;
            //if (connection == connectionStatus.connected)
            //{
            //    ParseData();
            //    if (points.Count > 0)
            //        foreach (uint val in points)
            //        {

            //            /*//debug function
            //            datacnt++;
            //            if (datacnt > 10000)
            //                datacnt = 0;
            //            datacount.Content = datacnt.ToString();
            //            */

            //            IncDataRateCnt();
            //            AddToChart(val);
            //            addToHeartRateCalculation(val);
            //            BeginHRComputation();
            //            calcVpp(val);
            //        }
            //    ScrollCharts();
            //}
        }


        private void oneSecStep_Tick(object sender, EventArgs e)
        {
            //UpdateTemp();
            //UpdateHeartRate();
            //fallGridCheck();
            //UpdateDataRate();
            //if (devMode)
            //    UpdateVpp();
        }

        private void resetCounter()
        {
            counter = 0;
        }

        #endregion

        #region GUI Event Trigers

        private void cbPort_DropDownOpened(object sender, EventArgs e)
        {
            UpdateComPortList();
        }

        private void ConnectDisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (connection == connectionStatus.disconnected)
            {
                if (Connect())
                {
                    ConnectDisconnectButton.Content = "DISCONNECT";
                }
                else
                    MessageBox.Show("Connection failed. Could not open COM port");
            }
            else if (connection == connectionStatus.connected)
            {
                if (Disconnect())
                    ConnectDisconnectButton.Content = "CONNECT";
                else
                    MessageBox.Show("Disconnect failed.  Could not close COM port");
            }
            else
            {
                //undefined state
                connection = connectionStatus.disconnected;             //reset
            }

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

            //DataGrid.Children.Clear();

            //change the tab color
            //SetTabStyle(CurrentPage,true); //set to default
            //SetTabStyle(topage, false); //set to color

            switch (topage)
            {
                case Page.Summary:
                    //if (SummaryPage == null)
                    //    SummaryPage = new Summary_Control();
                    //DataGrid.Children.Add(SummaryPage);
                    if (HRPage == null)
                        HRPage = new HearRate_Control();
                    DataGrid.Children.Add(HRPage);

                    //delete in final version
                    SetTabStyle(CurrentPage,true); //set to default
                    SetTabStyle(topage, false); //set to color

                    HRPage.debugText.Text += "initialized Summary Page\n";
                    break;
                //case Page.HeartRate:
                //    if (HRPage == null)
                //        HRPage = new HearRate_Control();
                //    DataGrid.Children.Add(HRPage);
                //    break;
                //case Page.Activity:
                //    if (ActivityPage == null)
                //        ActivityPage = new Activity_Control();
                //    DataGrid.Children.Add(ActivityPage);
                //    break;
                //case Page.Temp:
                //    if (TempPage == null)
                //        TempPage = new Temperature_Control();
                //    DataGrid.Children.Add(TempPage);
                //    break;
                //case Page.Settings:
                //    if (SettingsPage == null)
                //        SettingsPage = new Settings_Control();
                //    DataGrid.Children.Add(SettingsPage);
                //    break;
                default:
                    //ChangePage(Page.Summary);
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

        #region Parsing Function

        private void ParseData()
        {
            int byteCount = serialPort.BytesToRead;
            uint val, payloadLength;
            Byte tempByte;

            //points.Clear();
            parseStep = ParseStatus.idle;       //reset
            while (byteCount > 0)
            {
                switch (parseStep)
                {
                    case ParseStatus.idle:
                        try
                        {
                            tempByte = (Byte)serialPort.ReadByte();
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }
                        byteCount--;
                        if (tempByte == 0xFF)
                        {
                            parseStep = ParseStatus.header2;
                            //debugText.Text += "Hi";
                        }
                        break;

                    case ParseStatus.header2:
                        try
                        {
                            tempByte = (Byte)serialPort.ReadByte();
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }
                        byteCount--;
                        if (tempByte == 0xFE)
                            parseStep = ParseStatus.length;
                        else
                            parseStep = ParseStatus.idle;   //reset
                        break;

                    case ParseStatus.length:
                        try
                        {
                            payloadLength = (Byte)serialPort.ReadByte();
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }
                        byteCount--;
                        parseStep = ParseStatus.type;
                        break;

                    case ParseStatus.type:
                        try
                        {
                            tempByte = (Byte)serialPort.ReadByte();
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }
                        byteCount--;
                        switch (tempByte)
                        {
                            case 0x00:
                                //Control
                                parseStep = ParseStatus.control;
                                break;

                            case 0x01:
                                //Read
                                parseStep = ParseStatus.read;
                                break;
                            
                            case 0x02:
                                //Alerts
                                parseStep = ParseStatus.alert;
                                break;
                            case 0x03:
                                //Continious Data
                                parseStep = ParseStatus.contDataPayload;
                                break;
                            default:
                                parseStep = ParseStatus.idle;   //reset
                                break;
                        }
                        break;

                    //case ParseStatus.alert:
                    //    //debugText.Text += "Alert!\n";
                    //    //showFall();
                    //    parseStep = ParseStatus.idle;       //reset
                    //    break;

                    //case ParseStatus.contData_subtype:
                    //    parseStep = ParseStatus.idle;       //reset
                    //    break;

                    //case ParseStatus.contDataPayload:
                    //    try
                    //    {
                    //        val = (uint)serialPort.ReadByte();
                    //        val = val * 256 + (uint)serialPort.ReadByte();
                    //    }
                    //    catch (Exception)
                    //    {
                    //        parseStep = ParseStatus.idle;
                    //        break;
                    //    }
                    //    points.Add(val);    //add to PPG value list
                    //    try
                    //    {
                    //        AccelerometerData((double)((int)serialPort.ReadByte() - 128) / 64.0, (double)((int)serialPort.ReadByte() - 128) / 64.0, (double)((int)serialPort.ReadByte() - 128) / 64.0);
                    //    }
                    //    catch (Exception)
                    //    {
                    //        parseStep = ParseStatus.idle;
                    //        break;
                    //    }
                    //    byteCount -= 5;
                    //    debugText.Text +=val +"\n";

                    //    parseStep = ParseStatus.idle;       //reset
                    //    break;

                    //case ParseStatus.singleData_subtype:
                    //    try
                    //    {
                    //        tempByte = (Byte)serialPort.ReadByte();
                    //    }
                    //    catch (Exception)
                    //    {
                    //        parseStep = ParseStatus.idle;
                    //        break;
                    //    }
                    //    byteCount--;
                    //    switch (tempByte)
                    //    {
                    //        case 0x00:
                    //            try
                    //            {
                    //                tempByte = (Byte)serialPort.ReadByte();
                    //            }
                    //            catch (Exception)
                    //            {
                    //                parseStep = ParseStatus.idle;
                    //                break;
                    //            }
                    //            byteCount--;
                    //            temperature = (int)tempByte;    //set the temperature
                    //            break;
                    //        default:
                    //            parseStep = ParseStatus.idle;
                    //            break;
                    //    }
                    //    parseStep = ParseStatus.idle;
                    //    break;

                    //default:
                    //    parseStep = ParseStatus.idle;
                    //    break;

                }
            }
        }

        #endregion


    }
}
