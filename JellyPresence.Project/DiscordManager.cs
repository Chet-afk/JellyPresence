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
        private RichPresence rp = new RichPresence() 
        { 
            Timestamps = new Timestamps()
        };

        private bool isIdle = false;
        private bool isPaused = false;

        public DiscordManager(DiscordRpcClient client)
        {
            c = client;
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(PlaybackJSON p)
        {
            // Pause is by default false, so books etc wont ever be caught by this.
            if (p.PlayState.IsPaused)
            {
                SetPaused(p.NowPlayingItem);
                return;
            }

            // Just to shorten variable names
            Int64 curTicks = p.PlayState.PositionTicks;
            NowPlayingItemJSON eData = p.NowPlayingItem;

            // Determine Rich Presence based on Type
            switch (p.NowPlayingItem.Type)
            {
                case "Episode":
                    rp.Details = $"Watching {eData.SeriesName}";
                    rp.State = eData.Name;
                    break;
                case "Movie":
                    rp.Details = $"Watching {eData.Name}";
                    rp.State = null;
                    break;
                case "Book":
                    break;

                default: // Should never get here
                    return;
            }



            DateTime start = DateTime.UtcNow.AddSeconds(0 - ((curTicks * 100) / 1000000000));

            // Set Rich Presence details
            rp.Timestamps.Start = start;
            DateTime end = start.AddSeconds((eData.RunTimeTicks * 100) / 1000000000);
            rp.Timestamps.End = end;
            rp.Type = ActivityType.Watching;

            c.SetPresence(rp);

            isIdle = false;
            isPaused = false;
            
        }

        //TODO: Add image changing (once imgur api implemented?)
        private void SetPaused(NowPlayingItemJSON n)
        {
            if (isPaused) { return; }
            string show;

            switch(n.Type)
            {
                case ("Episode"):
                    show = n.SeriesName;
                    break;
                case ("Movie"):
                    show = n.Name;
                    break;
                default:
                    show = "";
                    break;
            }

            // Set paused small image here probably
            c.SetPresence(new RichPresence()
            {
                Details = $"Watching {show}",
                State = "(Paused)",
                Type = ActivityType.Watching
            });

            isPaused = true;
            isIdle = false;
        }


        // Functionally a reset 
        public void SetIdle()
        {

            // No need to constantly update if already idle.
            if (isIdle) { return; }
            c.SetPresence(new RichPresence()
            {
                State = "",
                Details = "Idle",
                Timestamps = idle,
                Type = ActivityType.Watching
            });

            isIdle = true;
            isPaused = false;
        }
    }
}
