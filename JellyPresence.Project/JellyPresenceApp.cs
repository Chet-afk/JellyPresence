
using System;
using System.Diagnostics;
using System.Threading;
using Discord;

namespace JellyPresence.Project
{
    public class JellyPresenceApp
    {

        public static void Main()
        {

            Config config = new Config();

            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();

            Discord.Discord client = new Discord.Discord(long.Parse(config.GetVal("CLIENTID")), 
                (UInt64)Discord.CreateFlags.Default);

            DiscordManager dMan = new DiscordManager(client);
            JellyfinManager jMan = new JellyfinManager(config.GetVal("JELLYAPIKEY"), 
                config.GetVal("JELLYURL"));

            while (jMan.GetJellyID())
            {
                jMan.QueryServerDevice();
            }


            while (true)
            {
                
                if (Process.GetProcessesByName("JellyfinMediaPlayer").Length == 0)
                {
                    client.Dispose();
                    Environment.Exit(0);
                }

                jMan.QueryServer();

                if (jMan.MissingFields()) { dMan.SetActivity(); }
                else
                {
                    dMan.SetActivity("Watching " + jMan.p.NowPlayingItem.SeriesName,
                            jMan.p.NowPlayingItem.Name,
                            jMan.p.PlayState.PositionTicks,
                            jMan.p.NowPlayingItem.RunTimeTicks);
                }
                Thread.Sleep(1000);
                client.RunCallbacks();



            }

        }

    }
}
