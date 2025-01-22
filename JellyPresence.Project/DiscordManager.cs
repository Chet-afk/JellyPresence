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

        Int64 ticks = -50000001; // Check for updates

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

            // Determine Rich Presence based on Type
            switch (p.NowPlayingItem.Type)
            {
                case "Episode":
                    SetEpisode(p.PlayState.PositionTicks, 
                        p.NowPlayingItem.Name, p.NowPlayingItem.SeriesName);
                    break;
                case "Movie":
                    SetMovie(p.PlayState.PositionTicks, p.NowPlayingItem.Name);
                    break;
                case "Book":
                    break;

                default: // Should never get here
                    return;
            }

            isIdle = false;
            isPaused = false;
            
        }

        private void SetEpisode(Int64 curTicks, string episodeName, string showTitle)
        {

            // Dont bother setting new presence if until 5 seconds have passed
            // or user is clicking through video
            if (curTicks < ticks + 50000000 && curTicks > ticks - 50000000) { return; }
            
            // I dont think discord does time left anymore lol, can't update that.
            // Just use current time to show "how far" into it you are
            DateTime secondsIn = DateTime.UtcNow.AddSeconds(0 - ((curTicks * 100) / 1000000000));

            c.SetPresence(new RichPresence()
            {
                Details = $"Watching {showTitle}",
                State = episodeName,
                Timestamps = new Timestamps(secondsIn),
                Type = ActivityType.Watching
            });
            ticks = curTicks;
        }

        private void SetMovie(Int64 curTicks, string Title)
        {
            if (curTicks < ticks + 50000000 && curTicks > ticks - 50000000) { return; }
            DateTime secondsIn = DateTime.UtcNow.AddSeconds(0 - ((curTicks * 100) / 1000000000));

            c.SetPresence(new RichPresence()
            {
                Details = $"Watching {Title}",
                Timestamps = new Timestamps(secondsIn),
                Type = ActivityType.Watching
            });
            ticks = curTicks;
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
        }


        // Functionally a reset 
        public void SetIdle()
        {

            // No need to constantly update if already idle.
            if (isIdle) { return; }
            RichPresence a = new RichPresence();
            c.SetPresence(new RichPresence()
            {
                State = "",
                Details = "Idle",
                Timestamps = idle,
                Type = ActivityType.Watching
            });

            isIdle = true;
            isPaused = false;
            ticks = -50000001;

        }
    }
}
