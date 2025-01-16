using System;
using System.Timers;
using Discord;

namespace JellyPresence.Project
{
    public class DiscordManager
    {
        private Discord.Discord client;
        private ActivityManager actManager;
        private Activity activity = new Discord.Activity();
        private ActivityTimestamps activityTimestamps = new ActivityTimestamps();
        private ActivityAssets assets = new ActivityAssets();


        public DiscordManager(long ClientID)
        {
            // maybe change to json so dont gotta convert to long lol
            client = new Discord.Discord(ClientID, (UInt64)Discord.CreateFlags.Default);
            actManager = client.GetActivityManager();
            actManager.UpdateActivity(activity, (res) =>
            {
            });

            StartDiscordUpdate();
        }

        //TODO: Add image changing (once imgur api implemented?)
        public void SetActivity(string showTitle, string userActivity,
            ActivityTimestamps timeRemaining)
        {
            activity.State = userActivity;
            activity.Details = showTitle;

        }


        private void StartDiscordUpdate()
        {
            var timer = new Timer(500);
            timer.Elapsed += UpdateEvent;
            timer.AutoReset = true;
            timer.Start();
        }

        private void UpdateEvent(Object s, ElapsedEventArgs e)
        {
            // Query jellyfin for data
            client.RunCallbacks();
        }
    }
}
