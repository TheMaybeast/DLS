using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DLS.Utils
{
    internal class Controls
    {
        private static Dictionary<DLSControls, Keys> listKeys = new Dictionary<DLSControls, Keys>();
        private static string ModKey = "Shift";
        private static List<int> DisabledControls = new List<int>();

        public static bool IsDLSControlDown(DLSControls controls)
        {
            switch (controls)
            {
                case DLSControls.SIREN_TOGGLE:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_TOGGLE])
                        || Game.IsControllerButtonDown(ControllerButtons.DPadDown);
                case DLSControls.SIREN_TONE1:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_TONE1]);
                case DLSControls.SIREN_TONE2:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_TONE2]);
                case DLSControls.SIREN_TONE3:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_TONE3]);
                case DLSControls.SIREN_TONE4:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_TONE4]);
                case DLSControls.SIREN_SCAN:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_SCAN]);
                case DLSControls.SIREN_HORN:
                    return Game.IsKeyDownRightNow(listKeys[DLSControls.SIREN_HORN])
                        || Game.IsControllerButtonDownRightNow(ControllerButtons.LeftThumb)
                        || Game.IsControlPressed(2, GameControl.VehicleHorn);
                case DLSControls.SIREN_AUX:
                    return Game.IsKeyDown(listKeys[DLSControls.SIREN_AUX])
                        || Game.IsControllerButtonDown(ControllerButtons.DPadUp);
                case DLSControls.SIREN_MAN:
                    return Game.IsKeyDownRightNow(listKeys[DLSControls.SIREN_MAN])
                        || Game.IsControllerButtonDownRightNow(ControllerButtons.B);
                case DLSControls.LIGHT_TOGGLE:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_TOGGLE])
                        || Game.IsControllerButtonDown(ControllerButtons.DPadLeft);
                case DLSControls.LIGHT_TADVISOR:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_TADVISOR]);
                case DLSControls.GEN_LOCKALL:
                    return Game.IsKeyDown(listKeys[DLSControls.GEN_LOCKALL]);
                case DLSControls.LIGHT_SBURN:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_SBURN]);
                case DLSControls.LIGHT_INTLT:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_INTLT]);
                case DLSControls.LIGHT_INDL:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_INDL]);
                case DLSControls.LIGHT_INDR:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_INDR]);
                case DLSControls.LIGHT_HAZRD:
                    return Game.IsKeyDown(listKeys[DLSControls.LIGHT_HAZRD]);
                default:
                    return false;
            }
        }

        public static bool IsDLSControlDownWithModifier(DLSControls controls)
        {
            switch (controls)
            {
                case DLSControls.LIGHT_TADVISOR:
                    switch (ModKey)
                    {
                        case "Shift":
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_TADVISOR]);
                        case "Control":
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_TADVISOR]);
                        case "Alt":
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_TADVISOR]);
                        default:
                            return false;
                    }
                case DLSControls.UI_TOGGLE:
                    switch (ModKey)
                    {
                        case "Shift":
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.UI_TOGGLE]);
                        case "Control":
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.UI_TOGGLE]);
                        case "Alt":
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.UI_TOGGLE]);
                        default:
                            return false;
                    }
                case DLSControls.LIGHT_SBURN:
                    switch (ModKey)
                    {
                        case "Shift":
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_SBURN]);
                        case "Control":
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_SBURN]);
                        case "Alt":
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(listKeys[DLSControls.LIGHT_SBURN]);
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public static void DisableControls()
        {
            foreach (int i in DisabledControls)
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, i, true);
        }

        public static void RefreshKeys()
        {
            listKeys.Clear();
            try
            {
                listKeys.Add(DLSControls.SIREN_TOGGLE, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "SirenToggle")));
                listKeys.Add(DLSControls.SIREN_TONE1, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Tone1")));
                listKeys.Add(DLSControls.SIREN_TONE2, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Tone2")));
                listKeys.Add(DLSControls.SIREN_TONE3, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Tone3")));
                listKeys.Add(DLSControls.SIREN_TONE4, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Tone4")));
                listKeys.Add(DLSControls.SIREN_SCAN, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Scan")));
                listKeys.Add(DLSControls.SIREN_HORN, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Horn")));
                listKeys.Add(DLSControls.SIREN_AUX, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "AuxToggle")));
                listKeys.Add(DLSControls.SIREN_MAN, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Manual")));

                listKeys.Add(DLSControls.LIGHT_TOGGLE, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "LightStage")));
                listKeys.Add(DLSControls.LIGHT_TADVISOR, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "TAdvisor")));
                listKeys.Add(DLSControls.LIGHT_SBURN, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "SteadyBurn")));
                listKeys.Add(DLSControls.LIGHT_INTLT, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "InteriorLT")));
                listKeys.Add(DLSControls.LIGHT_INDL, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "IndL")));
                listKeys.Add(DLSControls.LIGHT_INDR, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "IndR")));
                listKeys.Add(DLSControls.LIGHT_HAZRD, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "Hazard")));

                listKeys.Add(DLSControls.GEN_LOCKALL, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "LockAll")));

                listKeys.Add(DLSControls.UI_TOGGLE, (Keys)Enum.Parse(typeof(Keys), Settings.ReadKey("Keyboard", "UIKey")));
                ModKey = Settings.ReadKey("Keyboard", "Modifier");

                string[] disabledControls = Settings.ReadKey("Keyboard", "Disabled").Replace(" ", "").Trim().Split(',');
                foreach (string control in disabledControls)
                    DisabledControls.Add(control.ToInt32());
            }
            catch (Exception e)
            {
                ("Key Parse ERROR: " + e.Message).ToLog();
                Game.LogTrivial("Key Parse ERROR: " + e.Message);
            }

        }
    }

    internal enum DLSControls
    {
        SIREN_TOGGLE,
        SIREN_TONE1,
        SIREN_TONE2,
        SIREN_TONE3,
        SIREN_TONE4,
        SIREN_SCAN,
        SIREN_HORN,
        SIREN_AUX,
        SIREN_MAN,
        LIGHT_TOGGLE,
        LIGHT_TADVISOR,
        LIGHT_SBURN,
        LIGHT_INTLT,
        LIGHT_INDL,
        LIGHT_INDR,
        LIGHT_HAZRD,
        GEN_LOCKALL,
        UI_TOGGLE
    }
}