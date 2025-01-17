using System;
using System.Collections.Generic;
using dotenv.net;

namespace JellyPresence.Project
{
    /*
     * Only holds functions for env related stuff
     * until i come across things that might be needed lol
     */

    public class Config
    {
        private readonly IDictionary<string, string> envDict;
        public Config() 
        {
            envDict = DotEnv.Read();
            if (!envIntegrity(envDict))
            {
                throw new InvalidENVException("ENV Invalid");
            }
        }

        /* 
         * Ensure all needed fields are filled 
         */
        private static bool envIntegrity(IDictionary<string, string> _env)
        {
            return true;
        }

        public string GetVal(string key)
        {
            try
            {
                return envDict[key];
            }
            catch
            {
                Console.WriteLine("Could not return value. Check to ensure key is correct");
                return null;
            }
        }

        public class InvalidENVException : Exception
        {
            public InvalidENVException() { }
            public InvalidENVException(string message)
                : base(message)
            { }
            public InvalidENVException(string message, Exception inner)
                : base(message, inner)
            { }
        }
    }
}
