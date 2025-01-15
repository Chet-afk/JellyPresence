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

        private string jmpPath;
        private long clientID;

        private Discord.Discord client;
        private ActivityManager actManager;
        private Activity activity = new Discord.Activity();

        public JellyPresence()
        {
            envDict = DotEnv.Read();
            if (!Config.envIntegrity(envDict))
            {
                throw new InvalidENVException("ENV Invalid");
            }

            clientID = long.Parse(envDict["CLIENTID"]);

            // maybe change to json so dont gotta convert to long lol
            client = new Discord.Discord(clientID, (UInt64)Discord.CreateFlags.Default);
            actManager = client.GetActivityManager();

        }

        public static void StartJMP() 
        {
            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();
        }


        public void StartDiscordUpdate()
        {
            activity.State = "Test";
            activity.Details = "Details testing";
            actManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Console.WriteLine("OK");
                }
            });

            var timer = new Timer(500);
            timer.Elapsed += UpdateEvent;
            timer.AutoReset = true;
            timer.Start();
        }

        public void UpdateEvent(Object s, ElapsedEventArgs e)
        {
            client.RunCallbacks();
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
