using System.Collections.Generic;
using System.Diagnostics;
using System;
using dotenv.net;
using System.Timers;


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
            var timer = new Timer(1000);
            timer.Elapsed += UpdateEvent;
            timer.AutoReset = true;
            timer.Start();
        }

        private void UpdateEvent(Object s, ElapsedEventArgs e)
        {
            if (jellyfinManager.p == null) { }
            else
            {
                discordManager.SetActivity("Watching " + jellyfinManager.p.NowPlayingItem.SeriesName,
                    "Episode: " + jellyfinManager.p.NowPlayingItem.Name,
                    jellyfinManager.p.PlayState.PositionTicks,
                    jellyfinManager.p.NowPlayingItem.RunTimeTicks);
            }
            
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
