
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
            process.StartInfo.FileName = config.GetVal("JELLYPATH");
            process.Start();

            DiscordRpcClient client = new DiscordRpcClient(config.GetVal("CLIENTID"));
            
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

                if (jMan.MissingFields()) { dMan.SetIdle(); }

                else
                {
                    dMan.SetActivity(jMan.p);

                }

                
                Thread.Sleep(3000);



            }

        }

    }
}
