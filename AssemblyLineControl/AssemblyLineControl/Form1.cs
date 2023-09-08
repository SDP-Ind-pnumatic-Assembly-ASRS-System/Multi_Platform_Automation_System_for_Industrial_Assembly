using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

namespace AssemblyLineControl
{
    public partial class Form1 : Form
    {
        EtherCat et;
        OpcClient client;
        string AMSNETIDvar = "10.1.2.54.1.1";
        int portvar = 851;
        string IPAddress = "192.168.39.96";
        int IPport = 4841;

        public Form1()
        {
            InitializeComponent();
            AmsnetidBox.Text = AMSNETIDvar;
            AmsnetidportBox.Text = portvar.ToString();
            ipAddressBox.Text = IPAddress;
            ipPortBox.Text = IPport.ToString();

            try
            {
                messageBox.Items.Add("Connecting to EtherCat");
                et = new EtherCat(AMSNETIDvar, portvar);
                messageBox.Items.Add("Successfully connected to EtherCat");
            }
            catch (Exception ex)
            {
                messageBox.Items.Add(ex.Message);
            }
            var connection_status = et.getConnectionStatus();
            if (connection_status == 1)
            {
                Thread thread = new Thread(getSensorStatus);
                thread.Start();
                Statustxt.Text = "Connected";
                Statustxt.ForeColor = Color.Green;
            }
            else
            {
                Statustxt.Text = "Dissconnected";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnEstablish_Connection_Click(object sender, EventArgs e)
        {
            if(AmsnetidBox.Text.ToString() != "" && AmsnetidportBox.Text.ToString() != "")
            {
                AMSNETIDvar = AmsnetidBox.Text;
                portvar = Convert.ToInt32(AmsnetidportBox.Text);
                try
                {
                    messageBox.Items.Add("Connecting to EtherCat");
                    et = new EtherCat(AMSNETIDvar, portvar);
                
                    messageBox.Items.Add("Successfully connected to EtherCat");
                    Thread thread = new Thread(getSensorStatus);
                    thread.Start();
                }
                catch (Exception ex)
                {
                    messageBox.Items.Add(ex.Message);
                }
                var connection_status = et.getConnectionStatus();
                if (connection_status == 1)
                {
                    Statustxt.Text = "Connected";
                    Statustxt.ForeColor = Color.Green;
                }
                else
                {
                    Statustxt.Text = "Dissconnected";
                }
            }
            else
            {
                messageBox.Items.Add("Please enter valid AMSNETID and PORT");
            }
        }

        private void establishServerConnection()
        {
            if (ipAddressBox.Text.ToString() != "" && ipPortBox.Text.ToString() != "")
            {
                string ipaddress = ipAddressBox.Text;
                string ipPort = ipPortBox.Text;
                string url = String.Format("opc.tcp://{0}:{1}", ipaddress, ipPort);
                client = new OpcClient(url);
                try
                {
                    messageBox.Items.Add("Connecting to OPC UA Server");
                    client.Connect();
                    messageBox.Items.Add("Successfully connected to OPC UA Server");
                }
                catch (Exception ex)
                {
                    messageBox.Items.Add(ex.Message);
                }
            }
            else
            {
                messageBox.Items.Add("Please enter valid IPAddress and Port");
            }
        }

        private void getSensorStatus()
        {
            List<string> sensorStatusList = new List<string>();
            while (true)
            {
                try
                {
                    sensorStatusList = et.getStatus();
                    Invoke(new Action<List<string>>(updateGui), sensorStatusList);
                }
                catch
                {
                    break;
                }
            }
        }

        private void updateGui(List<string> sensorStatusList)
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

        private void startPinningProcess()
        {
            Invoke(new Action(() => updateProcessGui(PinningStatustxt, "On going", Color.Green)));
            Thread.Sleep(2000);
            et.setValue("PushExtend", true);
            et.setValue("PinExtend", true);
            Thread.Sleep(2000);
            et.setValue("PushExtend", false);
            Thread.Sleep(1000);
            et.setValue("PinExtend", false);
            SetServerValue(2,37,1);
            Invoke(new Action(() => updateProcessGui(PinningStatustxt, "Completed", Color.Red)));
        }

        private void updateProcessGui(Label label, string msg, System.Drawing.Color color)
        {
            label.Text = msg;
            label.ForeColor = color;
        }

        private void startAssembly()
        {
            Invoke(new Action(() => updateProcessGui(AssemblyStatustxt, "On going", Color.Green)));
            et.setValue("TrayRetract", false);
            et.setValue("TrayExtend", true);
            et.setValue("ComExtend", false);
            et.setValue("ComRetract", true);
            Thread.Sleep(5000);
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
        }

        private void MoveProductToASRS()
        {
            Invoke(new Action(() => updateProcessGui(MoveToASRStxt, "On going", Color.Green)));
            while (C2Sentxt.Text.ToString() == "False")
            {
                et.setValue("C2Fwd", true);
            }
            Thread.Sleep(400);
            et.setValue("C2Fwd", false);
            Invoke(new Action(() => updateProcessGui(MoveToASRStxt, "Completed", Color.Red)));
        }


        private void CompIntakeProcess()
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
            SetServerValue(2, 12, 1);
            Invoke(new Action(() => updateProcessGui(ComponentsIntaketxt, "Completed", Color.Red)));
        }

        
        
        private void SetServerValue(int ns, int i, int value)
        {
            
            var tagName = String.Format("ns={0};i={1}",ns,i);
            try
            {
                Thread.Sleep(1000);
                client.WriteNode(tagName, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
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

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void btnStartFullProcess_Click(object sender, EventArgs e)
        {
            object threadParameters = new {a = true};
            Thread thread = new Thread(startProcess1);
            thread.Start(threadParameters);
        }

        private void setup()
        {

        }

        private void startProcess1(object parameters)
        {
            var threadParams = (dynamic)parameters;

            // Access the values
            bool a = threadParams.a;

            string firstProduct = "";
            if (a == true)
            {
                firstProduct = "Pl";
            }
            else
            {
                firstProduct = "Al";
            }
            placeProduct(firstProduct);
            et.setValue("C1Fwd", true);
            Thread.Sleep(4000);
            et.setValue("C1Fwd", false);
            string s = getQR();
            if(s == "Defective")
            {
                if (firstProduct == "Pl")
                {
                    object threadParameters = new { a = true };
                    Thread thread = new Thread(startProcess1);
                    thread.Start(threadParameters);
                }
                else if(firstProduct == "Al")
                {
                    object threadParameters = new { a = false };
                    Thread thread = new Thread(startProcess1);
                    thread.Start(threadParameters);
                }
               
            }
            else if (s == "Non Defective")
            {
                if(firstProduct == "Pl")
                {
                    object threadParameters = new { a = false };
                    Thread thread = new Thread(startProcess1);
                    thread.Start(threadParameters);
                    while (C1IRStatustxt.Text == "False")
                    {
                        et.setValue("C1Fwd", true);
                    }
                    et.setValue("C1Fwd", false);
                    SetServerValue(2, 36, 1);
                    SetServerValue(2,39,1);
                    while(C1IRStatustxt.Text == "True")
                    {
                        et.setValue("C1Fwd", false);
                    }
                    et.setValue("C1Fwd", true);
                    while (C1IRStatustxt.Text == "False")
                    {
                        et.setValue("C1Fwd", true);
                    }
                    et.setValue("C1Fwd", false);
                    SetServerValue(2, 36, 1);
                    SetServerValue(2, 39, 1);
                }
                else
                {
                    while(C1IRStatustxt.Text == "False")
                    {
                        et.setValue("C1Fwd", true);
                    }
                    et.setValue("C1Fwd", false);
                    SetServerValue(2, 36, 1);
                    SetServerValue(2, 39, 0);
                }
            }
            else
            {
                et.setValue("C1Fwd", false);
            }
        }

        private string getQR()
        {
            return txtDefNonDef.Text.ToString();
        }

        private void placeProduct(string type)
        {
            if (type == "Al")
            {
                type = "Pl";
            }
            else if(type == "Pl")
            {
                type = "Al";
            }
            var ExtendStr = String.Format("{0}Extend",type);
            var RetractStr = String.Format("{0}Retract",type);
            et.setValue(ExtendStr, true);
            Thread.Sleep(500);
            et.setValue(ExtendStr, false);
            et.setValue(RetractStr, true);
            Thread.Sleep(500);
            et.setValue(RetractStr, false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtDefNonDef.Text = "Non Defective";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtDefNonDef.Text = "Defective";
        }

        private void messageBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            establishServerConnection();
        }
    }
}
