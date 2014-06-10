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
        Ppg_Control PpgPage = null;
        Imp_Control ImpPage = null;
        Settings_Control SettingsPage = null;
        Log_Control LogPage = null;

        enum Page { Summary, HeartRate, Activity, Temp, Ppg, Imp, Settings, Log, Unknown };     //models the page tabs
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
        private const int BAUD_RATE = 9600;

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
            contDataPayload_old, 
            contDataPayload_EcgImpAcc,
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
                    sendDisconnectCommand();
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
                    sendConnectCommand();
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


        // TODO: Make sure that the connect and disconnect cannot change state without ack from cardioleaf
        private void sendConnectCommand()
        {
            var connectCommand = new Byte[] {0xFF, 0xFE, 0x01, 0x00, 0x00};
            sendDataOverSerial(connectCommand);
        }

        private void sendDisconnectCommand()
        {
            var connectCommand = new Byte[] { 0xFF, 0xFE, 0x01, 0x00, 0x01 };
            sendDataOverSerial(connectCommand);
        }
        
        private void sendDataOverSerial(Byte[] data)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(data, 0, data.Length);
                }
                catch (Exception)
                {
                    //exception caught
                    throw;
                }
            }
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
            if (connection == connectionStatus.connected)
            {
                ParseData();
            }
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
                    ResetAll();
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

        private void ResetAll()
        {
            resetCounter();
            ResetChild(CurrentPage);
        }

        #endregion

        #region onclose function

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
		
		#endregion

        #region TabControl Functions

        private void ResetChild(Page CurrentPage)
        {
            switch (CurrentPage)
            {
                case Page.Summary:
                    SummaryPage.Reset();
                    break;
                case Page.HeartRate:
                    HRPage.Reset();
                    break;
                case Page.Activity:
                    ActivityPage.Reset();
                    break;
                case Page.Temp:
                    TempPage.Reset();
                    break;
                case Page.Imp:
                    ImpPage.Reset();
                    break;
                case Page.Settings:
                    SettingsPage.Reset();
                    break;
                case Page.Log:
                    LogPage.Reset();
                    break;
            }
        }

        private bool ChangePage(Page topage)
        {
            if (CurrentPage != Page.Unknown && CurrentPage == topage) return false;

            DataGrid.Children.Clear();

            //change the tab color
            SetTabStyle(CurrentPage,true); //set to default
            SetTabStyle(topage, false); //set to color

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
                case Page.Ppg:
                    if (PpgPage == null)
                        PpgPage = new Ppg_Control();
                    DataGrid.Children.Add(PpgPage);
                    break;
                case Page.Imp:
                    if (ImpPage == null)
                        ImpPage = new Imp_Control();
                    DataGrid.Children.Add(ImpPage);
                    break;
                case Page.Settings:
                    if (SettingsPage == null)
                        SettingsPage = new Settings_Control();
                    DataGrid.Children.Add(SettingsPage);
                    break;

                case Page.Log:
                    if (LogPage == null)
                        LogPage = new Log_Control();
                    DataGrid.Children.Add(LogPage);
                    break;
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
                case Page.Ppg:
                    SetTabStyle(PpgTab, toDefault);
                    break;
                case Page.Settings:
                    SetTabStyle(SettingsTab, toDefault);
                    break;
                case Page.Log:
                    SetTabStyle(LogTab, toDefault);
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

        private void PpgTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Ppg);
        }

        private void ImpTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangePage(Page.Imp);
        }

        private void SettingsTab_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChangePage(Page.Settings);
        }

        private void LogTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangePage(Page.Log);
        }
		
		
        #endregion

        #region Parsing Function

        private void ParseData()
        {
            int byteCount = serialPort.BytesToRead;

            //int val1,val2,val3;

            List<ECGImpAccData> DataList = new List<ECGImpAccData>();
            ECGImpAccData DataPoint;
            int[] ecgData,accData,impData ;
            int payloadLength;
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
                            parseStep = ParseStatus.header2;
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
                                parseStep = ParseStatus.idle;
                                break;

                            case 0x01:
                                //Read
                                parseStep = ParseStatus.read;
                                parseStep = ParseStatus.idle;
                                break;
                            
                            case 0x02:
                                //Alerts
                                parseStep = ParseStatus.alert;
                                parseStep = ParseStatus.idle;
                                break;
                            case 0x03:
                                //Continious Data
                                parseStep = ParseStatus.contDataPayload_old;
                                //parseStep = ParseStatus.idle;
                                break;

                            case 0x05:
                                //Continious Data
                                parseStep = ParseStatus.contDataPayload_EcgImpAcc;
                                //parseStep = ParseStatus.idle;
                                break;

                            default:
                                parseStep = ParseStatus.idle;   //reset
                                break;
                        }
                        break;


                    case ParseStatus.contDataPayload_old:   //old format
                        //try
                        //{
                        //    val1 = (int)serialPort.ReadByte();
                        //    val1 = val1  + (int)serialPort.ReadByte() * 256;
                        //    val3 = (int)serialPort.ReadByte();
                        //    val3 = val3 + (int)serialPort.ReadByte() * 256;
                        //    val2 = val1 + val3;
                        //}
                        //catch (Exception)
                        //{
                        //    parseStep = ParseStatus.idle;
                        //    break;
                        //}  
                        ////HRPage.AddToChart(val1,val2,val3);      //add to chart
                        //byteCount -= 2;

                        parseStep = ParseStatus.idle;           //reset
                        break;

                    case ParseStatus.contDataPayload_EcgImpAcc:   //0X05 FORMAT

                        DataPoint = new ECGImpAccData();
                        ecgData = new int[8];
                        accData = new int[3];
                        impData = new int[2];
                        try
                        {
                            for(int i = 0 ; i < 8 ; i++)
                            {
                                ecgData[i] = (int)serialPort.ReadByte();
                                ecgData[i] = ecgData[i] + (int)serialPort.ReadByte() * 256;
                                byteCount -= 2;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                impData[i] = (int)serialPort.ReadByte();
                                impData[i] = impData[i] + (int)serialPort.ReadByte() * 256;
                                byteCount -= 2;
                            }

                            for (int i = 0; i < 3; i++)
                            {
                                accData[i] = (int)serialPort.ReadByte();
                                byteCount -= 1;
                            }

                            DataPoint.updateData(ecgData, accData, impData);

                            DataList.Add(DataPoint);        //adding the data point to the data list collection
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }

                        parseStep = ParseStatus.idle;
                        break;
                }
            }
            if (DataList.Count > 0)
                DisplayData(DataList);
        }
        #endregion
        
        
        private void DisplayData(List<ECGImpAccData> DataList)
        {

            List<int[]> ecgDataList = new List<int[]>();
            foreach(ECGImpAccData dataPoint in DataList)
            {
                ecgDataList.Add(dataPoint.getEcgData());
            }

            HRPage.AddToChart(ecgDataList.ToArray());
            SummaryPage.AddToChart(ecgDataList.ToArray());
            //parse function writes to all the pagez simu;ltaneously:
            // 12 leads: HR,
            // 1 lead : summary
            // activity page, summary
            // temp page, summary
            // also save to log using the correct filename
        }



    }
}


// TODO and task organization

// update the connection and disconnect procedures to acount for errors
// steps : make HR page work... then worry qabt the others. need to have a separate thread for the incoming data and the data processing



