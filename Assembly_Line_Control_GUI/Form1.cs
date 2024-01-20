using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using Opc.UaFx;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FireSharp.Response;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;

namespace AssemblyLineControl
{
    public partial class Form1 : Form
    {
        EtherCat et;
        OPCServer opcClient;
        MySqlConnection conn;
        FireBaseDatabase fdb;
        string opcServerStatus = "Disconnected";
        string etherCatStatus = "Disconnected";
        string firebaseStatus = "Disconnected";
        string DigitalTwin = "Off";
        string AMSNETIDvar = "10.1.2.54.1.1";
        int portvar = 851;
        string IPAddress = "169.254.102.127";
        int IPport = 4841;
        string serverName = "localhost";
        string uid = "root";
        string pwd = "AR@2023";
        string database = "assemblyline";
        string AuthSecret = "NCsBz5YP6bwh5EyFlUVnWPywif23cj6dcZrzSAPM";
        string BasePath = "https://assemblyline-50210-default-rtdb.asia-southeast1.firebasedatabase.app/";

        public Form1() //Initaliser (Constructor)
        {
            InitializeComponent();
            AmsnetidBox.Text = AMSNETIDvar;
            AmsnetidportBox.Text = portvar.ToString();
            ipAddressBox.Text = IPAddress;
            ipPortBox.Text = IPport.ToString();
            mysqlServerBox.Text = serverName;
            mysqlUIDBox.Text = uid;
            mysqlPWBox.Text = pwd;
            mysqlDatabaseBox.Text = database;
            AuthSecretTxtBox.Text = AuthSecret;
            BasepathTxtBox.Text = BasePath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Configure system tab
        private void establishEtherCatConnection() //Establish connection with TwinCAT
        {
            if (AmsnetidBox.Text.ToString() != "" && AmsnetidportBox.Text.ToString() != "")
            {
                AMSNETIDvar = AmsnetidBox.Text;
                portvar = Convert.ToInt32(AmsnetidportBox.Text);
                try
                {
                    messageBox.Items.Add("Connecting to EtherCat");
                    et = new EtherCat(AMSNETIDvar, portvar);
                    
                }
                catch (Exception ex)
                {
                    messageBox.Items.Add(ex.Message);
                }
                var connection_status = et.getConnectionStatus();
                if (connection_status == 1)
                {
                    messageBox.Items.Add("Successfully connected to EtherCat");
                    etherCatStatus = "Connected";
                    if(etherCatStatus == "Connected")
                    {
                        try
                        {
                            Thread thread = new Thread(getSensorStatus);
                            thread.Start();
                        }
                        catch (Exception ex)
                        {
                            Invoke(new Action(() => updateMessageBox(ex.Message)));
                        }
                    }
                    Statustxt.Text = "Connected";
                    Statustxt.ForeColor = Color.Green;
                }
                else
                {
                    Statustxt.Text = "Disconnected";
                    etherCatStatus = "Disconnected";
                }
            }
            else
            {
                messageBox.Items.Add("Please enter valid AMSNETID and PORT");
            }
        }
        
        private void establishServerConnection()//Establish connection with OPC UA Server
        {
            if (ipAddressBox.Text.ToString() != "" && ipPortBox.Text.ToString() != "")
            {
                try
                {
                    messageBox.Items.Add("Connecting to OPC Server");
                    opcClient = new OPCServer(ipAddressBox.Text.ToString(), ipPortBox.Text.ToString());
                    opcStatusTxt.Text = "Connected";
                    opcStatusTxt.ForeColor = Color.Green;
                    messageBox.Items.Add("Successfully connected to OPC Server");
                    opcServerStatus = "Connected";
                }
                catch (Exception ex)
                {
                    opcStatusTxt.Text = "Disconnected";
                    opcStatusTxt.ForeColor = Color.Red;
                    messageBox.Items.Add(ex.Message);
                }
            }
            else
            {
                messageBox.Items.Add("Please enter valid IPAddress and Port");
            }
        }

        private void establishMYSQLConnection()//Establish connection with MYSQL
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                string myConnectionString = String.Format("server={0};uid={1};pwd={2};database={3}"
                    , mysqlServerBox.Text.ToString(),
                    mysqlUIDBox.Text.ToString(),
                    mysqlPWBox.Text.ToString(),
                    mysqlDatabaseBox.Text.ToString());


                conn.ConnectionString = myConnectionString;
                conn.Open();

                if (conn.State.ToString() == "Open")
                {
                    mysqlStatusTxt.Text = "Connected";
                    mysqlStatusTxt.ForeColor = Color.Green;
                }
                else
                {
                    mysqlStatusTxt.Text = "Disconnected";
                    mysqlStatusTxt.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => updateMessageBox(ex.Message)));
                mysqlStatusTxt.Text = "Disconnected";
                mysqlStatusTxt.ForeColor = Color.Red;
            }
        }

        private void establishFirebaseConnection()//Establish connection with OPC UA Server
        {
            if (AuthSecretTxtBox.Text.ToString() != "" && BasepathTxtBox.Text.ToString() != "")
            {
                try
                {
                    messageBox.Items.Add("Connecting to Firebase");
                    fdb = new FireBaseDatabase(AuthSecretTxtBox.Text.ToString(), BasepathTxtBox.Text.ToString());
                    bool fdbStatus = fdb.getConnectionStatus();
                    if (fdbStatus)
                    {
                        firebaseStatusTxt.Text = "Connected";
                        firebaseStatusTxt.ForeColor = Color.Green;
                        messageBox.Items.Add("Successfully connected to Firebase");
                        firebaseStatus = "Connected";
                    }
                    else
                    {
                        firebaseStatusTxt.Text = "Disconnected";
                        firebaseStatus = "Disconnected";
                        firebaseStatusTxt.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    firebaseStatusTxt.Text = "Disconnected";
                    firebaseStatus = "Disconnected";
                    firebaseStatusTxt.ForeColor = Color.Red;
                    messageBox.Items.Add(ex.Message);
                }
            }
            else
            {
                messageBox.Items.Add("Please enter valid AuthSecret ID and BasePath");
            }
        }

        private void btnDigitalTwinStart_Click(object sender, EventArgs e)//Turns on the Digital Twin(Condition to send the values to MATLAB)
        {
            if (etherCatStatus == "Disconnected")
            {
                Invoke(new Action(() => updateMessageBox("Establish connection with EtherCAT before starting Digital Twin")));
            }
            else
            {
                if (opcServerStatus == "Disconnected")
                {
                    Invoke(new Action(() => updateMessageBox("Establish connection with OPC UA Server before starting Digital Twin")));
                }
                else
                {
                    DigitalTwin = "On";
                    digitalTwinStatusTxt.Text = DigitalTwin;
                    digitalTwinStatusTxt.ForeColor = Color.Green;
                }
            }
        }

        private void btnDigitalTwinStop_Click(object sender, EventArgs e)//Turns off the Digital Twin(Condition to stop sending the values to MATLAB)
        {
            DigitalTwin = "Off";
            digitalTwinStatusTxt.Text = DigitalTwin;
            digitalTwinStatusTxt.ForeColor = Color.Red;
        }

        private void btnMySqlConnect_Click(object sender, EventArgs e)
        {
            establishMYSQLConnection();
        }

        private void btnEstablish_Connection_Click(object sender, EventArgs e)
        {
            establishEtherCatConnection();
        }

        private void btnEstablish_OPC_Server_Connection_Click(object sender, EventArgs e)
        {
            establishServerConnection();
        }

        private void btnFirebaseConnect_Click(object sender, EventArgs e)
        {
            establishFirebaseConnection();
        }

        //Manual Control Tab
        private void btnAlExtend_Click(object sender, EventArgs e)
        {
            et.setValue("AlExtend", true);
        }

        private void btnAlRetract_Click(object sender, EventArgs e)
        {
            et.setValue("AlRetract", true);
        }

        private void btnPlExtend_Click(object sender, EventArgs e)
        {
            et.setValue("PlExtend", true);
        }

        private void btnPlRetract_Click(object sender, EventArgs e)
        {
            et.setValue("PlRetract", true);
        }

        private void btnC1Fwd_Click(object sender, EventArgs e)
        {
            et.setValue("C1Fwd", true);
        }

        private void btnC1Rev_Click(object sender, EventArgs e)
        {
            et.setValue("C1Rev", true);
        }

        private void btnAlExtendStop_Click(object sender, EventArgs e)
        {
            et.setValue("AlExtend", false);
        }

        private void btnAlRetractStop_Click(object sender, EventArgs e)
        {
            et.setValue("AlRetract", false);
        }

        private void btnPlExtendStop_Click(object sender, EventArgs e)
        {
            et.setValue("PlExtend", false);
        }

        private void btnPlRetractStop_Click(object sender, EventArgs e)
        {
            et.setValue("PlRetract", false);
        }

        private void btnC1FwdStop_Click(object sender, EventArgs e)
        {
            et.setValue("C1Fwd", false);
        }

        private void btnC1RevStop_Click(object sender, EventArgs e)
        {
            et.setValue("C1Rev", false);
        }

        private void btnPinExtend_Click(object sender, EventArgs e)
        {
            et.setValue("PinExtend", true);
        }

        private void btnPinRetract_Click(object sender, EventArgs e)
        {
            et.setValue("PinExtend", false);
        }

        private void btnPushExtend_Click(object sender, EventArgs e)
        {
            et.setValue("PushExtend", true);
        }

        private void btnPushRetract_Click(object sender, EventArgs e)
        {
            et.setValue("PushExtend", false);
        }

        private void btnTrayExtend_Click(object sender, EventArgs e)
        {
            et.setValue("TrayExtend", true);
        }

        private void btnTrayExtendStop_Click(object sender, EventArgs e)
        {
            et.setValue("TrayExtend", false);
        }

        private void btnTrayRetract_Click(object sender, EventArgs e)
        {
            et.setValue("TrayRetract", true);
        }

        private void btnTrayRetractStop_Click(object sender, EventArgs e)
        {
            et.setValue("TrayRetract", false);
        }

        private void btnComExtend_Click(object sender, EventArgs e)
        {
            et.setValue("ComExtend", true);
        }

        private void btnComExtendStop_Click(object sender, EventArgs e)
        {
            et.setValue("ComExtend", false);
        }

        private void btnComRetract_Click(object sender, EventArgs e)
        {
            et.setValue("ComRetract", true);
        }

        private void btnComRetractStop_Click(object sender, EventArgs e)
        {
            et.setValue("ComRetract", false);
        }

        private void btnC2Fwd_Click(object sender, EventArgs e)
        {
            et.setValue("C2Fwd", true);
        }

        private void btnC2FwdStop_Click(object sender, EventArgs e)
        {
            et.setValue("C2Fwd", false);
        }

        private void btnC2Rev_Click(object sender, EventArgs e)
        {
            et.setValue("C2Rev", true);
        }

        private void btnC2RevStop_Click(object sender, EventArgs e)
        {
            et.setValue("C2Rev", false);
        }

        private void btnGrp1On_Click(object sender, EventArgs e)
        {
            et.setValue("Gripper1", true);
        }

        private void btnGrp1Off_Click(object sender, EventArgs e)
        {
            et.setValue("Gripper1", false);
        }

        private void btnGrp2On_Click(object sender, EventArgs e)
        {
            et.setValue("Gripper2", true);
        }

        private void btnGrp2Off_Click(object sender, EventArgs e)
        {
            et.setValue("Gripper2", false);
        }

        private void viewWarehouseRefreshBtn_Click(object sender, EventArgs e)
        {
            getWarehouseInfo();
        }

        private void productDetailsRefreshBtn_Click(object sender, EventArgs e)
        {
            getProductDetails();
        }

        private void OrdersRefreshBtn_Click(object sender, EventArgs e)
        {
            if(orderStatusComboBox.Text.ToString() != "")
            {
                errorProvider.Clear();
                getOrderDetails();
               
            }
            else
            {
                errorProvider.SetError(orderStatusComboBox, "Please select the status of orders to displayed");
            }
        }

        private void btnStartProduction_Click(object sender, EventArgs e)
        {
            WarehouseLocation warehouselocation = get_empty_warehouse_location();
            if (warehouselocation.path != null)
            {
                get_next_Order();
            }
            else updateMessageBox("Warehouse does not have an empty slot");
        }

        private async void getOrderDetails()
        {
            Task<JObject> responseTask = fdb.retreiveData("Orders");
            if(responseTask != null)
            {
                JObject response = await responseTask; // Await the task to get the JObject result
                //Console.WriteLine(response.ToString());
                List<OrderDetails> orderDetails = new List<OrderDetails>();
                if (response != null)
                {
                    foreach (var property in response.Properties())
                    {
                        // Access the property name (key)
                        string propertyName = property.Name;
                        // Access the property value as a JObject
                        JObject orderData = property.Value as JObject;
                        //Console.WriteLine(orderData.ToString());
                        // Deserialize the orderData into an OrderDetails object
                        if (orderData != null)
                        {
                            OrderDetails order = orderData.ToObject<OrderDetails>();
                            JObject orderListObject = orderData["orderList"].Value<JObject>();
                            Dictionary<string, int> orderList = orderListObject.ToObject<Dictionary<string, int>>();
                            order.orderList = orderList;
                            if (order != null)
                            {
                                orderDetails.Add(order);
                            }
                        }
                    }

                    OrderDetailsDatagridView.Rows.Clear();
                    foreach (OrderDetails order in orderDetails)
                    {if(orderStatusComboBox.Text.ToString() != "All")
                        {
                            if (order.orderStatus == orderStatusComboBox.Text.ToString())
                            {
                                string orderListString = JsonConvert.SerializeObject(order.orderList);
                                int RowIndex = OrderDetailsDatagridView.Rows.Add();
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderID"].Value = order.orderID;
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["CustomerID"].Value = order.customerID;
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderList"].Value = orderListString;
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["Date"].Value = order.date;
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["DueDate"].Value = order.dueDate;
                                OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderStatus"].Value = order.orderStatus;
                            }
                        }
                        else
                        {
                            string orderListString = JsonConvert.SerializeObject(order.orderList);
                            int RowIndex = OrderDetailsDatagridView.Rows.Add();
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderID"].Value = order.orderID;
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["CustomerID"].Value = order.customerID;
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderList"].Value = orderListString;
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["Date"].Value = order.date;
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["DueDate"].Value = order.dueDate;
                            OrderDetailsDatagridView.Rows[RowIndex].Cells["OrderStatus"].Value = order.orderStatus;
                        }
                    }
                }
                else
                {
                    updateMessageBox("No Orders available");
                }
            }
            else
            {
                updateMessageBox("No Orders available");
            }
        }

        private async void get_next_Order()
        {
            Task<JObject> responseTask = fdb.retreiveData("Orders");
            if (responseTask != null)
            {
                JObject response = await responseTask; // Await the task to get the JObject result
                List<OrderDetails> orderDetails = new List<OrderDetails>();
                OrderDetails selectedOrder = null;
                if (response != null)
                {
                    foreach (var property in response.Properties())
                    {
                        // Access the property name (key)
                        string propertyName = property.Name;
                        // Access the property value as a JObject
                        JObject orderData = property.Value as JObject;
                        // Deserialize the orderData into an OrderDetails object
                        if (orderData != null)
                        {
                            OrderDetails order = orderData.ToObject<OrderDetails>();
                            if (order != null)
                            {
                                orderDetails.Add(order);
                            }
                        }
                    }
                    DateTime currentTimestamp = DateTime.Now;
                    DateTime nearestDate = DateTime.MinValue;
                    TimeSpan nearestTimeSpan = TimeSpan.MaxValue;
                    foreach (OrderDetails order in orderDetails)
                    {
                        
                        DateTime timestamp = DateTime.ParseExact(order.dueDate, "yyyy-MM-dd HH:mm:ss", null);
                        TimeSpan timeSpan = currentTimestamp - timestamp;
                        timeSpan = (timeSpan < TimeSpan.Zero) ? -timeSpan : timeSpan; // Calculate the absolute value

                        if (timeSpan < nearestTimeSpan)
                        {
                            nearestDate = timestamp;
                            nearestTimeSpan = timeSpan;
                            selectedOrder = order;
                        }
                    }

                    int quantity = selectedOrder.orderList["ALPL"];
                    start_Production_for_Order(quantity);
                }
                else
                {
                    updateMessageBox("No Orders available");
                }
            }
            else
            {
                updateMessageBox("No Orders available");
            }
        }

        private void start_Production_for_Order(int quantity)
        {
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.QuantityR1, quantity * 2);
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.QuantityR3, quantity);
            start_complete_process();
        }

        //Process Control Tab
        private void btnCompIntakeStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(CompIntakeProcess);
            thread.Start();
        }

        private void btnPinningStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(startPinningProcess);
            thread.Start();
        }

        private void btnAssemblyStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(startAssembly);
            thread.Start();
        }

        private void btnMoveToASRSStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(MoveProductToASRS);
            thread.Start();
        }

        private void btnStartFullProcess_Click(object sender, EventArgs e)
        {
            start_complete_process();

        }

        private void start_complete_process()
        {
            Thread thread = new Thread(listenDispenseValues);
            thread.Start();

            Thread pinningThread = new Thread(listenPinningValues);
            pinningThread.Start();

            Thread assemblyThread = new Thread(listenAssemblyValues);
            assemblyThread.Start();

            Thread conveyorThread = new Thread(listenConveyorValues);
            conveyorThread.Start();

            Thread gripper1Thread = new Thread(listenGripper1);
            gripper1Thread.Start();

            Thread gripper2Thread = new Thread(listenGripper2);
            gripper2Thread.Start();
        }

        private string getProductStatus() //Gets Defective/Non-Defective status from the camera
        {
            Invoke(new Action(() => updateMessageBox("Waiting for camera input")));
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Capture, 1);
            String capturedValue;
            while (true) {
                capturedValue = opcClient.GetServerStringValue(OPCNodes.NameSpaceValue, OPCNodes.CapturedValue).ToString(); //Parsing error
                if (capturedValue != "none")
                {
                    break;
                }
            }
            Invoke(new Action(() => updateMessageBox("Camera input received")));
            return capturedValue;
        }

        private void placeProduct(string type) //Dispenses Aluminium/Plastic product based on 'type' parameter
        {
            Invoke(new Action(() => updateMessageBox(String.Format("Placing {0} product", type))));
            var ExtendStr = String.Format("{0}Extend",type);
            var RetractStr = String.Format("{0}Retract",type);
            et.setValue(ExtendStr, true);
            Thread.Sleep(500);
            et.setValue(ExtendStr, false);
            et.setValue(RetractStr, true);
            Thread.Sleep(500);
            et.setValue(RetractStr, false);
            Invoke(new Action(() => updateMessageBox(String.Format("Placed {0} product", type))));
        }

        //Processes
        private void CompIntakeProcess() //Places the Aluminium and Plastic components on the conveyor and turns on the conveyor
        {
            Invoke(new Action(() => updateProcessGui(ComponentsIntaketxt, "On going", Color.Green)));
            placeProduct("Al");
            Thread.Sleep(500);
            et.setValue("AlRetract", false);
            et.setValue("C1Fwd", true);
            Thread.Sleep(4000);
            et.setValue("C1Fwd", false);
            Thread.Sleep(500);
            placeProduct("Pl");
            Thread.Sleep(500);
            et.setValue("PlRetract", false);
            while (C1IRStatustxt.Text.ToString() == "False")
            {
                et.setValue("C1Fwd", true);
            }
            Thread.Sleep(250);
            et.setValue("C1Fwd", false);
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.C1Sen, 1);
            Invoke(new Action(() => updateProcessGui(ComponentsIntaketxt, "Completed", Color.Red)));
        }

        private void startPinningProcess() //Pins the component
        {
            Invoke(new Action(() => updateMessageBox("Pinning in progress")));
            Invoke(new Action(() => updateProcessGui(PinningStatustxt, "On going", Color.Green)));
            setUpPinningStation();
            Thread.Sleep(500);
            et.setValue("PushExtend", true);
            Thread.Sleep(1000);
            et.setValue("PinExtend", true);
            Thread.Sleep(3000);
            et.setValue("PushExtend", false);
            Thread.Sleep(1000);
            et.setValue("PinExtend", false);
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Pinning, 1);
            Invoke(new Action(() => updateProcessGui(PinningStatustxt, "Completed", Color.Red)));
            Invoke(new Action(() => updateMessageBox("Pinning completed")));
        }

        private void setUpPinningStation() //Retracts the pinner and pusher
        {
            et.setValue("PushExtend", false);
            et.setValue("PinExtend", false);
        }

        private void startAssembly() //Assembles the product by compressing two components
        {
            Invoke(new Action(() => updateMessageBox("Assembly in progress")));
            Invoke(new Action(() => updateProcessGui(AssemblyStatustxt, "On going", Color.Green)));
            setUpAssemblyStation();
            Thread.Sleep(1000);
            et.setValue("TrayExtend", false);
            et.setValue("TrayRetract", true);
            Thread.Sleep(1000);
            et.setValue("TrayRetract", false);
            et.setValue("ComRetract", false);
            et.setValue("ComExtend", true);
            Thread.Sleep(2000);
            et.setValue("ComExtend", false);
            et.setValue("ComRetract", true);
            Thread.Sleep(1000);
            et.setValue("TrayExtend", true);
            Invoke(new Action(() => updateProcessGui(AssemblyStatustxt, "Completed", Color.Red)));
            Invoke(new Action(() => updateMessageBox("Assembly Completed")));
        }

        private void setUpAssemblyStation() //Extends the tray and retracts the compressor
        {
            et.setValue("TrayRetract", false);
            et.setValue("TrayExtend", true);
            et.setValue("ComExtend", false);
            et.setValue("ComRetract", true);
        }

        private void MoveProductToASRS() //Start the conveyor2 till it reached the ASRS
        {
            Invoke(new Action(() => updateMessageBox("Conveyor 2 Running")));
            Invoke(new Action(() => updateProcessGui(MoveToASRStxt, "On going", Color.Green)));
            while (C2Sentxt.Text.ToString() == "False")
            {
                et.setValue("C2Fwd", true);
            }
            Thread.Sleep(400);
            et.setValue("C2Fwd", false);
            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Conveyor, 0);
            Invoke(new Action(() => updateProcessGui(MoveToASRStxt, "Completed", Color.Red)));
            Invoke(new Action(() => updateMessageBox("Conveyor 2 Stopped")));
        }

        //Threads
        private void listenDispenseValues()
        {
            while (true){
                int dispense = opcClient.GetServerValue(OPCNodes.NameSpaceValue,OPCNodes.Dispense);
                if (dispense == 1)
                {
                    List<String> productQueue = opcClient.GetProductQueue(OPCNodes.NameSpaceValue,OPCNodes.ProductQueue);
                    Console.WriteLine(productQueue.ToString());
                    try
                    {
                        String pp = null;
                        if(productQueue.Count <= 1) {
                            placeProduct("Pl");
                            pp = "Pl";
                        }
                        else
                        {
                            String lastProduct = productQueue.Last();
                            Invoke(new Action(() => updateMessageBox(lastProduct)));
                            if (lastProduct == "Plastic")
                            {
                                placeProduct("Al");
                                pp = "Al";
                            }
                            else if (lastProduct == "Aluminium")
                            {
                                placeProduct("Pl");
                                pp = "Pl";
                            }
                        }
                        et.setValue("C1Fwd", true);
                        if(pp == "Pl")
                        {
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Thread.Sleep(4000);
                        }
                        et.setValue("C1Fwd", false);
                        Thread.Sleep(500);
                        String capturedValue = getProductStatus();
                        Console.WriteLine(capturedValue);
                        if (capturedValue == "Defective")
                        {
                            et.setValue("C1Rev", true);
                            Thread.Sleep(6000);
                            et.setValue("C1Rev", false);
                            opcClient.SetServerStringValue(OPCNodes.NameSpaceValue, OPCNodes.CapturedValue, "none");
                        }
                        else if (capturedValue == "Non-Defective")
                        {
                            while (C1IRStatustxt.Text == "False")
                            {
                                et.setValue("C1Fwd", true);
                            }
                            Thread.Sleep(200);
                            et.setValue("C1Fwd", false);
                            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.CompIntake, 1); //Sets compIntake 1
                            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Dispense, 0); //Sets Dispense 0
                            opcClient.SetServerStringValue(OPCNodes.NameSpaceValue, OPCNodes.CapturedValue, "none"); //Sets capturedValue none
                        }
                        else
                        {
                            et.setValue("C1Fwd", false);
                            et.setValue("C1Rev", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => updateMessageBox(ex.ToString())));
                    }
                }
            }
        }

        private void getSensorStatus() //Gets all the sensor values from the TwinCAT Software and sends the list to Update GUI func
        {
            List<string> sensorStatusList = new List<string>();
            while (true)
            {
                try
                {
                    sensorStatusList = et.getStatus();
                    Invoke(new Action<List<string>>(updateGui), sensorStatusList);
                    if(DigitalTwin == "On")
                    {
                        if (opcServerStatus == "Connected")
                        {
                            try
                            {
                                Thread thread = new Thread(() => sendSensorStatusToOPC(sensorStatusList));
                                thread.Start();
                            }
                            catch (Exception ex)
                            {
                                Invoke(new Action(() => updateMessageBox(ex.Message)));
                            }
                        }
                        else
                        {
                            Invoke(new Action(() => updateMessageBox("OPC UA Server not found")));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.ToString())));
                    break;
                }
            }
        }

        private void sendSensorStatusToOPC(List<string> sensorStatusList)//Gets the sensorStatusList from getsensorStatus and sends it to OPC Server(for Digital Twin)
        {
            try
            {
                List<int> intList = sensorStatusList.Select(str => str.Equals("True", StringComparison.OrdinalIgnoreCase) ? 1 : 0).ToList();
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.AlExtendSense, intList[0]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.AlRetractSense, intList[1]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PlExtendSense, intList[2]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PlRetractSense, intList[3]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PinExtSen, intList[5]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PinRetSen, intList[6]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PushExtSen, intList[7]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.PushRetSen, intList[8]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.TrayExtSen, intList[9]);
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.TrayRetSen, intList[10]);
            }
            catch(Exception ex) {
                Invoke(new Action(() => updateMessageBox(ex.Message.ToString())));
            }
        }

        private void listenGripper1()//Listens the Gripper value in the server to turn on the gripper
        {
            while (true)
            {
                try
                {
                    int gripper2 = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Gripper1);
                    switch (gripper2)
                    {
                        case 0:
                            try
                            {
                                et.setValue("Gripper1", false);
                                break;
                            }
                            catch
                            {
                                Invoke(new Action(() => updateMessageBox("Error in Gripper1-Twincat")));
                                break;
                            }
                        case 1:
                            try
                            {
                                et.setValue("Gripper1", true);
                                break;
                            }
                            catch
                            {
                                Invoke(new Action(() => updateMessageBox("Error in Gripper1-Twincat")));
                                break;
                            }
                        case 2:
                            et.setValue("Gripper1", false);
                            Invoke(new Action(() => updateMessageBox("Error in Gripper1-OPC Server")));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.Message)));
                    break;
                }
            }
        }

        private void listenGripper2()//Listens the Gripper value in the server to turn on the gripper
        {
            while (true)
            {
                try
                {
                    int gripper2 = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Gripper2);
                    switch (gripper2)
                    {
                        case 0:
                            try
                            {
                                et.setValue("Gripper2", false);
                                break;
                            }
                            catch
                            {
                                Invoke(new Action(() => updateMessageBox("Error in Gripper2-Twincat")));
                                break;
                            }
                        case 1:
                            try
                            {
                                et.setValue("Gripper2", true);
                                break;
                            }
                            catch
                            {
                                Invoke(new Action(() => updateMessageBox("Error in Gripper2-Twincat")));
                                break;
                            }
                        case 2:
                            et.setValue("Gripper2", false);
                            Invoke(new Action(() => updateMessageBox("Error in Gripper2-OPC Server")));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.Message)));
                    break;
                }
            }
        }

        private void listenPinningValues()//Listens the Pinning value in the server to turn on the pinning process
        {
            while (true)
            {
                try
                {
                    int pinning = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Pinning);
                    switch (pinning)
                    {
                        case 0:
                            break;
                        case 1:
                            List<String> productQueue = opcClient.GetProductQueue(OPCNodes.NameSpaceValue, OPCNodes.ProductQueue2);
                            String productType = "";
                            try
                            {
                                productType = productQueue[1];
                                if (productType == "Plastic")
                                {
                                    opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Pinning, 2);

                                    break;
                                }
                                else if (productType == "Aluminium")
                                {
                                    Thread pinningThread = new Thread(startPinningProcess);
                                    pinningThread.Start();
                                    pinningThread.Join();
                                    Invoke(new Action(() => opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Pinning, 2)));
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch {
                                break;
                            }
                        case 2:
                            break;
                        case 3:
                            Invoke(new Action(() => updateMessageBox("Error in Pinning Station")));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.Message)));
                    break;
                }
            }
        }

        private void listenAssemblyValues()//Listens the Assembly value in the server to turn on the assembly process
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    int assembly = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Assembly);
                    switch (assembly)
                    {
                        case 0: //Ready to assemble(No component placed)
                            break;
                        case 1: //Start assembly
                            List<String> productQueue = opcClient.GetProductQueue(OPCNodes.NameSpaceValue, OPCNodes.ProductQueue3);
                            String productType = "";
                            try
                            {
                                productType = productQueue[1];
                                if (productType == "Plastic")
                                {
                                    String[] emptyList = new String[1] { "0" };
                                    opcClient.SetProductQueue(OPCNodes.NameSpaceValue, OPCNodes.ProductQueue3, emptyList);
                                    Invoke(new Action(() => opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Assembly, 0)));
                                    break;
                                }
                                else if (productType == "Aluminium")
                                {
                                    Thread assemblyThread = new Thread(startAssembly);
                                    assemblyThread.Start();
                                    assemblyThread.Join();
                                    Invoke(new Action(() => opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Assembly, 2)));
                                    String[] emptyList = new String[1] { "0" };
                                    opcClient.SetProductQueue(OPCNodes.NameSpaceValue, OPCNodes.ProductQueue3, emptyList);
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch
                            {
                                break;
                            }
                        case 2: //Assembly completed
                            break;
                        case 3: //Error
                            Invoke(new Action(() => updateMessageBox("Error in Assembly Station")));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.Message)));
                    break;
                }
            }
        }

        private void listenConveyorValues()//Listens the Conveyor value in the server to turn on the conveyor
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    int conveyor = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Conveyor);
                    switch (conveyor)
                    {
                        case 0: //Ready to move(No component placed)
                            break;
                        case 1: //Move (Component present at intake) 
                            Thread conveyorThread = new Thread(MoveProductToASRS);
                            conveyorThread.Start();
                            conveyorThread.Join();
                            List<String> result = assignProductID_and_TypeID();
                            WarehouseLocation warehouseLocation =  get_empty_warehouse_location();
                            if(warehouseLocation.path != null)
                            {
                                int result_code = -1;
                                Thread asrsThread = new Thread(() => result_code = ASRS_pick_and_place(warehouseLocation.path));
                                asrsThread.Start();
                                asrsThread.Join();
                                if (result_code == 1)
                                {
                                    update_warehouse_location_status(warehouseLocation, result);
                                    update_product_details(warehouseLocation, result);
                                }
                                else if (result_code == 2)
                                {
                                    Invoke(new Action(() => updateMessageBox("Error in ASRS-Pick And Place")));
                                }
                            }
                            else
                            {
                                Invoke(new Action(() => updateMessageBox("Warehouse does not have an empty slot")));
                                
                            }
                            opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.Conveyor, 0);
                            break;
                        case 2: //Moved (Component present at ASRS)
                            break;
                        case 3: //Error
                            Invoke(new Action(() => updateMessageBox("Error in Conveyor 2")));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => updateMessageBox(ex.Message)));
                    break;
                }
            }
        }

        private List<String> assignProductID_and_TypeID()//Assigns Product and Type IDs using products from Queue 4
        {
            List<String> productQueue = opcClient.GetProductQueue(OPCNodes.NameSpaceValue, OPCNodes.ProductQueue);//Change to Q4
            string product1 = (productQueue[productQueue.Count - 2]).Substring(0,2);
            string product2 = (productQueue[productQueue.Count - 1]).Substring(0,2);

            string typeId = $"{product1}{product2}";
            string productId = GenerateProductId(product1, product2);
            List<String> result = new List<String>
            {
                typeId,
                productId                
            };
            return result;
        }



        //MYSQL
        private WarehouseLocation get_empty_warehouse_location()//Gets empty warehouse location and path
        {
            WarehouseLocation warehouseLocation = new WarehouseLocation();
            string  query = "SELECT * FROM Warehouse WHERE  Status = 'Empty' LIMIT 1";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                warehouseLocation.storageLocation = reader.GetString("StorageLocation");
                warehouseLocation.path = reader.GetString("Path");
                warehouseLocation.status = reader.GetString("Status");
                warehouseLocation.productId = reader.GetString("ProductID");
                warehouseLocation.typeId = reader.GetString("TypeID");
            }
            reader.Close();
            return warehouseLocation;
        }

        private void update_warehouse_location_status(WarehouseLocation warehouseLocation, List<String> result)//Adds ProductID and TypeID to placed warehouse location and changes it status
        {
            string query = "UPDATE Warehouse SET TypeID = @TypeID,ProductID = @ProductID, Status = @Status WHERE StorageLocation = @StorageLocation";
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@TypeID", result[0]);
                command.Parameters.AddWithValue("@ProductID", result[1]);
                command.Parameters.AddWithValue("@Status", "Occupied");
                command.Parameters.AddWithValue("@StorageLocation", warehouseLocation.storageLocation);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                    }
                }
            }
        }

        private void update_product_details(WarehouseLocation warehouseLocation, List<String> result)//Adds Storage Location to Product Details and changes it status
        {
            string query = "INSERT INTO ProductDetails (OrderID, ProductID, TypeID, StorageLocation, Status) VALUES (@OrderID, @ProductID, @TypeID, @StorageLocation, @Status)";
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@OrderID", "");
                command.Parameters.AddWithValue("@ProductID", result[1]);
                command.Parameters.AddWithValue("@TypeID", result[0]);
                command.Parameters.AddWithValue("@StorageLocation", warehouseLocation.storageLocation);
                command.Parameters.AddWithValue("@Status", "Consumable");
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                    }
                }
            }
        }

        private void getWarehouseInfo()
        {
            Product product = new Product();
            string status = viewWarehouseStatusComboBox.Text;
            string query;
            if (status == "All") query = "SELECT * FROM Warehouse";
            else query = "SELECT * FROM Warehouse WHERE  Status = " + "'" + status + "'";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            ViewWarehouseDataGridView.Rows.Clear();

            while (reader.Read())
            {
                product.productId = reader.GetString("ProductID");
                product.typeId = reader.GetString("TypeID");
                product.storageLocation = reader.GetString("StorageLocation");
                product.status = reader.GetString("Status");

                int RowIndex = ViewWarehouseDataGridView.Rows.Add();
                ViewWarehouseDataGridView.Rows[RowIndex].Cells["viewWarehouseTypeID"].Value = product.typeId;
                ViewWarehouseDataGridView.Rows[RowIndex].Cells["viewWarehouseStorageLocation"].Value = product.storageLocation;
                ViewWarehouseDataGridView.Rows[RowIndex].Cells["viewWarehouseProductID"].Value = product.productId;
                ViewWarehouseDataGridView.Rows[RowIndex].Cells["viewWarehouseStatus"].Value = product.status;
            }
            reader.Close();
        }

        private void getProductDetails()
        {
            Product product = new Product();
            string status = ProductDetailsStatusComboBox.Text;
            string query;
            if (status == "All") query = "SELECT * FROM ProductDetails";
            else query = "SELECT * FROM ProductDetails WHERE  Status = " + "'" + status + "'";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            ProductDetailsDataGridView.Rows.Clear();

            while (reader.Read())
            {
                product.productId = reader.GetString("ProductID");
                product.typeId = reader.GetString("TypeID");
                product.storageLocation = reader.GetString("StorageLocation");
                product.status = reader.GetString("Status");
                product.orderid = reader.GetString("OrderID");

                int RowIndex = ProductDetailsDataGridView.Rows.Add();
                ProductDetailsDataGridView.Rows[RowIndex].Cells["PDTypeID"].Value = product.typeId;
                ProductDetailsDataGridView.Rows[RowIndex].Cells["PDStorageLocation"].Value = product.storageLocation;
                ProductDetailsDataGridView.Rows[RowIndex].Cells["PDProductID"].Value = product.productId;
                ProductDetailsDataGridView.Rows[RowIndex].Cells["PDStatus"].Value = product.status;
                ProductDetailsDataGridView.Rows[RowIndex].Cells["PDOrderID"].Value = product.orderid;

            }
            reader.Close();
        }

        private int ASRS_pick_and_place(String path)
        {
            try
            {
                opcClient.SetServerValue(OPCNodes.NameSpaceValue, OPCNodes.autoStorage, 1); // To be changed
                opcClient.SetServerStringValue(OPCNodes.NameSpaceValue, OPCNodes.autoStoragePath, path); // To be changed
                int autoStorage = -1;
                while(autoStorage != 0)
                {
                    autoStorage = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.autoStorage);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => updateMessageBox(ex.Message.ToString() + "Error in ASRS Pick and Place")));
                return 2;
            }
        }

        static string GenerateProductId(string product1, string product2)
        {
            Random random = new Random();
            int randomNum = random.Next(1000, 10000);
            string productId = $"{product1}{product2}_{randomNum}";
            return productId;
        }

        


        //GUI
        private void updateMessageBox(string message) //Adds a message to message box
        {
            messageBox.Items.Add(message);
        }

        private void updateGui(List<string> sensorStatusList) //Gets the list of sensor data and updates the GUI
        {
            AlExtendedStatustxt.Text = sensorStatusList[0];
            AlRetractedStatustxt.Text = sensorStatusList[1];
            PlExtendedStatustxt.Text = sensorStatusList[2];
            PlRetractedStatustxt.Text = sensorStatusList[3];
            C1IRStatustxt.Text = sensorStatusList[4];

            PinExtSentxt.Text = sensorStatusList[5];
            PinRetSentxt.Text = sensorStatusList[6];
            PushExtSen.Text = sensorStatusList[7];
            PushRetSen.Text = sensorStatusList[8];

            TrayExtSentxt.Text = sensorStatusList[9];
            TrayRetSentxt.Text = sensorStatusList[10];

            C2Sentxt.Text = sensorStatusList[11];
        }

        private void updateProcessGui(Label label, string msg, System.Drawing.Color color) //Updates the process status in GUI
        {
            label.Text = msg;
            label.ForeColor = color;
        }

        private void btnStartProduction_PCC_Click(object sender, EventArgs e)
        {
            if (opcServerStatus != "Disconnected" && etherCatStatus != "Disconnected" && firebaseStatus != "Disconnected")
            {
                WarehouseLocation warehouselocation =  get_empty_warehouse_location();
                if (warehouselocation.path != null) {
                    int count = Convert.ToInt32(productCountUpDown.Value);
                    if (count != 0) start_Production_for_Order(count);
                    else updateMessageBox("0 products cannot be produced");
                    updateProcessGui(ProductionProcessStatusTxt, "Production in progress", Color.Red);
                    btnStartProduction_PCC.Enabled = false;
                    Thread progressBarThread = new Thread(() => updateProgressBar_PCC(count));
                    progressBarThread.Start();
                }
                else updateMessageBox("Warehouse does not have an empty slot");
            }
            else updateMessageBox("One of your system is inactive");
        }

        private void updateProgressBar_PCC(int count)
        {
            int quantity = -1;
            do
            {
                quantity = opcClient.GetServerValue(OPCNodes.NameSpaceValue, OPCNodes.QuantityR3);
                if (quantity == count) Invoke(new Action(() => productionProgressBar.Value = 0));
                else
                {
                    Invoke(new Action(() => productionProgressBar.Value = (100 - ((quantity * 100) / count))));
                }
            }
            while (quantity != 0);
            Invoke(new Action(() => updateProcessGui(ProductionProcessStatusTxt, "Production Completed", Color.Green)));
            Invoke(new Action(() => btnStartProduction_PCC.Enabled = true));
        }

        private void btnStopProduction_PCC_Click(object sender, EventArgs e)
        {
            start_Production_for_Order(0);
            btnStartProduction_PCC.Enabled = true;
        }
    } 
}