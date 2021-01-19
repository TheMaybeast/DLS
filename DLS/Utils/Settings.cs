using Rage;
using System.Windows.Forms;

namespace DLS.Utils
{
    class Settings
    {
        internal static InitializationFile INI = new InitializationFile(@"Plugins\DLS.ini");

        // Controls
        public static int CON_INDLEFT { get; } = INI.ReadInt32("Controls", "CON_INDLEFT", 100);
        public static int CON_INDRIGHT { get; } = INI.ReadInt32("Controls", "CON_INDRIGHT", 42);
        public static int CON_HZRD { get; } = INI.ReadInt32("Controls", "CON_HZRD", 83);
        public static int CON_NEXTLIGHTS { get; } = INI.ReadInt32("Controls", "CON_NEXTLIGHTS", 85);
        public static int CON_PREVLIGHTS { get; } = INI.ReadInt32("Controls", "CON_PREVLIGHTS", 157);
        public static int CON_TOGGLESIREN { get; } = INI.ReadInt32("Controls", "CON_TOGGLESIREN", 19);
        public static int CON_NEXTSIREN { get; } = INI.ReadInt32("Controls", "CON_NEXTSIREN", 80);
        public static int CON_PREVSIREN { get; } = INI.ReadInt32("Controls", "CON_PREVSIREN", 73);
        public static int CON_AUXSIREN { get; } = INI.ReadInt32("Controls", "CON_AUXSIREN", 27);
        public static int CON_HORN { get; } = INI.ReadInt32("Controls", "CON_HORN", 86);
        public static int CON_TA { get; } = INI.ReadInt32("Controls", "CON_TA", 245);

        // Settings
        public static bool SET_SCNDLS { get; } = INI.ReadBoolean("Settings", "SirenControlNonDLS", true);
        public static bool SET_AILC { get; } = INI.ReadBoolean("Settings", "AILightsControl", true);
        public static bool SET_INDENABLED { get; } = INI.ReadBoolean("Settings", "IndEnabled", true);
        public static bool SET_BRAKELIGHT { get; } = INI.ReadBoolean("Settings", "BrakeLightsEnabled", true);        
        public static bool SET_SIRENKILL { get; } = INI.ReadBoolean("Settings", "SirenKill", true);
        public static bool SET_LOGTOCONSOLE { get; } = INI.ReadBoolean("Settings", "LogToConsole", false);
        public static bool SET_PATCHEXTRAS { get; } = INI.ReadBoolean("Settings", "PatchExtras", true);
        public static string SET_AUDIONAME { get; } = INI.ReadString("Settings", "AudioName", "TOGGLE_ON");
        public static string SET_AUDIOREF { get; } = INI.ReadString("Settings", "AudioRef", "HUD_FRONTEND_DEFAULT_SOUNDSET");

        // UI
        public static bool UI_ENABLED { get; } = INI.ReadBoolean("UI", "UIEnabled", true);
        public static Keys UI_TOGGLE { get; } = INI.ReadEnum("Keyboard", "UIKey", Keys.E);
        public static int UI_WIDTH { get; } = INI.ReadInt32("UI", "Width", 550);
        public static int UI_HEIGHT { get; } = INI.ReadInt32("UI", "Height", 220);
        public static int UI_OFFSETX { get; } = INI.ReadInt32("UI", "OffsetX", 1920);
        public static int UI_OFFSETY { get; } = INI.ReadInt32("UI", "OffsetY", 1080);

        internal static void IniCheck()
        {
            if (INI.Exists())
            {
                "Loaded: DLS.ini".ToLog();
                return;
            }
            "ERROR: DLS.ini was not found!".ToLog();
        }
    }
}