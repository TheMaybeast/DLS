using Rage.Native;

namespace DLS.Utils
{
    internal static class Sirens
    {
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

        public static void MoveUpStage(ActiveVehicle activeVeh, bool isDLS = false, bool isMan = false)
        {
            if (isDLS)
            {
                activeVeh.SirenStage = GetNextStage(activeVeh.SirenStage);
            }
            else
            {
                switch (activeVeh.SirenStage)
                {
                    case SirenStage.Off:
                        activeVeh.SirenStage = SirenStage.One;
                        break;
                    case SirenStage.One:
                        activeVeh.SirenStage = SirenStage.Two;
                        break;
                    case SirenStage.Two:
                        activeVeh.SirenStage = SirenStage.Warning;
                        break;
                    case SirenStage.Warning:
                        activeVeh.SirenStage = SirenStage.Warning2;
                        break;
                    case SirenStage.Warning2:
                        activeVeh.SirenStage = SirenStage.Off;
                        break;
                }
            }
            Update(activeVeh, isDLS);
        }

        public static SirenStage GetNextStage(SirenStage sirenStage)
        {
            if (sirenStage != SirenStage.Warning2)
            {
                return sirenStage + 1;
            }
            else
            {
                return SirenStage.One;
            }
        }
        public static SirenStage GetNextStage(SirenStage sirenStage, DLSModel vehDLS)
        {
            return vehDLS.AvailableSirenStages.NextSirenStage(sirenStage, false);
        }
    }
}