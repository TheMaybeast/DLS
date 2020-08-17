using Rage;
using System;
using System.Globalization;
using System.IO;

namespace DLS.Utils
{
    class Settings
    {
        private static string _location = @"Plugins\DLS.ini";
        public static void IniCheck()
        {
            if (File.Exists(_location)) return; "Loaded: DLS.ini".ToLog();
            "ERROR: DLS.ini was not found!".ToLog();
        }

        public static InitializationFile InitializeIni()
        {
            var ini = new InitializationFile(_location, CultureInfo.InvariantCulture, false);
            ini.Create();
            return ini;
        }

        public static string ReadKey(string sectionName, string keyName)
        {
            var value = "";
            try
            {
                var ini = InitializeIni();
                value = ini.ReadString(sectionName, keyName);
            }
            catch (Exception e)
            {
                ("DLS.ini ERROR: " + e.Message).ToLog();
                Game.LogTrivial("DLS.ini ERROR: " + e.Message);
            }
            return value;
        }
    }
}
