using System.Collections.Generic;
using System.Diagnostics;
using dotenv.net;

namespace JellyPresence.Project
{
    public class JellyPresence
    {

        private readonly IDictionary<string, string> envDict;
        private string jmpPath;

        public JellyPresence()
        {
            envDict = DotEnv.Read();
        }

        public static void StartJMP() 
        {
            Process process = new Process();
            process.StartInfo.FileName = "D:\\Jellyfin\\JellyfinMediaPlayer.exe";
            process.Start();
        }

    }
}
