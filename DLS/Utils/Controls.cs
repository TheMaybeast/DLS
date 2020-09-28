using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DLS.Utils
{
    internal class Controls
    {
        private static List<int> DisabledControls = new List<int>();

        static Controls()
        {
            string[] disabledControls = Settings.GEN_DISABLEDS.Replace(" ", "").Trim().Split(',');
            foreach (string control in disabledControls)
                DisabledControls.Add(control.ToInt32());
        }

        public static bool IsDLSControlDown(DLSControls controls)
        {
            switch (controls)
            {
                case DLSControls.SIREN_TOGGLE:
                    return Game.IsKeyDown(Settings.SIREN_TOGGLE)
                        || Game.IsControllerButtonDown(ControllerButtons.DPadDown);
                case DLSControls.SIREN_TONE1:
                    return Game.IsKeyDown(Settings.SIREN_TONE1);
                case DLSControls.SIREN_TONE2:
                    return Game.IsKeyDown(Settings.SIREN_TONE2);
                case DLSControls.SIREN_TONE3:
                    return Game.IsKeyDown(Settings.SIREN_TONE3);
                case DLSControls.SIREN_TONE4:
                    return Game.IsKeyDown(Settings.SIREN_TONE4);
                case DLSControls.SIREN_SCAN:
                    return Game.IsKeyDown(Settings.SIREN_SCAN);
                case DLSControls.SIREN_HORN:
                    return Game.IsKeyDownRightNow(Settings.SIREN_HORN)
                        || Game.IsControllerButtonDownRightNow(ControllerButtons.LeftThumb)
                        || Game.IsControlPressed(2, GameControl.VehicleHorn);
                case DLSControls.SIREN_AUX:
                    return Game.IsKeyDown(Settings.SIREN_AUX)
                        || Game.IsControllerButtonDown(ControllerButtons.DPadUp);
                case DLSControls.SIREN_MAN:
                    return Game.IsKeyDownRightNow(Settings.SIREN_MAN)
                        || Game.IsControllerButtonDownRightNow(ControllerButtons.B);
                case DLSControls.LIGHT_TOGGLE:
                    return Game.IsKeyDown(Settings.LIGHT_TOGGLE)
                        || Game.IsControllerButtonDown(ControllerButtons.DPadLeft);
                case DLSControls.LIGHT_TADVISOR:
                    return Game.IsKeyDown(Settings.LIGHT_TADVISOR);
                case DLSControls.GEN_LOCKALL:
                    return Game.IsKeyDown(Settings.GEN_LOCKALL);
                case DLSControls.LIGHT_SBURN:
                    return Game.IsKeyDown(Settings.LIGHT_SBURN);
                case DLSControls.LIGHT_INTLT:
                    return Game.IsKeyDown(Settings.LIGHT_INTLT);
                case DLSControls.LIGHT_INDL:
                    return Game.IsKeyDown(Settings.LIGHT_INDL);
                case DLSControls.LIGHT_INDR:
                    return Game.IsKeyDown(Settings.LIGHT_INDR);
                case DLSControls.LIGHT_HAZRD:
                    return Game.IsKeyDown(Settings.LIGHT_HAZRD);
                default:
                    return false;
            }
        }

        public static bool IsDLSControlDownWithModifier(DLSControls controls)
        {
            switch (controls)
            {
                case DLSControls.LIGHT_TADVISOR:
                    switch (Settings.GEN_MODIFIER)
                    {
                        case Keys.Shift:
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_TADVISOR);
                        case Keys.Control:
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_TADVISOR);
                        case Keys.Alt:
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_TADVISOR);
                        default:
                            return false;
                    }
                case DLSControls.UI_TOGGLE:
                    switch (Settings.GEN_MODIFIER)
                    {
                        case Keys.Shift:
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(Settings.UI_TOGGLE);
                        case Keys.Control:
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(Settings.UI_TOGGLE);
                        case Keys.Alt:
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(Settings.UI_TOGGLE);
                        default:
                            return false;
                    }
                case DLSControls.LIGHT_SBURN:
                    switch (Settings.GEN_MODIFIER)
                    {
                        case Keys.Shift:
                            return Game.IsShiftKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_SBURN);
                        case Keys.Control:
                            return Game.IsControlKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_SBURN);
                        case Keys.Alt:
                            return Game.IsAltKeyDownRightNow
                                && Game.IsKeyDown(Settings.LIGHT_SBURN);
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