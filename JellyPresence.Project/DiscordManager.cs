using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Discord;

namespace JellyPresence.Project
{
    public class DiscordManager
    {
        private DateTime epochStart = new DateTime(1970, 1, 1);
        private ActivityManager actMan;
        private Activity activity = new Activity();

        public DiscordManager(Discord.Discord client)
        {
            this.actMan = client.GetActivityManager();
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(string showTitle, string userActivity,
            Int64 currentTime, Int64 runtime)
        {

            activity.State = userActivity;
            activity.Details = showTitle;

            Int64 endTimeEpoch = (Int64)(((runtime * 100) - (currentTime * 100)) / 1000000000);
            Int64 currentTimeEpoch = (Int64)DateTime.UtcNow.Subtract(epochStart).TotalSeconds;


            activity.Timestamps.End = endTimeEpoch + currentTimeEpoch;

            actMan.UpdateActivity(activity, (res) =>
            {
            });
            
        }

        public void SetActivity()
        {

            activity.State = "";
            activity.Details = "Idle";


            activity.Timestamps.End = 0;

            actMan.UpdateActivity(activity, (res) =>
            {
            });

        }
    }
}
