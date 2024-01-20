using Opc.UaFx;
using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblyLineControl
{
    public class OPCServer
    {
        private OpcClient client = null;
        public OPCServer(String ipaddress, String ipPort)
        {
            string url = String.Format("opc.tcp://{0}:{1}", ipaddress, ipPort);
            client = new OpcClient(url);
            try
            {
                Console.WriteLine("Connecting to OPC UA Server");
                client.Connect();
                Console.WriteLine("Successfully connected to OPC UA Server");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetServerValue(int ns, int i, int value)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                client.WriteNode(tagName, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SetServerStringValue(int ns, int i, String value)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                client.WriteNode(tagName, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public String GetServerStringValue(int ns, int i)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                return client.ReadNode(tagName).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "3";
            }
        }

        public int GetServerValue(int ns, int i)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                return int.Parse(client.ReadNode(tagName).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 3;
            }
        }

        public List<String> GetProductQueue(int ns, int i)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                OpcValue inputString = client.ReadNode(tagName);
                String[] arrayStr = (String[])inputString.Value;
                List<String> stringList = new List<String>();
                foreach (String x in arrayStr)
                {
                    stringList.Add(x);
                }
                return stringList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                List<string> stringList = new List<string> { "1" };
                return stringList;
            }
        }

        public void SetProductQueue(int ns, int i, String[] value)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                client.WriteNode(tagName, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SetPath(int ns, int i, int[] value)
        {
            var tagName = String.Format("ns={0};i={1}", ns, i);
            try
            {
                client.WriteNode(tagName, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
