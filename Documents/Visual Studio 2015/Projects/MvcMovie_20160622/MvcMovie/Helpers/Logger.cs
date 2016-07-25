using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMovie.Helpers
{
    using System;
    using System.Diagnostics;
    public class Logger             
    {
        private static Logger logger;

        public static readonly string CRITICAL = "CRITICAL";
        public static readonly string ERROR = "ERROR";
        public static readonly string WARNING = "WARNING";
        public static readonly string INFO = "INFO";
        public static readonly string DEBUG = "DEBUG";

        private Logger() { }

        public static Logger Get()
        {

                if (logger == null)
                {
                    logger = new Logger();
                }
                return logger;
 
        }

        public void Log(string level, string message, Exception e)
        {
            Debug.WriteLine(level + ":  " + message);
            
            if (e != null)
            {
                Debug.WriteLine("Exception:  " + e.ToString());
            }
            
        }
    }
}