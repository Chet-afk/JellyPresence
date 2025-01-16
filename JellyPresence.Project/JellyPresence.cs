using System.Collections.Generic;
using System.Diagnostics;
using System;
using dotenv.net;
using System.Timers;
using Discord;
using System.Threading;

namespace JellyPresence.Project
{
    public class JellyPresence
    {

        private readonly IDictionary<string, string> envDict;
        private DiscordManager discordManager;
        private JellyfinManager jellyfinManager;

        private string jmpPath;

        public JellyPresence()
        {
            envDict = DotEnv.Read();
            if (!Config.envIntegrity(envDict))
            {
                throw new InvalidENVException("ENV Invalid");
            }

            discordManager = new DiscordManager(long.Parse(envDict["CLIENTID"]));
            jellyfinManager = new JellyfinManager(envDict["JELLYAPIKEY"], envDict["JELLYURL"]);
        }

        public static void StartJMP() 
        {
            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();
        }

        // Create an timer to 
        // query jellymanager and update discordmanager activity

        private void UpdateEvent(Object s, ElapsedEventArgs e)
        {
            
        }

    }

    public class InvalidENVException : Exception
    {
        public InvalidENVException() { }
        public InvalidENVException(string message)
            : base(message)
        { }
        public InvalidENVException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
