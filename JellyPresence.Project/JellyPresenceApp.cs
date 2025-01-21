
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using DiscordRPC;
using DiscordRPC.Logging;


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

            DiscordRpcClient client = new DiscordRpcClient(config.GetVal("CLIENTID"));
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };
            client.Initialize();


            DiscordManager dMan = new DiscordManager(client);
            JellyfinManager jMan = new JellyfinManager(config.GetVal("JELLYAPIKEY"), 
                config.GetVal("JELLYURL"));

            while (jMan.GetJellyID())
            {
                jMan.QueryServerDevice();
            }


            Int64 ticks = -50000001;

            while (true)
            {
                
                if (Process.GetProcessesByName("JellyfinMediaPlayer").Length == 0)
                {
                    client.Dispose();
                    Environment.Exit(0);
                }

                jMan.QueryServer();

                if (jMan.MissingFields()) { dMan.SetIdle(); ticks = -50000001; }

                // Limits amount of SetPresence calls to every 5 seconds,
                // or if user is jumping to different spots
                else if (jMan.p.PlayState.PositionTicks > ticks + 50000000
                    || jMan.p.PlayState.PositionTicks < ticks - 50000000)
                {

                    dMan.SetActivity(jMan.p.NowPlayingItem.SeriesName,
                            jMan.p.NowPlayingItem.Name,
                            jMan.p.PlayState.PositionTicks);

                    ticks = jMan.p.PlayState.PositionTicks;
                }

                else if (jMan.p.PlayState.IsPaused)
                {
                    dMan.SetPaused(jMan.p.NowPlayingItem.SeriesName,
                                     jMan.p.NowPlayingItem.Name);
                }

                Thread.Sleep(1000);



            }

        }

    }
}
