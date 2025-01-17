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
        private ActivityTimestamps activityTimestamps = new ActivityTimestamps();

        public DiscordManager(Discord.Discord client)
        {
            this.actMan = client.GetActivityManager();
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(string showTitle, string userActivity,
            long currentTime, long runtime)
        {

            activity.State = userActivity;
            activity.Details = showTitle;

            long endTimeEpoch = (runtime - currentTime) / 10;
            long currentTimeEpoch = (long)DateTime.UtcNow.Subtract(epochStart).TotalSeconds;

            activityTimestamps.End = endTimeEpoch + currentTimeEpoch;

            activity.Timestamps = activityTimestamps;

            Console.WriteLine(activity.Timestamps.End);

            actMan.UpdateActivity(activity, (res) =>
            {
            });
            
        }
    }
}
