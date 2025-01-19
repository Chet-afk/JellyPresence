using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
        private string deviceJellyID =  "";

        private readonly HttpRequestMessage getDevice;
        private HttpRequestMessage getInfo;

        public PlaybackJSON p;

        private HttpClient client = new HttpClient(); 


        public JellyfinManager(string apiKey, string jellyfinURL)
        {
            APIKEY = apiKey;
            serverURL = jellyfinURL;
            getDevice = new HttpRequestMessage(HttpMethod.Get, serverURL + "/Devices?apikey=" + APIKEY);
        }

        public bool MissingFields()
        {
            // True = not viewing anything / haven't claimed all info
            if (p is null ||
                p.PlayState is null ||
                p.NowPlayingItem is null ||
                p.NowPlayingItem.SeriesName is null ||
                p.NowPlayingItem.Name is null ||
                p.NowPlayingItem.SeriesId is null ||
                p.NowPlayingItem.RunTimeTicks == 0) {  return true; }
            return false;
        }

        public void QueryServerDevice()
        {
            try
            {
                HttpResponseMessage r = client.Send(getDevice);
                r.EnsureSuccessStatusCode();
                string s = new StreamReader(r.Content.ReadAsStream()).ReadToEnd();

                ItemsJSON i = JsonSerializer.Deserialize<ItemsJSON>(s);
                foreach (DevicesJSON d in i.Items)
                {
                    if (d.Name == deviceName)
                    {
                        deviceJellyID = d.Id;
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        public void QueryServer()
        {

            getInfo = new HttpRequestMessage(HttpMethod.Get, 
                serverURL + "/Sessions?deviceId=" + deviceJellyID + "&apikey=" + APIKEY);

            try
            {
                HttpResponseMessage r = client.Send(getInfo);
                r.EnsureSuccessStatusCode();
                string b = new StreamReader(r.Content.ReadAsStream()).ReadToEnd();

                if (b == "[]")
                {
                    p = null; 
                    return; 
                }

                // I dont know why but the json is wrapped in an array of size 1 before getting
                // to the actual information. This just removes the array portion
                b = b.Substring(1, b.Length - 2);

                p = JsonSerializer.Deserialize<PlaybackJSON>(b);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }
        public bool GetJellyID()
        {
            return deviceJellyID == "";
        }
    }

    // Classes to hold json return vals
    public class ItemsJSON
    {
        public IEnumerable<DevicesJSON> Items { get; set; }
    }

    public class DevicesJSON
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    // Classes for playback vals
    public class PlaybackJSON
    {
        public PlaystateJSON PlayState { get; set; }
        public NowPlayingItemJSON NowPlayingItem { get; set; }
    }
    public class PlaystateJSON
    {
        // 1 Tick = 100 Nanoseconds
        public Int64 PositionTicks {  get; set; }
        public bool IsPaused { get; set; }
    }
    public class NowPlayingItemJSON
    {
        // Episode name
        public string Name { get; set; } = null;
        // Show name
        public string SeriesName { get; set; } = null;

        public string SeriesId { get; set; } = null;
        public Int64 RunTimeTicks { get; set; } = 0;
        
        // Just in case need it for later
        //public string Id { get; set; }
    }
}
