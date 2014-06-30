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
    /// 
    enum Page { Summary, HeartRate, Activity, Temp, Ppg, Imp, Settings, Log, Unknown, Debug };     //models the page tabs
        
    public partial class MainWindow : Window
    {
        // Variables

        #region Tab Control variables

        Summary_Control SummaryPage = new Summary_Control();
        HearRate_Control HRPage = new HearRate_Control();
        Temperature_Control TempPage = new Temperature_Control();
        Activity_Control ActivityPage = new Activity_Control();
        Settings_Control SettingsPage = new Settings_Control();
        Ppg_Control PpgPage = new Ppg_Control();
        Imp_Control ImpPage = new Imp_Control();
        Log_Control LogPage = new Log_Control();
        Debug_Control DebugPage = new Debug_Control();

        Page CurrentPage;
        #endregion

        #region timer variables
        private int counter = 0;
        //private int datacnt = 0;
        //private int fallCounter = 0;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Threading.DispatcherTimer oneSecStep = new System.Windows.Threading.DispatcherTimer();

        private int errorFlag_LeadOff, errorFlag_Fall, errorFlag_Batt;

        #endregion

        #region connection variables

        int connectFlag = 0;
        int disconnectFlag = 0;

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
            temp,
            contDataPayload_old,
            contDataPayload_EcgImpAcc,
            batt,
            spo2,
        }
        ParseStatus parseStep = ParseStatus.idle;

        enum battStatus            //following PacketformatV2
        {
            full,
            medium,
            critical,
            shutdown,
        }

        int[] ppgBuffer = new int[2];

        #endregion

        # region "heart Rate Variables"

        HeartRateHelper HRHelper = HeartRateHelper.Instance;

        #endregion

        #region "logging variables"
        LoggingHelper logger = new LoggingHelper();
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

            SettingsPage.updateMainWindowInstance(this);

            InitializeTimer();
            UpdateComPortList();
            UpdateChartWidth();
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

        private void RefreshApplication()
        {
            clearAllCharts();
            resetHR();
            resetTemp();
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

        private void Disconnect()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    sendDisconnectCommand();
                    disconnectFlag = 1;
                    ConnectDisconnectButton.IsEnabled = false;
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
        }

        private void Connect()
        {
            string portName = cbPort.Text;
            if (portName.CompareTo("None") == 0)
            {
                MessageBox.Show("Please select a valid COM port."); 
                return;
            }
                
            serialPort.PortName = portName;
            serialPort.BaudRate = BAUD_RATE;
            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection failed. Could not connect to selected COM port.");
                return;
            }

            if (serialPort.IsOpen)
            {
                sendConnectCommand();
                connectFlag = 1;
                ConnectDisconnectButton.IsEnabled = false;
            }

        }

        private void CancelConnect()
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("System Error.");
            }
            connectFlag = 0;
            ConnectDisconnectButton.IsEnabled = true;
            MessageBox.Show("Connection timeout.");
        }

        private void CancelDisconnect()
        {
            disconnectFlag = 0;
            ConnectDisconnectButton.IsEnabled = true;
        }

        private void disconnectedAckReceived()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("System Error. Could not disconnect COM port");
                }
                connection = connectionStatus.disconnected;
                ConnectDisconnectButton.Content = "CONNECT";
                disconnectFlag = 0;
                ConnectDisconnectButton.IsEnabled = true;
            }
        }

        private void connectedAckReceived()
        {
            connection = connectionStatus.connected;
            ConnectDisconnectButton.Content = "DISCONNECT";
            connectFlag = 0;
            ConnectDisconnectButton.IsEnabled = true;

            RefreshApplication();

            logger.StartLogging();
        }

        private void sendConnectCommand()
        {
            var connectCommand = new Byte[] { 0xFF, 0xFE, 0x01, 0x00, 0x00 };
               
            if(!sendDataOverSerial(connectCommand))
                CancelConnect();
        }

        private void sendDisconnectCommand()
        {
            var disconnectCommand = new Byte[] { 0xFF, 0xFE, 0x01, 0x00, 0x01 };
            if(!sendDataOverSerial(disconnectCommand))
                CancelDisconnect();
        }

        private Boolean sendDataOverSerial(Byte[] data)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Write(data, 0, data.Length);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
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
            if (serialPort.IsOpen)
            {
                ParseData();
            }
        }

        private void oneSecStep_Tick(object sender, EventArgs e)
        {
            if (connection == connectionStatus.connected) 
                UpdateGraphs();
            CheckConnectionTimer();
            CheckErrorFlag();
        }

        

        private void resetCounter()
        {
            counter = 0;
        }

        private void CheckConnectionTimer()
        {
            if (connectFlag >0 )
            {
                connectFlag++;
                if (connectFlag > 3)
                {
                    CancelConnect();
                }
            }
            else if (disconnectFlag > 0)
            {
                disconnectFlag++;
                if (disconnectFlag > 3)
                {
                    CancelDisconnect();
                }
            }
        }
        
        private void CheckErrorFlag()
        {
            if (errorFlag_LeadOff > 0)
            {
                errorFlag_LeadOff--;
                if (errorFlag_LeadOff == 0)
                    HideError(0);
            }
            if (errorFlag_Fall > 0)
            {
                errorFlag_Fall--;
                if (errorFlag_Fall == 0)
                    HideError(1);
            }
            if (errorFlag_Batt > 0)
            {
                errorFlag_Batt--;
                if (errorFlag_Batt == 0)
                    HideError(2);
            }
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
                Connect();
            }
            else if (connection == connectionStatus.connected)
            {
                Disconnect();
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

            DataGrid.Children.Clear();

            //change the tab color
            SetTabStyle(CurrentPage,true); //set to default
            SetTabStyle(topage, false); //set to color

            switch (topage)
            {
                case Page.Summary:
                    DataGrid.Children.Add(SummaryPage);
                    break;
                case Page.HeartRate:
                    DataGrid.Children.Add(HRPage);
                    break;
                case Page.Activity:
                    DataGrid.Children.Add(ActivityPage);
                    break;
                case Page.Temp:
                    DataGrid.Children.Add(TempPage);
                    break;
                case Page.Ppg:
                    DataGrid.Children.Add(PpgPage);
                    break;
                case Page.Imp:
                    DataGrid.Children.Add(ImpPage);
                    break;
                case Page.Settings:
                    DataGrid.Children.Add(SettingsPage);
                    break;

                case Page.Log:
                    DataGrid.Children.Add(LogPage);
                    break;
                case Page.Debug:
                     DataGrid.Children.Add(DebugPage);
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
                case Page.Imp:
                    SetTabStyle(ImpTab, toDefault);
                    break;
                case Page.Debug:
                    SetTabStyle(DebugTab, toDefault);
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

        private void DebugTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangePage(Page.Debug);
        }
		
        #endregion

        #region Parsing Function

        private void ParseData()
        {
            int byteCount = serialPort.BytesToRead;

            //int val1,val2,val3;

            List<ECGImpAccSpo2Data> DataList = new List<ECGImpAccSpo2Data>();
            ECGImpAccSpo2Data DataPoint;
            int[] ecgData,impData, accData;
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

                            case 0x06:
                                //PPG Data
                                parseStep = ParseStatus.spo2;
                                break;

                            default:
                                parseStep = ParseStatus.idle;   //reset
                                break;
                        }
                        break;

                    case ParseStatus.alert:

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
                                //Free fall
                                ShowError(1);
                                parseStep = ParseStatus.idle;
                                break;

                            case 0x01:
                                //battery overheat TODO
                                ShowError(2);
                                parseStep = ParseStatus.idle;
                                break;
                            
                            case 0x02:
                                //Marker TODO
                                SummaryPage.SetUserMarkerAlert();
                                parseStep = ParseStatus.idle;
                                break;
                            case 0x03:
                                //Lead Status TODO
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

                                if (tempByte != 0x00)
                                    ShowError(0);
                                parseStep = ParseStatus.idle;
                                break;

                            case 0x04:
                                //CL Connected
                                connectedAckReceived();
                                parseStep = ParseStatus.idle;
                                break;

                            case 0x05:
                                //CL Disconnected
                                disconnectedAckReceived();
                                return;             //discard all the remaining bytes

                            case 0x06:
                                //Batt. Strength
                                parseStep = ParseStatus.batt;
                                break;

                            case 0x07:
                                //ambient temp-not implemented
                                parseStep = ParseStatus.idle;
                                break;

                            case 0x08:
                                //body temperature 
                                parseStep = ParseStatus.temp;
                                break;

                            default:
                                parseStep = ParseStatus.idle;   //reset+-
                                break;
                        }
                        break;

                    case ParseStatus.batt:

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
                                //full
                                SetBatt(battStatus.full);
                                break;

                            case 0x01:
                                //medium
                                SetBatt(battStatus.medium);
                                break;
                            
                            case 0x02:
                                //critical
                                SetBatt(battStatus.critical);
                                break;

                            case 0x03:
                                //shutdown
                                SetBatt(battStatus.shutdown);
                                break;
                        }
                        parseStep = ParseStatus.idle;   //reset
                        break;

                    case ParseStatus.temp:
                        int bodyTemperature_rawValue;
                        double convertedTemp;
                        
                        try
                        {
                            bodyTemperature_rawValue = (int)serialPort.ReadByte();
                            bodyTemperature_rawValue = bodyTemperature_rawValue + (int)serialPort.ReadByte() * 256;
                            byteCount -= 2;


                            convertedTemp = TempPage.ConvertRawTemp(bodyTemperature_rawValue);
                            TempPage.AddToGraph(convertedTemp);
                            UpdateTempratureTabData(convertedTemp);

                            StaticVariables.Temperature = convertedTemp;

                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }

                        parseStep = ParseStatus.idle;
                        break;

                    case ParseStatus.contDataPayload_old:   //old format
                        parseStep = ParseStatus.idle;           //reset
                        break;

                    case ParseStatus.contDataPayload_EcgImpAcc:   //0X05 FORMAT

                        DataPoint = new ECGImpAccSpo2Data();
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
                                accData[i] = Convert.ToInt32((sbyte)serialPort.ReadByte());
                                byteCount -= 1;
                            }

                            DataPoint.updateData(ecgData, accData, impData, ppgBuffer);

                            DataList.Add(DataPoint);        //adding the data point to the data list collection
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }

                        parseStep = ParseStatus.idle;
                        break;

                    case ParseStatus.spo2:   //0X06 FORMAT

                        try
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                ppgBuffer[i] = (int)serialPort.ReadByte();
                                ppgBuffer[i] = ppgBuffer[i] + (int)serialPort.ReadByte() * 256;
                                byteCount -= 2;
                            }                           
                        }
                        catch (Exception)
                        {
                            parseStep = ParseStatus.idle;
                            break;
                        }

                        parseStep = ParseStatus.idle;
                        break;

                    default:
                        parseStep = ParseStatus.idle;
                        break;
                }
            }
            if (DataList.Count > 0)
                ProcessAndDisplayData(DataList);
        }


        private void SetBatt(battStatus p)
        {
            switch (p)
            {
                case battStatus.full:
                    imgBatt_full.Visibility = Visibility.Visible;
                    imgBatt_medium.Visibility = Visibility.Hidden;
                    imgBatt_critical.Visibility = Visibility.Hidden;
                    imgBatt_shutdown.Visibility = Visibility.Hidden;
                    break;
                case battStatus.medium:
                    imgBatt_full.Visibility = Visibility.Hidden;
                    imgBatt_medium.Visibility = Visibility.Visible;
                    imgBatt_critical.Visibility = Visibility.Hidden;
                    imgBatt_shutdown.Visibility = Visibility.Hidden;
                    break;
                case battStatus.critical:
                    imgBatt_full.Visibility = Visibility.Hidden;
                    imgBatt_medium.Visibility = Visibility.Hidden;
                    imgBatt_critical.Visibility = Visibility.Visible;
                    imgBatt_shutdown.Visibility = Visibility.Hidden;
                    break;
                case battStatus.shutdown:
                    imgBatt_full.Visibility = Visibility.Hidden;
                    imgBatt_medium.Visibility = Visibility.Hidden;
                    imgBatt_critical.Visibility = Visibility.Hidden;
                    imgBatt_shutdown.Visibility = Visibility.Visible;
                    break;
            }
        }

        
        #endregion

        #region display Data
        private void ProcessAndDisplayData(List<ECGImpAccSpo2Data> DataList)
        {

            List<int[]> ecgDataList = new List<int[]>();
            List<double[]> accDataList = new List<double[]>();
            List<int[]> impDataList = new List<int[]>();
            List<int[]> ppgDataList = new List<int[]>();

            List<int[]> summaryDataList = new List<int[]>();

            List<int> HRList = new List<int>();

            foreach(ECGImpAccSpo2Data dataPoint in DataList)
            {
                summaryDataList.Add(dataPoint.getSummaryData());

                ecgDataList.Add(dataPoint.getEcgData());
                accDataList.Add(dataPoint.getAccData());
                impDataList.Add(dataPoint.getImpData());
                ppgDataList.Add(dataPoint.getPpgData());
                HRList.Add(dataPoint.getHRMetadata());

                logger.addToFile(dataPoint);
            }

            SummaryPage.AddToChart(summaryDataList.ToArray());
            
            HRPage.AddToChart(ecgDataList.ToArray());            
            
            ActivityPage.AddToChart(accDataList.ToArray());
            UpdateActivityTabData(DataList.Last().getAccData(), DataList.Last().getSmoothenActivityVal());

            ImpPage.AddToChart(impDataList.ToArray());

            PpgPage.AddToChart(ppgDataList.ToArray());

            UpdateHeartRateTabData(HRHelper.getHeartRate());

            DebugPage.AddToChart(HRList.ToArray());

            //parse function writes to all the pagez simu;ltaneously:
            // 12 leads: HR,
            // 1 lead : summary
            // activity page, summary
            // temp page, summary
            // also save to log using the correct filename
        }

        private void UpdateGraphs()
        {
            UpdateHRGraph();
            UpdateSummaryGraph();
            UpdatePpgGraph();
            UpdateActGraph();
            UpdateImpGraph();
            //add update functipos for more graphs
        }

        private void UpdateHRGraph()
        {
            HRPage.AddToHRGraph(HRHelper.getHeartRate());
        }

        private void UpdatePpgGraph()
        {
            PpgPage.AddToPpgGraph(HRHelper.getHeartRate());
        }

        private void UpdateActGraph()
        {
            ActivityPage.AddToActGraph(HRHelper.getHeartRate());
        }

        private void UpdateImpGraph()
        {
            ImpPage.AddToImpGraph(HRHelper.getHeartRate());
        }

        Random random = new Random(); //temp variable - delete in final version
        private void UpdateSummaryGraph()
        {
            int[] temp = new int[3];
            temp[0] = HRHelper.getHeartRate();
            temp[1] = random.Next(10,60);    //replace with function
            temp[2] = random.Next(0, 100);    //replace with function

            SummaryPage.AddToGraph(temp);
        }


        private void UpdateTempratureTabData(double convertedTemp)
        {
            tbTemp.Text = String.Format("{0:0.0}", Math.Round(convertedTemp, 1));
        }

        private void UpdateHeartRateTabData(int HRVal)
        {
            tbHeartRate.Text = HRVal.ToString(); 
        }

        private void UpdateActivityTabData(double[] acc, double smoothenedMag)
        {
            tbxVal.Text = acc[0].ToString();
            tbyVal.Text = acc[1].ToString();
            tbzVal.Text = acc[2].ToString();


            smoothenedMag *= 2.5;    //multiplier


            if (smoothenedMag > 100)
                smoothenedMag = 100;
            else if (smoothenedMag < 0)
                smoothenedMag = 0;

            arcActivity.EndAngle = (smoothenedMag * 3.10) - 155;       //modify the activity arc

            tbActivityIndex.Text = ((int)smoothenedMag).ToString();
            if (smoothenedMag < 33)
                tbActivityStatus.Text = "RELAXED";
            else if (smoothenedMag < 66)
                tbActivityStatus.Text = "MODERATE";
            else
                tbActivityStatus.Text = "INTENSIVE";

        }


        private void ShowError(int err)
        {

            switch(err)
            {
                case 0:
                    //lead off
                    labelError_LeadOff.Visibility = Visibility.Visible;
                    errorFlag_LeadOff = 3;
                    break;
                case 1:
                    //fall
                    labelError_Fall.Visibility = Visibility.Visible;
                    errorFlag_Fall = 3;
                    break;
                case 2:
                    //batt
                    labelError_Batt.Visibility = Visibility.Visible;
                    errorFlag_Batt = 3;
                    break;
            }
            
            
        }

        private void HideAllErrorMsg()
        {
            labelError_LeadOff.Visibility = Visibility.Collapsed;
            labelError_Fall.Visibility = Visibility.Collapsed;
            labelError_Batt.Visibility = Visibility.Collapsed;
        }

        private void HideError(int err)
        {

            switch (err)
            {
                case 0:
                    //lead off
                    labelError_LeadOff.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    //fall
                    labelError_Fall.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    //batt
                    labelError_Batt.Visibility = Visibility.Collapsed;
                    break;
            }


        }

        private void resetHR()
        {
            HRHelper.ResetHeartRate();
        }


        private void resetTemp()
        {
            UpdateTempratureTabData(0);
        }

        #endregion

        #region "Chart Functions"

        internal void UpdateChartWidth()
        {
            SummaryPage.UpdateChartWidth();
            HRPage.UpdateChartWidth();
            //TempPage
            ActivityPage.UpdateChartWidth();
            //SettingsPage
            PpgPage.UpdateChartWidth();
            ImpPage.UpdateChartWidth();
            LogPage.UpdateChartWidth();
            DebugPage.UpdateChartWidth();
           
        }

        internal void clearAllCharts()
        {
            SummaryPage.Reset();
            HRPage.Reset();
            TempPage.Reset();
            ActivityPage.Reset();
            SettingsPage.Reset();
            PpgPage.Reset();
            ImpPage.Reset();
            LogPage.Reset();
            DebugPage.Reset();
        }
        #endregion
    }
}

/*
 * upon connect  
 * - clear error
 * - clear graphs
*/