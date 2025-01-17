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

        private string title;

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

            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                try
                {
                    HttpResponseMessage r = await client.GetAsync(serverURL + "/Sessions?deviceId=" + deviceJellyID 
                        + "&apikey=" + APIKEY);
                    r.EnsureSuccessStatusCode();
                    string b = await r.Content.ReadAsStringAsync();

                    // I dont know why but the json is wrapped in an array of size 1 before getting
                    // to the actual information. This just removes the array portion
                    b = b.Substring(1, b.Length-2);
                    
                    PlaybackJSON p = JsonSerializer.Deserialize<PlaybackJSON>(b);
                    Console.WriteLine(p.NowPlayingItem.Name + " " + p.PlayState.PositionTicks);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
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
        public long PositionTicks {  get; set; }
        public bool IsPaused {  get; set; }
    }
    public class NowPlayingItemJSON
    {
        // Episode name
        public string Name { get; set; }
        public long RunTimeTicks { get; set; }
        public string SeriesName { get; set; }
        public string SeriesId {  get; set; }
        // Just in case need it for later
        //public string Id { get; set; }
    }
}
