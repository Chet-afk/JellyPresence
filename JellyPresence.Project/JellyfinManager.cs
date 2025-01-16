using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JellyPresence.Project
{
    public class JellyfinManager
    {
        private readonly string APIKEY;
        private readonly string deviceName = Environment.MachineName;

        public JellyfinManager(string apiKey)
        {
            APIKEY = apiKey;
        }
    }
}
