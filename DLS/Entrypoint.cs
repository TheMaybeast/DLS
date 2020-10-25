﻿using DLS.Threads;
using DLS.Utils;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

[assembly: Plugin("Dynamic Lighting System", Description = "Better ELS but for default lighting", Author = "TheMaybeast", PrefersSingleInstance = true, ShouldTickInPauseMenu = true, SupportUrl = "https://discord.gg/HUcXxkq")]
namespace DLS
{
    internal class Entrypoint
    {
        //List of Configured DLSModels by .xml files
        public static Dictionary<Model, DLSModel> DLSModelsDict = new Dictionary<Model, DLSModel>();
        //Vehicles currently being managed by DLS
        public static List<ActiveVehicle> activeVehicles = new List<ActiveVehicle>();
        //List of All TAgroups
        public static Dictionary<string, TAgroup> tagroups = new Dictionary<string, TAgroup>();
        //Pool of Available ELs
        public static List<EmergencyLighting> AvailablePool = new List<EmergencyLighting>();
        //Pool of Used ELs
        public static Dictionary<uint, EmergencyLighting> UsedPool = new Dictionary<uint, EmergencyLighting>();
        //List of used Sound IDs
        public static List<int> UsedSoundIDs = new List<int>();

        //If DLS is on Key Lock method
        public static bool keysLocked = false;

        public static void Main()
        {
            //Initiates Log File
            Log Log = new Log();

            // Checks if .ini file is created.
            Settings.IniCheck();

            //Version check and logging.
            FileVersionInfo rphVer = FileVersionInfo.GetVersionInfo("ragepluginhook.exe");
            Game.LogTrivial("Detected RPH " + rphVer.FileVersion);
            if (rphVer.FileMinorPart < 78)
            {
                Game.LogTrivial("RPH 78+ is required to use this mod");
                "ERROR: RPH 78+ is required but not found".ToLog();
                Game.DisplayNotification($"~y~Unable to load DLS~w~\nRagePluginHook version ~b~78~w~ or later is required, you are on version ~b~{rphVer.FileMinorPart}");
                return;
            }
            AssemblyName pluginInfo = Assembly.GetExecutingAssembly().GetName();
            Game.LogTrivial($"LOADED DLS v{pluginInfo.Version}");

            //Load DLS Models
            "Loading: DLS Vehicle Configurations".ToLog();
            DLSModelsDict = Vehicles.GetAllModels();
            "Loaded: DLS Vehicle Configurations".ToLog();

            //Load TAgroups
            "Loading: TAgroups".ToLog();
            tagroups = Vehicles.GetAllTAgroups();
            "Loaded: TAgroups".ToLog();

            //Loads MPDATA audio
            NativeFunction.Natives.SET_AUDIO_FLAG("LoadMPData", true);

            //Creates player controller
            "Loading: DLS - Player Controller".ToLog();
            GameFiber.StartNew(delegate { PlayerController.Process(); }, "DLS - Player Controller");
            "Loaded: DLS - Player Controller".ToLog();

            //Creates special modes managers
            "Loading: DLS - Special Modes Managers".ToLog();
            if(Settings.SET_AILC)
                GameFiber.StartNew(delegate { SpecialModesManager.ProcessAI(); }, "DLS - Special Modes AI Manager");
            GameFiber.StartNew(delegate { SpecialModesManager.ProcessPlayer(); }, "DLS - Special Modes Player Manager");
            "Loaded: DLS - Special Modes Managers".ToLog();

            //Creates cleanup manager
            "Loading: DLS - Cleanup Manager".ToLog();
            GameFiber.StartNew(delegate { Threads.CleanupManager.Process(); }, "DLS - Cleanup Manager");
            "Loaded: DLS - Cleanup Manager".ToLog();

            if (Settings.UI_ENABLED)
                UIManager.Process();

            if (Settings.SET_PATCHEXTRAS)
            {
                bool patched = ExtraRepairPatch.DisableExtraRepair();
                if (patched) "Patched extra repair".ToLog();
                else "ERROR: Failed to patch extra repair".ToLog();
            }
        }

        private static void OnUnload(bool isTerminating)
        {
            if (UsedSoundIDs.Count > 0)
            {
                "Unloading used SoundIDs".ToLog();
                foreach (int id in UsedSoundIDs)
                {
                    NativeFunction.Natives.STOP_SOUND(id);
                    NativeFunction.Natives.RELEASE_SOUND_ID(id);
                    ("Unloaded SoundID " + id).ToLog();
                }
                "Unloaded all used SoundIDs".ToLog();
            }
            if (activeVehicles.Count > 0)
            {
                "Refreshing vehicle's default EL".ToLog();
                foreach (ActiveVehicle aVeh in activeVehicles)
                {
                    if (aVeh.Vehicle)
                    {
                        aVeh.Vehicle.EmergencyLightingOverride = aVeh.DefaultEL;
                        aVeh.Vehicle.IsSirenSilent = aVeh.IsSirenSilent;
                        aVeh.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                        NativeFunction.Natives.SET_VEHICLE_RADIO_ENABLED(aVeh.Vehicle, true);
                        Lights.ResetExtras(aVeh);
                        ("Refreshed " + aVeh.Vehicle.Handle).ToLog();
                    }
                    else
                        ("Vehicle does not exist anymore!").ToLog();
                }
                "Refreshed vehicle's default EL".ToLog();
            }
        }

        [ConsoleCommand]
        private static void Command_GetStagingInfo()
        {
            Vehicle veh = Game.LocalPlayer.Character.CurrentVehicle;

            if (veh && veh.GetActiveVehicle() != null)
            {
                ActiveVehicle aVeh = veh.GetActiveVehicle();
                Game.LogTrivial("Siren: " + aVeh.SirenStage.ToString());
                Game.LogTrivial("Light: " + aVeh.LightStage.ToString());
                Game.LogTrivial("TAStage: " + aVeh.TAStage.ToString());
                Game.LogTrivial("SBOn: " + aVeh.SBOn.ToString());
                Game.LogTrivial("TAstage: " + aVeh.TAgroup.TaPatterns[aVeh.TApatternCurrentIndex].Name);
                Game.LogTrivial("ELName: " + veh.EmergencyLightingOverride.Name);
            }
        }
    }
}