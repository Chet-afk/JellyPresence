using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace JellyPresence.Project
{
    public class JellyfinManager
    {
        private readonly string APIKEY;
        private readonly string serverURL;
        private readonly string deviceName = Environment.MachineName;
        private string deviceJellyID;

        private HttpClient client = new HttpClient(); 


        public JellyfinManager(string apiKey, string jellyfinURL)
        {
            APIKEY = apiKey;
            serverURL = jellyfinURL;


            // Discard to stop CS4014 warning
            // Fire and forget lets goooooo
            _ = QueryServer();
        }

        private async Task QueryServer()
        {

            // First get current machines device ID for easier filtering
            try
            {
                HttpResponseMessage r = await client.GetAsync(serverURL + "/Devices?apikey=" + APIKEY);
                r.EnsureSuccessStatusCode();
                string s = await r.Content.ReadAsStringAsync();
                ItemsJSON i = JsonSerializer.Deserialize<ItemsJSON>(s);
                Console.WriteLine(i);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            //while (true)
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(3));
            //    try
            //    {
            //        HttpResponseMessage r = await client.GetAsync(serverURL + "/Sessions?apikey=" + APIKEY);
            //        r.EnsureSuccessStatusCode();
            //        string b = await r.Content.ReadAsStringAsync();
            //        Console.WriteLine(b);

            //    }
            //    catch (HttpRequestException e)
            //    {
            //        Console.WriteLine($"{e.Message}");
            //    }
            //}
        }
    }

    public class ItemsJSON
    {
        public IList<DevicesJSON> Items { get; set; }
    }

    public class DevicesJSON
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
