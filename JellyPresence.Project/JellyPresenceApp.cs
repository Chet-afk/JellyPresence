using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyPresence.Project
{
    public class JellyPresenceApp
    {
        public static void Main()
        {
            JellyPresence j = new JellyPresence();
            JellyPresence.StartJMP();
            j.StartDiscordUpdate();
            while (true) ;
        }
    }
}
