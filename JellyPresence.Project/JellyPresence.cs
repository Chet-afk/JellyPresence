using System.Collections.Generic;
using System.Diagnostics;
using System;
using dotenv.net;
using System.Timers;
using System.Threading.Tasks;
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

            jellyfinManager = new JellyfinManager(envDict["JELLYAPIKEY"], envDict["JELLYURL"]);
            UpdateInfo();
        }

        public static void StartJMP() 
        {
            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();
        }

        // Create an timer to 
        // update discord activity stuff
        private void UpdateInfo()
        {
            
            while (true)
            {
                if (jellyfinManager.p == null) { }
                else
                {
                    //discordManager.SetActivity("Watching " + jellyfinManager.p.NowPlayingItem.SeriesName,
                    //    "Episode: " + jellyfinManager.p.NowPlayingItem.Name,
                    //    jellyfinManager.p.PlayState.PositionTicks,
                    //    jellyfinManager.p.NowPlayingItem.RunTimeTicks);
                }
                Thread.Sleep(1000);
            }
        }

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
