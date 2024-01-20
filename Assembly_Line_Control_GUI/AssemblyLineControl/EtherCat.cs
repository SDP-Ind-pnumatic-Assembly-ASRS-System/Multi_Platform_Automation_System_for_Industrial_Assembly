using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwinCAT.Ads;

namespace AssemblyLineControl
{
    public class EtherCat
    {
        private AdsClient client = null;

        public EtherCat(string AMSNETID, int Port) {
            client = new AdsClient();
            client.Connect(AMSNETID, Port);

            if (client.IsConnected)
            {
                Console.WriteLine("Client has been initialised");
                Console.WriteLine(client.ClientAddress);
            }
            else
            {
                Console.WriteLine("Client connection has not been initialised");
            }
        }

        public int getConnectionStatus()
        {
            if (client.IsConnected)
            {
                return 1;
            }
            else if (client == null) {
                return 0;
            }
            return 0;
        }
        
        public List<string> getStatus()
        {
            var alextend = client.CreateVariableHandle("GVL.AlExtendSense");
            var alretract = client.CreateVariableHandle("GVL.AlRetractSense");
            var plextend = client.CreateVariableHandle("GVL.PlExtendSense");
            var plretract = client.CreateVariableHandle("GVL.PlRetractSense");
            var C1Sen = client.CreateVariableHandle("GVL.C1Sen");

            var PinExtSen = client.CreateVariableHandle("GVL.PinExtSen");
            var PinRetSen = client.CreateVariableHandle("GVL.PinRetSen");
            var PushExtSen = client.CreateVariableHandle("GVL.PushExtSen");
            var PushRetSen = client.CreateVariableHandle("GVL.PushRetSen");

            var TrayExtSen = client.CreateVariableHandle("GVL.TrayExtSen");
            var TrayRetSen = client.CreateVariableHandle("GVL.TrayRetSen");

            var C2Sen = client.CreateVariableHandle("GVL.C2Sen");

            var alextendvalue = client.ReadAny(alextend, typeof(bool));
            var alretractvalue = client.ReadAny(alretract, typeof(bool));
            var plextendvalue = client.ReadAny(plextend, typeof(bool));
            var plretractvalue = client.ReadAny(plretract, typeof(bool));
            var C1Senvalue = client.ReadAny(C1Sen, typeof(bool));

            var PinExtSenvalue = client.ReadAny(PinExtSen, typeof(bool));
            var PinRetSenvalue = client.ReadAny(PinRetSen, typeof(bool));
            var PushExtSenvalue = client.ReadAny(PushExtSen, typeof(bool));
            var PushRetSenvalue = client.ReadAny(PushRetSen, typeof(bool));

            var TrayExtSenvalue = client.ReadAny(TrayExtSen, typeof(bool));
            var TrayRetSenvalue = client.ReadAny(TrayRetSen, typeof(bool));

            var C2Senvalue = client.ReadAny(C2Sen, typeof(bool));

            return new List<string>() { alextendvalue.ToString(), alretractvalue.ToString(), plextendvalue.ToString(),
            plretractvalue.ToString(), C1Senvalue.ToString(), PinExtSenvalue.ToString(), PinRetSenvalue.ToString(),
            PushExtSenvalue.ToString(), PushRetSenvalue.ToString(), TrayExtSenvalue.ToString(), TrayRetSenvalue.ToString(),
            C2Senvalue.ToString()};
        }

        public void setValue(string objName, bool value)
        {
            string formattedString = string.Format("GVL.{0}",objName);
            var x = client.CreateVariableHandle(formattedString);
            client.WriteAny(x, value);
        }
    }
}
