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
            if (!envIntegrity())
            {
                throw new InvalidENVException("ENV Invalid");
            }
        }

        /* 
         * Ensure all needed fields are filled 
         */
        private bool envIntegrity()
        {
            try
            {
                string a = envDict["CLIENTID"];
                string b = envDict["JELLYAPIKEY"];
                string c = envDict["JELLYURL"];
            }
            catch (KeyNotFoundException e)
            {
                // Write stuff to a log eventually
                return false;
            }
            return true;
        }

        public string GetVal(string key)
        {
            try
            {
                return envDict[key];
            }
            catch (KeyNotFoundException e) 
            {
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
