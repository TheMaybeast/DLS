using Rage.Native;

namespace DLS.Utils
{
    class Sound
    {
        public static int NewSoundID(ActiveVehicle activeVeh)
        {
            if (activeVeh.SoundId != 999)
            {
                NativeFunction.Natives.STOP_SOUND(activeVeh.SoundId);
                NativeFunction.Natives.RELEASE_SOUND_ID(activeVeh.SoundId);
                Entrypoint.UsedSoundIDs.Remove(activeVeh.SoundId);
#if DEBUG
                ("Deallocated SoundID " + activeVeh.SoundId).ToLog();
#endif
            }
            int newID = NativeFunction.Natives.GET_SOUND_ID<int>();
            activeVeh.SoundId = newID;
            Entrypoint.UsedSoundIDs.Add(newID);
#if DEBUG
            ("Allocated SoundID " + newID).ToLog();
#endif
            return newID;
        }

        public static int TempSoundID()
        {
            int newID = NativeFunction.Natives.GET_SOUND_ID<int>();
            Entrypoint.UsedSoundIDs.Add(newID);
#if DEBUG
            ("Allocated TempSoundID " + newID).ToLog();
#endif
            return newID;
        }

        public static void ClearTempSoundID(int id)
        {
            NativeFunction.Natives.STOP_SOUND(id);
            NativeFunction.Natives.RELEASE_SOUND_ID(id);
            Entrypoint.UsedSoundIDs.Remove(id);
#if DEBUG
            ("Deallocated SoundID " + id).ToLog();
#endif
        }
    }
}
