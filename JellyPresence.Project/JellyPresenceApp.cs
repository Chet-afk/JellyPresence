
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

            Discord.Discord client = new Discord.Discord(long.Parse(config.GetVal("CLIENTID")), 
                (UInt64)Discord.CreateFlags.Default);

            DiscordManager dMan = new DiscordManager();
            JellyfinManager jMan = new JellyfinManager(config.GetVal("JELLYAPIKEY"), 
                config.GetVal("JELLYURL"));




            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();

            while (true)
            {
                if (Process.GetProcessesByName("JellyfinMediaPlayer").Length == 0)
                {
                    client.Dispose();
                    Environment.Exit(0);
                }

                // Sleep the thread when changing activity values
                // Otherwise protected memory clashes will happen
                if (jMan.p == null || 
                    jMan.p.NowPlayingItem.SeriesName == null || 
                    jMan.p.NowPlayingItem.Name == null) { }
                else
                {
                    dMan.SetActivity(client, "Watching " + jMan.p.NowPlayingItem.SeriesName,
                            "Episode: " + jMan.p.NowPlayingItem.Name,
                            jMan.p.PlayState.PositionTicks,
                            jMan.p.NowPlayingItem.RunTimeTicks);
                }
                Thread.Sleep(500);
                client.RunCallbacks();
                Thread.Sleep(500);



            }

        }

    }
}
