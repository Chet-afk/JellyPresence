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
        private Timestamps idle = new Timestamps(DateTime.UtcNow);
        private bool isIdle = false;
        private bool isPaused = false;

        public DiscordManager(DiscordRpcClient client)
        {
            c = client;
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(string showTitle, string userActivity,
            Int64 currentTime)
        {

            // I dont think discord does time left anymore lol, can't update that.
            //long secondsToEnd = (((runtime * 100) - (currentTime * 100)) / 1000000000);
            //DateTime end = DateTime.UtcNow.AddSeconds(secondsToEnd);

            // Just use current time to show "how far" into it you are
            DateTime secondsIn = DateTime.UtcNow.AddSeconds(0-((currentTime * 100) / 1000000000));


            c.SetPresence(new RichPresence()
            {
                State = userActivity,
                Details = $"Watching {showTitle}",
                Timestamps = new Timestamps(secondsIn)
            });

            isIdle = false;
            isPaused = false;
            
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetPaused(string showTitle, string userActivity)
        {
            if (isPaused) { return; }

            // Set paused small image here probably
            c.SetPresence(new RichPresence()
            {
                State = $"(Paused) {userActivity}",
                Details = $"Watching {showTitle}"
            });

            isPaused = true;
        }

        public void SetIdle()
        {

            // No need to constantly update if already idle.
            if (isIdle) { return; }

            c.SetPresence(new RichPresence()
            {
                State = "",
                Details = "Idle",
                Timestamps = idle
            });

            isIdle = true;
            isPaused = false;

        }
    }
}
