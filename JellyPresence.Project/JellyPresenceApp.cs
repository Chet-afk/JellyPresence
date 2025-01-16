
using System;
using System.Diagnostics;
using System.Timers;

namespace JellyPresence.Project
{
    public class JellyPresenceApp
    {
        bool instanceOpen = true;

        public static void Main()
        {
            JellyPresence j = new JellyPresence();
            JellyPresence.StartJMP();

            Timer t = new Timer(2000);
            t.Elapsed += UpdateEvent;
            t.AutoReset = true;
            t.Start();

            // Cant just check for JMP instances in the loop
            // Discord will cause break by accessing protected memory 
            while (true);
        }

        private static void UpdateEvent(Object s, ElapsedEventArgs e)
        {
            if (Process.GetProcessesByName("JellyfinMediaPlayer").Length == 0)
            {
                Environment.Exit(0);
            }
        }
    }
}
