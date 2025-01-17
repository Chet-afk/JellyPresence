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


        public DiscordManager()
        {
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(Discord.Discord client, string showTitle, string userActivity,
            long currentTime, long runtime)
        {

            ActivityManager actMan = client.GetActivityManager();

            Activity activity = new Activity();
            activity.State = userActivity;
            activity.Details = showTitle;

            //long endTimeEpoch = (runtime - currentTime) / 10;
            //long currentTimeEpoch = (long)DateTime.UtcNow.Subtract(epochStart).TotalSeconds;

            //activityTimestamps.End = endTimeEpoch + currentTimeEpoch;

            //activity.Timestamps = activityTimestamps;



            actMan.UpdateActivity(activity, (res) =>
            {
            });
            
        }
    }
}
