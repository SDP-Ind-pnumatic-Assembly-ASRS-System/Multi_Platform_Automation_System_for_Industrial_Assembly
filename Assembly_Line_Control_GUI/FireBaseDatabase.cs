using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FireSharp.Config;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AssemblyLineControl
{
    public class FireBaseDatabase
    {
        private IFirebaseConfig config = new FirebaseConfig();

        IFirebaseClient client;
        public FireBaseDatabase(string authsecret, string basepath) {
            config = new FirebaseConfig
            {
                AuthSecret = authsecret,
                BasePath = basepath
            };
            try
            {
                client = new FireSharp.FirebaseClient(config);
                if (client != null)
                {
                    Console.WriteLine("Connected to Firebase");
                }
                else
                {
                    Console.WriteLine("Connection Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool getConnectionStatus()
        {
            if (client != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<JObject> retreiveData(string nodePath)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync(nodePath);
                if (response != null)
                {
                    string json = response.Body; // Get the JSON content as a string
                    JObject jsonObject = JObject.Parse(json); // Parse the JSON string into a JObject
                    return jsonObject;
                }
                else { return null; }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
      
