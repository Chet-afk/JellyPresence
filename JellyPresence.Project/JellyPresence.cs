using System.Collections.Generic;
using System.Diagnostics;
using System;
using dotenv.net;
using System.Timers;
using Discord;

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
            jellyfinManager = new JellyfinManager(envDict["JELLYFINAPIKEY"]);
        }

        public static void StartJMP() 
        {
            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();
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
