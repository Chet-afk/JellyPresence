using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DiscordRPC;

namespace JellyPresence.Project
{
    public class DiscordManager
    {
        DiscordRpcClient c;

        public DiscordManager(DiscordRpcClient client)
        {
            c = client;
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(string showTitle, string userActivity,
            Int64 currentTime, Int64 runtime)
        {


            Int64 endTimeEpoch = (Int64)(((runtime * 100) - (currentTime * 100)) / 1000000000);
            Int64 currentTimeEpoch = (Int64)DateTimeOffset.Now.ToUnixTimeSeconds();

            DateTime end = DateTime.UnixEpoch.AddSeconds(endTimeEpoch + currentTimeEpoch);




            Console.WriteLine($"{currentTimeEpoch} {endTimeEpoch} {endTimeEpoch + currentTimeEpoch}");


            c.SetPresence(new RichPresence()
            {
                State = userActivity,
                Details = showTitle,
                Timestamps = new Timestamps()
                { End = end },
            });

        }

        public void SetActivity()
        {

            c.SetPresence(new RichPresence()
            {
                State = "",
                Details = "Idle",
                Buttons = new Button[]
                {
                    new Button() { Label = "a", Url = "https://google.com" }
                }
            });

        }
    }
}
