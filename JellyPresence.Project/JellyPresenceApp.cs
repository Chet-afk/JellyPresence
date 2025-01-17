
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
                    string series = jMan.p.NowPlayingItem.SeriesName;
                    string episode = jMan.p.NowPlayingItem.Name;
                    long cur_pos = jMan.p.PlayState.PositionTicks;
                    long runtime = jMan.p.NowPlayingItem.RunTimeTicks;
                    dMan.SetActivity(client, "Watching " + series,
                            "Episode: " + episode,
                            cur_pos,
                            runtime);
                }
                Thread.Sleep(1000);
                client.RunCallbacks();



            }

        }

    }
}
