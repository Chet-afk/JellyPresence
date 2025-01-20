
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
                    
                    dMan.SetActivity($"Watching {jMan.p.NowPlayingItem.SeriesName}",
                            jMan.p.NowPlayingItem.Name,
                            jMan.p.PlayState.PositionTicks,
                            jMan.p.NowPlayingItem.RunTimeTicks);
                }
                Thread.Sleep(1000);



            }

        }

    }
}
