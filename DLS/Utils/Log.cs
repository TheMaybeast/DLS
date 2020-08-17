using System;
using System.IO;
using System.Reflection;

namespace DLS.Utils
{
    class Log
    {
        public Log()
        {
            string message = "DLS - Dynamic Lighting System - v" + Assembly.GetExecutingAssembly().GetName().Version;
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = @"Plugins/DLS.log";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}
