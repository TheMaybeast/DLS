using Rage;
using Rage.Native;
using System.Collections.Generic;

namespace DLS.Utils
{
    internal static class Sirens
    {
        private static List<SirenStage> defaultSirenStages = new List<SirenStage> { SirenStage.One, SirenStage.Two, SirenStage.Warning, SirenStage.Warning2 };

        public static void Update(ActiveVehicle activeVeh, bool dls = true)
        {
            switch (activeVeh.SirenStage)
            {
                case SirenStage.Off:
                    Sound.NewSoundID(activeVeh);
                    break;
                case SirenStage.One:
                    if (dls)
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), activeVeh.Vehicle.GetDLS().SoundSettings.Tone1, activeVeh.Vehicle, 0, 0, 0);
                    else
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), "VEHICLES_HORNS_SIREN_1", activeVeh.Vehicle, 0, 0, 0);
                    break;
                case SirenStage.Two:
                    if (dls)
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), activeVeh.Vehicle.GetDLS().SoundSettings.Tone2, activeVeh.Vehicle, 0, 0, 0);
                    else
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), "VEHICLES_HORNS_SIREN_2", activeVeh.Vehicle, 0, 0, 0);
                    break;
                case SirenStage.Warning:
                    if (dls)
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), activeVeh.Vehicle.GetDLS().SoundSettings.Tone3, activeVeh.Vehicle, 0, 0, 0);
                    else
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), "VEHICLES_HORNS_POLICE_WARNING", activeVeh.Vehicle, 0, 0, 0);
                    break;
                case SirenStage.Warning2:
                    if (dls)
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), activeVeh.Vehicle.GetDLS().SoundSettings.Tone4, activeVeh.Vehicle, 0, 0, 0);
                    else
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), "VEHICLES_HORNS_AMBULANCE_WARNING", activeVeh.Vehicle, 0, 0, 0);
                    break;
                case SirenStage.Horn:
                    if (dls)
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), activeVeh.Vehicle.GetDLS().SoundSettings.Horn, activeVeh.Vehicle, 0, 0, 0);
                    else
                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(Sound.NewSoundID(activeVeh), "SIRENS_AIRHORN", activeVeh.Vehicle, 0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        public static void MoveUpStage(ActiveVehicle activeVeh, bool isDLS = false, DLSModel dlsModel = null, bool isMan = false)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
            if (isDLS)
                activeVeh.SirenStage = dlsModel.AvailableSirenStages.Next(activeVeh.SirenStage);
            else
                activeVeh.SirenStage = defaultSirenStages.Next(activeVeh.SirenStage);
            Update(activeVeh, isDLS);
        }

        public static void MoveDownStage(ActiveVehicle activeVeh, bool isDLS = false, DLSModel dlsModel = null, bool isMan = false)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
            if (isDLS)
                activeVeh.SirenStage = dlsModel.AvailableSirenStages.Previous(activeVeh.SirenStage);
            else
                activeVeh.SirenStage = defaultSirenStages.Previous(activeVeh.SirenStage);
            Update(activeVeh, isDLS);
        }

        public static void SetAirManuState(ActiveVehicle activeVeh, bool isDLS, int? newState)
        {
            if(newState != activeVeh.AirManuState)
            {
                if(activeVeh.AirManuID != null)
                {
                    Sound.ClearTempSoundID((int)activeVeh.AirManuID);
                    activeVeh.AirManuID = null;
                }

                switch (newState)
                {
                    case 1:
                        activeVeh.AirManuID = Sound.TempSoundID();
                        if (isDLS)
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, activeVeh.Vehicle.GetDLS().SoundSettings.Horn, activeVeh.Vehicle, 0, 0, 0);
                        else
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, "SIRENS_AIRHORN", activeVeh.Vehicle, 0, 0, 0);
                        break;
                    case 2:
                        activeVeh.AirManuID = Sound.TempSoundID();
                        if (isDLS)
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, activeVeh.Vehicle.GetDLS().SoundSettings.Tone1, activeVeh.Vehicle, 0, 0, 0);
                        else
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, "VEHICLES_HORNS_SIREN_1", activeVeh.Vehicle, 0, 0, 0);
                        break;
                    case 3:
                        activeVeh.AirManuID = Sound.TempSoundID();
                        if (isDLS)
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, activeVeh.Vehicle.GetDLS().SoundSettings.Tone2, activeVeh.Vehicle, 0, 0, 0);
                        else
                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AirManuID, "VEHICLES_HORNS_SIREN_2", activeVeh.Vehicle, 0, 0, 0);
                        break;
                }

                activeVeh.AirManuState = newState;
            }
        }
    }
}