using DLS.Utils;
using Rage;
using Rage.Native;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DLS.Threads
{
    class PlayerController
    {
        private static SirenStage sirenStageBeforeHorn = SirenStage.Off;
        private static bool manActive = false;
        internal static bool manButtonDown = false;
        internal static bool hornButtonDown = false;
        internal static bool blktOn = false;
        private static Vehicle lastVehicle;

        private static Stopwatch scanTimer = new Stopwatch();
        private static Random random = new Random();
        private static int scanTiming;

        public static void Process()
        {
            while (true)
            {
                GameFiber.Yield();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    Vehicle veh = Game.LocalPlayer.Character.CurrentVehicle;
                    while (veh && veh.IsEngineOn && veh.Driver == Game.LocalPlayer.Character
                        && !Game.IsPaused && veh.HasSiren)
                    {
                        GameFiber.Yield();
                        bool tone1ButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_TONE1);
                        bool tone2ButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_TONE2);
                        bool tone3ButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_TONE3);
                        bool tone4ButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_TONE4);
                        manButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_MAN);
                        hornButtonDown = Controls.IsDLSControlDown(DLSControls.SIREN_HORN);

                        if (veh.GetActiveVehicle() == null)
                        {
                            if (veh.IsSirenOn)
                            {
                                if (!veh.IsSirenSilent)
                                    Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, true, LightStage.Three, SirenStage.One));
                                else
                                    Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, true, LightStage.Three, SirenStage.Off));
                            }
                            else
                                Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, true));
                        }
                        if (Controls.IsDLSControlDown(DLSControls.GEN_LOCKALL))
                        {
                            Entrypoint.keysLocked = !Entrypoint.keysLocked;
                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "TOGGLE_ON", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                        }
                        if (!blktOn && Entrypoint.BLightsEnabled && NativeFunction.Natives.IS_VEHICLE_STOPPED<bool>(veh))
                            NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(veh, true);
                        if (!Entrypoint.keysLocked)
                        {                            
                            NativeFunction.Natives.SET_VEHICLE_RADIO_ENABLED(veh, false);
                            ActiveVehicle activeVeh = veh.GetActiveVehicle();
                            DLSModel vehDLS;
                            if (veh)
                                vehDLS = veh.GetDLS();
                            else
                                vehDLS = null;
                            bool ahrnInterruptsSiren = vehDLS != null ? vehDLS.SoundSettings.AirHornInterruptsSiren.ToBoolean() : false;
                            if (Controls.IsDLSControlDownWithModifier(DLSControls.UI_TOGGLE))
                                UIManager.IsUIOn = !UIManager.IsUIOn;
                            if (vehDLS != null)
                            {
                                Controls.DisableControls();
                                if (lastVehicle != veh)
                                {
                                    lastVehicle = veh;
                                    if (vehDLS.SpecialModes.SirenUI != "")
                                        UI.Importer.GetCustomSprites(vehDLS.SpecialModes.SirenUI);
                                    else
                                    {
                                        UI.Importer.ResetSprites();
                                        UI.Importer.GetCustomSprites();
                                    }
                                }
                                else if (activeVeh.TempUsed
                                    && activeVeh.TempLightStage != activeVeh.LightStage)
                                {
                                    activeVeh.LightStage = activeVeh.TempLightStage;
                                    Lights.Update(activeVeh);
                                    activeVeh.TempUsed = false;
                                }
                                else if (activeVeh.TAType != "off"
                                    && Controls.IsDLSControlDownWithModifier(DLSControls.LIGHT_TADVISOR))
                                {
                                    if (activeVeh.TApatternCurrentIndex + 1 < activeVeh.TAgroup.TaPatterns.Count)
                                        activeVeh.TApatternCurrentIndex++;
                                    else
                                        activeVeh.TApatternCurrentIndex = 0;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                    Lights.UpdateTA(true, activeVeh);
                                    continue;
                                }
                                else if (Game.IsKeyDown(Keys.F) || Game.IsControllerButtonDown(ControllerButtons.Y))
                                {
                                    if (vehDLS.SpecialModes.PresetSirenOnLeaveVehicle != "none")
                                    {
                                        if (activeVeh.LightStage != LightStage.Off && activeVeh.LightStage != LightStage.Empty)
                                        {
                                            activeVeh.TempLightStage = activeVeh.LightStage;
                                            activeVeh.TempUsed = true;
                                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(veh, LeaveVehicleFlags.None);
                                            GameFiber.Sleep(1000);
                                            if (!veh.IsEngineOn)
                                                veh.IsEngineOn = true;
                                            string presetSiren = vehDLS.SpecialModes.PresetSirenOnLeaveVehicle;
                                            switch (presetSiren)
                                            {
                                                case "stage1":
                                                    activeVeh.LightStage = LightStage.One;
                                                    Lights.Update(activeVeh);
                                                    break;
                                                case "stage2":
                                                    activeVeh.LightStage = LightStage.Two;
                                                    Lights.Update(activeVeh);
                                                    break;
                                                case "stage3":
                                                    activeVeh.LightStage = LightStage.Three;
                                                    Lights.Update(activeVeh);
                                                    break;
                                                case "custom1":
                                                    activeVeh.LightStage = LightStage.CustomOne;
                                                    Lights.Update(activeVeh);
                                                    break;
                                                case "custom2":
                                                    activeVeh.LightStage = LightStage.CustomOne;
                                                    Lights.Update(activeVeh);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(veh, LeaveVehicleFlags.None);
                                        }
                                    }
                                    if (Vehicles.GetSirenKill(activeVeh) && activeVeh.SirenStage != SirenStage.Off)
                                    {
                                        activeVeh.SirenStage = SirenStage.Off;
                                        Utils.Sirens.Update(activeVeh);
                                    }
                                }
                                if (!Entrypoint.keysLocked)
                                {
                                    #region Lights Manager
                                    if (Controls.IsDLSControlDownWithModifier(DLSControls.LIGHT_SBURN))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        blktOn = !blktOn;
                                        if (blktOn)
                                        {
                                            activeVeh.IndStatus = IndStatus.Off;
                                            activeVeh.LightStage = LightStage.Off;
                                            activeVeh.SBOn = false;
                                            activeVeh.TAStage = TAStage.Off;
                                            if (activeVeh.IntLightOn)
                                                Lights.ToggleIntLight(activeVeh);
                                            Lights.Update(activeVeh);
                                            Lights.UpdateIndicator(activeVeh);
                                            NativeFunction.Natives.SET_VEHICLE_LIGHTS(veh, 1);
                                            continue;
                                        }
                                        else
                                        {
                                            NativeFunction.Natives.SET_VEHICLE_LIGHTS(veh, 0);
                                            continue;
                                        }
                                    }
                                    else if (blktOn)
                                        continue;
                                    else if (vehDLS.DoesVehicleHaveLightStage(LightStage.One)
                                        && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D1))
                                    {
                                        if (activeVeh.LightStage != LightStage.One)
                                            activeVeh.LightStage = LightStage.One;
                                        else
                                            activeVeh.LightStage = LightStage.Off;
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        Lights.Update(activeVeh);
                                    }
                                    else if (vehDLS.DoesVehicleHaveLightStage(LightStage.Two)
                                            && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D2))
                                    {
                                        if (activeVeh.LightStage != LightStage.Two)
                                            activeVeh.LightStage = LightStage.Two;
                                        else
                                            activeVeh.LightStage = LightStage.Off;
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        Lights.Update(activeVeh);
                                    }
                                    else if (vehDLS.DoesVehicleHaveLightStage(LightStage.Three)
                                            && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D3))
                                    {
                                        if (activeVeh.LightStage != LightStage.Three)
                                            activeVeh.LightStage = LightStage.Three;
                                        else
                                            activeVeh.LightStage = LightStage.Off;
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        Lights.Update(activeVeh);
                                    }
                                    else if (Controls.IsDLSControlDown(DLSControls.LIGHT_TOGGLE))
                                    {
                                        Lights.MoveUpStage(activeVeh);
                                    }
                                    else if (Controls.IsDLSControlDown(DLSControls.LIGHT_TADVISOR)
                                        && vehDLS.TrafficAdvisory.Type.ToLower() != "off")
                                    {
                                        if (activeVeh.LightStage == LightStage.Off)
                                        {
                                            activeVeh.LightStage = LightStage.Empty;
                                            veh.ShouldVehiclesYieldToThisVehicle = false;
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            if (activeVeh.SBOn)
                                                Lights.UpdateSB(activeVeh);
                                            veh.IsSirenOn = true;
                                            veh.IsSirenSilent = true;
                                            Lights.MoveUpStageTA(activeVeh);
                                            continue;
                                        }
                                        else if (activeVeh.LightStage == LightStage.Empty)
                                        {
                                            Lights.MoveUpStageTA(activeVeh);
                                            if (activeVeh.TAStage == TAStage.Off)
                                            {
                                                if (activeVeh.SBOn)
                                                {
                                                    veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                                    Lights.UpdateSB(activeVeh);
                                                    break;
                                                }
                                                activeVeh.LightStage = LightStage.Off;
                                                veh.ShouldVehiclesYieldToThisVehicle = true;
                                                veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                                veh.IsSirenOn = false;
                                            }
                                            Lights.UpdateSB(activeVeh);
                                            continue;
                                        }
                                        Lights.MoveUpStageTA(activeVeh);
                                    }
                                    else if (Controls.IsDLSControlDown(DLSControls.LIGHT_SBURN)
                                        && vehDLS.SpecialModes.SteadyBurn.SteadyBurnEnabled.ToBoolean())
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        activeVeh.SBOn = !activeVeh.SBOn;
                                        if (activeVeh.SBOn && activeVeh.LightStage == LightStage.Off)
                                        {
                                            activeVeh.LightStage = LightStage.Empty;
                                            veh.ShouldVehiclesYieldToThisVehicle = false;
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            if (activeVeh.TAStage != TAStage.Off)
                                                Lights.UpdateTA(true, activeVeh);
                                            veh.IsSirenOn = true;
                                            veh.IsSirenSilent = true;
                                        }
                                        else if (!activeVeh.SBOn && activeVeh.LightStage == LightStage.Empty)
                                        {
                                            if (activeVeh.TAStage != TAStage.Off)
                                            {
                                                veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                                Lights.UpdateTA(true, activeVeh);
                                                break;
                                            }
                                            activeVeh.LightStage = LightStage.Off;
                                            veh.ShouldVehiclesYieldToThisVehicle = true;
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            veh.IsSirenOn = false;
                                        }
                                        else if (activeVeh.SBOn && activeVeh.LightStage != LightStage.Off)
                                        {

                                        }
                                        Lights.UpdateSB(activeVeh);
                                    }
                                    else if (Controls.IsDLSControlDown(DLSControls.LIGHT_INTLT))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        Lights.ToggleIntLight(activeVeh);
                                    }
                                    else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_INDL))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        if (activeVeh.IndStatus == IndStatus.Left)
                                            activeVeh.IndStatus = IndStatus.Off;
                                        else
                                            activeVeh.IndStatus = IndStatus.Left;
                                        Lights.UpdateIndicator(activeVeh);
                                    }
                                    else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_INDR))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        if (activeVeh.IndStatus == IndStatus.Right)
                                            activeVeh.IndStatus = IndStatus.Off;
                                        else
                                            activeVeh.IndStatus = IndStatus.Right;
                                        Lights.UpdateIndicator(activeVeh);
                                    }
                                    else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_HAZRD))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                        if (activeVeh.IndStatus == IndStatus.Both)
                                            activeVeh.IndStatus = IndStatus.Off;
                                        else
                                            activeVeh.IndStatus = IndStatus.Both;
                                        Lights.UpdateIndicator(activeVeh);
                                    }
                                    #endregion Lights Manager
                                    #region Siren Manager                                    
                                    if (activeVeh.SirenStage == SirenStage.Off)
                                    {
                                        if (tone1ButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.One))
                                            {
                                                activeVeh.SirenStage = SirenStage.One;
                                                Utils.Sirens.Update(activeVeh);

                                                manActive = false;
                                            }
                                        }
                                        else if (tone2ButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Two))
                                            {
                                                activeVeh.SirenStage = SirenStage.Two;
                                                Utils.Sirens.Update(activeVeh);

                                                manActive = false;
                                            }
                                        }
                                        else if (tone3ButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning))
                                            {
                                                activeVeh.SirenStage = SirenStage.Warning;
                                                Utils.Sirens.Update(activeVeh);

                                                manActive = false;
                                            }
                                        }
                                        else if (tone4ButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning2))
                                            {
                                                activeVeh.SirenStage = SirenStage.Warning2;
                                                Utils.Sirens.Update(activeVeh);

                                                manActive = false;
                                            }
                                        }
                                        else if (manButtonDown && !hornButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.One))
                                                activeVeh.SirenStage = SirenStage.One;
                                            else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Two))
                                                activeVeh.SirenStage = SirenStage.Two;
                                            else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning))
                                                activeVeh.SirenStage = SirenStage.Warning;
                                            else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning2))
                                                activeVeh.SirenStage = SirenStage.Warning2;
                                            Utils.Sirens.Update(activeVeh);

                                            manActive = true;
                                        }
                                        else if (manButtonDown && hornButtonDown)
                                        {
                                            if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Two))
                                                activeVeh.SirenStage = SirenStage.Two;
                                            else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning))
                                                activeVeh.SirenStage = SirenStage.Warning;
                                            else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning2))
                                                activeVeh.SirenStage = SirenStage.Warning2;
                                            Utils.Sirens.Update(activeVeh);

                                            manActive = true;
                                        }
                                        else if (!manButtonDown && hornButtonDown)
                                        {
                                            activeVeh.SirenStage = SirenStage.Horn;
                                            Utils.Sirens.Update(activeVeh);
                                            hornButtonDown = true;
                                            manActive = true;
                                        }
                                    }
                                    else if (manActive)
                                    {
                                        if (manButtonDown && !hornButtonDown)
                                        {
                                            if (activeVeh.SirenStage != SirenStage.One)
                                            {
                                                if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.One))
                                                    activeVeh.SirenStage = SirenStage.One;
                                                else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Two))
                                                    activeVeh.SirenStage = SirenStage.Two;
                                                else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning))
                                                    activeVeh.SirenStage = SirenStage.Warning;
                                                else if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning2))
                                                    activeVeh.SirenStage = SirenStage.Warning2;
                                                Utils.Sirens.Update(activeVeh);
                                            }
                                        }
                                        else if (manButtonDown && hornButtonDown)
                                        {
                                            if (activeVeh.SirenStage != SirenStage.Two)
                                            {
                                                activeVeh.SirenStage = SirenStage.Two;
                                                Utils.Sirens.Update(activeVeh);
                                            }
                                        }
                                        else if (!manButtonDown && hornButtonDown)
                                        {
                                            if (activeVeh.SirenStage != SirenStage.Horn)
                                            {
                                                activeVeh.SirenStage = SirenStage.Horn;
                                                Utils.Sirens.Update(activeVeh);
                                            }
                                        }
                                        else
                                        {
                                            activeVeh.SirenStage = SirenStage.Off;
                                            Utils.Sirens.Update(activeVeh);
                                            hornButtonDown = false;
                                            manActive = false;
                                        }
                                    }
                                    else
                                    {
                                        if (!ahrnInterruptsSiren)
                                        {
                                            if (hornButtonDown)
                                            {
                                                if (!activeVeh.HornOn)
                                                {
                                                    activeVeh.HornID = Sound.TempSoundID();
                                                    activeVeh.HornOn = true;
                                                    NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.HornID, vehDLS.SoundSettings.Horn, activeVeh.Vehicle, 0, 0, 0);
                                                }
                                            }
                                            else if (activeVeh.HornOn)
                                            {
                                                Sound.ClearTempSoundID(activeVeh.HornID);
                                                activeVeh.HornOn = false;
                                            }
                                        }

                                        if (hornButtonDown && ahrnInterruptsSiren)
                                        {
                                            if (sirenStageBeforeHorn == SirenStage.Off)
                                            {
                                                sirenStageBeforeHorn = activeVeh.SirenStage;
                                            }
                                            if (activeVeh.SirenStage != SirenStage.Horn)
                                            {
                                                activeVeh.SirenStage = SirenStage.Horn;
                                                activeVeh.HornOn = true;
                                                Utils.Sirens.Update(activeVeh);
                                            }
                                        }
                                        else if (manButtonDown)
                                        {
                                            if (sirenStageBeforeHorn == SirenStage.Off)
                                            {
                                                sirenStageBeforeHorn = activeVeh.SirenStage;
                                            }
                                            if (activeVeh.SirenStage != Utils.Sirens.GetNextStage(sirenStageBeforeHorn, vehDLS))
                                            {
                                                activeVeh.SirenStage = Utils.Sirens.GetNextStage(sirenStageBeforeHorn, vehDLS);
                                                Utils.Sirens.Update(activeVeh);
                                            }
                                        }
                                        else if (sirenStageBeforeHorn != SirenStage.Off)
                                        {
                                            activeVeh.SirenStage = sirenStageBeforeHorn;
                                            Utils.Sirens.Update(activeVeh);

                                            sirenStageBeforeHorn = SirenStage.Off;
                                        }

                                        if (sirenStageBeforeHorn == SirenStage.Off)
                                        {
                                            if (tone1ButtonDown)
                                            {
                                                if (activeVeh.SirenStage == SirenStage.One)
                                                {
                                                    activeVeh.SirenStage = SirenStage.Off;
                                                    Utils.Sirens.Update(activeVeh);
                                                }
                                                else
                                                {
                                                    if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.One))
                                                    {
                                                        activeVeh.SirenStage = SirenStage.One;
                                                        Utils.Sirens.Update(activeVeh);
                                                    }
                                                }
                                            }
                                            else if (tone2ButtonDown)
                                            {
                                                if (activeVeh.SirenStage == SirenStage.Two)
                                                {
                                                    activeVeh.SirenStage = SirenStage.Off;
                                                    Utils.Sirens.Update(activeVeh);
                                                }
                                                else
                                                {
                                                    if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Two))
                                                    {
                                                        activeVeh.SirenStage = SirenStage.Two;
                                                        Utils.Sirens.Update(activeVeh);
                                                    }
                                                }
                                            }
                                            else if (tone3ButtonDown)
                                            {
                                                if (activeVeh.SirenStage == SirenStage.Warning)
                                                {
                                                    activeVeh.SirenStage = SirenStage.Off;
                                                    Utils.Sirens.Update(activeVeh);
                                                }
                                                else
                                                {
                                                    if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning))
                                                    {
                                                        activeVeh.SirenStage = SirenStage.Warning;
                                                        Utils.Sirens.Update(activeVeh);
                                                    }
                                                }
                                            }
                                            else if (tone4ButtonDown)
                                            {
                                                if (activeVeh.SirenStage == SirenStage.Warning2)
                                                {
                                                    activeVeh.SirenStage = SirenStage.Off;
                                                    Utils.Sirens.Update(activeVeh);
                                                }
                                                else
                                                {
                                                    if (vehDLS.DoesVehicleHaveSirenStage(SirenStage.Warning2))
                                                    {
                                                        activeVeh.SirenStage = SirenStage.Warning2;
                                                        Utils.Sirens.Update(activeVeh);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Controls.IsDLSControlDown(DLSControls.SIREN_AUX))
                                    {
                                        if (activeVeh.AuxOn)
                                        {
                                            Sound.ClearTempSoundID(activeVeh.AuxID);
                                            activeVeh.AuxOn = false;
                                        }
                                        else
                                        {
                                            activeVeh.AuxID = Sound.TempSoundID();
                                            activeVeh.AuxOn = true;
                                            NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AuxID, vehDLS.SoundSettings.Tone1, activeVeh.Vehicle, 0, 0, 0);
                                        }
                                    }
                                    else if (activeVeh.LightStage != LightStage.Off
                                            && Controls.IsDLSControlDown(DLSControls.SIREN_TOGGLE))
                                    {
                                        if (activeVeh.SirenStage == SirenStage.Off)
                                        {
                                            Utils.Sirens.MoveUpStage(activeVeh, true);
                                        }
                                        else
                                        {
                                            activeVeh.SirenStage = SirenStage.Off;
                                            activeVeh.IsScanOn = false;
                                            Utils.Sirens.Update(activeVeh);
                                        }
                                    }
                                    else if (Controls.IsDLSControlDown(DLSControls.SIREN_SCAN))
                                    {
                                        if (!activeVeh.IsScanOn)
                                        {
                                            scanTimer.Start();
                                            scanTiming = random.Next(2, 8);
                                            activeVeh.SirenStage = vehDLS.AvailableSirenStages[random.Next(1, vehDLS.AvailableSirenStages.Count - 1)];
                                            Utils.Sirens.Update(activeVeh);
                                        }
                                        if (activeVeh.IsScanOn)
                                        {
                                            activeVeh.SirenStage = SirenStage.Off;
                                            Utils.Sirens.Update(activeVeh);
                                        }
                                        activeVeh.IsScanOn = !activeVeh.IsScanOn;
                                    }
                                    if (activeVeh.IsScanOn && !(manButtonDown || (vehDLS.SoundSettings.AirHornInterruptsSiren.ToBoolean() && hornButtonDown)) && scanTimer.ElapsedMilliseconds >= (scanTiming * 1000))
                                    {
                                        scanTimer.Restart();
                                        scanTiming = random.Next(2, 8);
                                        SirenStage old = activeVeh.SirenStage;
                                        SirenStage _new = vehDLS.AvailableSirenStages[random.Next(1, vehDLS.AvailableSirenStages.Count)];
                                        while (old == _new)
                                        {
                                            GameFiber.Yield();
                                            _new = vehDLS.AvailableSirenStages[random.Next(1, vehDLS.AvailableSirenStages.Count)];
                                        }
                                        activeVeh.SirenStage = _new;
                                        Utils.Sirens.Update(activeVeh);
                                    }
                                    if (activeVeh.IsScanOn && (tone1ButtonDown || tone2ButtonDown || tone3ButtonDown || tone4ButtonDown))
                                        activeVeh.IsScanOn = false;
                                    if (!activeVeh.IsScanOn && scanTimer.IsRunning)
                                        scanTimer.Reset();
                                    #endregion Siren Manager
                                }
                            }
                            else if (Entrypoint.SCforNDLS)
                            {
                                Controls.DisableControls();
                                #region Lights Manager
                                if ((Game.IsKeyDown(Keys.F) || Game.IsControllerButtonDown(ControllerButtons.Y))
                                    && Vehicles.GetSirenKill(activeVeh) && activeVeh.SirenStage != SirenStage.Off)
                                {
                                    activeVeh.SirenStage = SirenStage.Off;
                                    Utils.Sirens.Update(activeVeh);
                                }
                                if (Controls.IsDLSControlDown(DLSControls.LIGHT_TOGGLE))
                                {
                                    switch (veh.IsSirenOn)
                                    {
                                        case true:
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                            activeVeh.LightStage = LightStage.Off;
                                            veh.IsSirenOn = false;
                                            activeVeh.SirenStage = SirenStage.Off;
                                            activeVeh.IsScanOn = false;
                                            Utils.Sirens.Update(activeVeh);
                                            break;
                                        case false:
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                            activeVeh.LightStage = LightStage.Three;
                                            veh.IsSirenOn = true;
                                            veh.IsSirenSilent = true;
                                            break;
                                    }
                                }
                                else if (Controls.IsDLSControlDown(DLSControls.LIGHT_INTLT))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                    Lights.ToggleIntLight(activeVeh);
                                }
                                else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_INDL))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                    if (activeVeh.IndStatus == IndStatus.Left)
                                        activeVeh.IndStatus = IndStatus.Off;
                                    else
                                        activeVeh.IndStatus = IndStatus.Left;
                                    Lights.UpdateIndicator(activeVeh);
                                }
                                else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_INDR))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                    if (activeVeh.IndStatus == IndStatus.Right)
                                        activeVeh.IndStatus = IndStatus.Off;
                                    else
                                        activeVeh.IndStatus = IndStatus.Right;
                                    Lights.UpdateIndicator(activeVeh);
                                }
                                else if (Entrypoint.IndEnabled
                                        && Controls.IsDLSControlDown(DLSControls.LIGHT_HAZRD))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                    if (activeVeh.IndStatus == IndStatus.Both)
                                        activeVeh.IndStatus = IndStatus.Off;
                                    else
                                        activeVeh.IndStatus = IndStatus.Both;
                                    Lights.UpdateIndicator(activeVeh);
                                }
                                #endregion Lights Manager
                                #region Siren Manager
                                if (activeVeh.SirenStage == SirenStage.Off)
                                {
                                    if (tone1ButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.One;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = false;
                                    }
                                    else if (tone2ButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.Two;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = false;
                                    }
                                    else if (tone3ButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.Warning;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = false;
                                    }
                                    else if (tone4ButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.Warning2;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = false;
                                    }
                                    else if (manButtonDown && !hornButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.One;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = true;
                                    }
                                    else if (manButtonDown && hornButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.Two;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = true;
                                    }
                                    else if (!manButtonDown && hornButtonDown)
                                    {
                                        activeVeh.SirenStage = SirenStage.Horn;
                                        Utils.Sirens.Update(activeVeh, false);

                                        manActive = true;
                                    }
                                }
                                else if (manActive)
                                {
                                    if (manButtonDown && !hornButtonDown)
                                    {
                                        if (activeVeh.SirenStage != SirenStage.One)
                                        {
                                            activeVeh.SirenStage = SirenStage.One;
                                            Utils.Sirens.Update(activeVeh, false);
                                        }
                                    }
                                    else if (manButtonDown && hornButtonDown)
                                    {
                                        if (activeVeh.SirenStage != SirenStage.Two)
                                        {
                                            activeVeh.SirenStage = SirenStage.Two;
                                            Utils.Sirens.Update(activeVeh, false);
                                        }
                                    }
                                    else if (!manButtonDown && hornButtonDown)
                                    {
                                        if (activeVeh.SirenStage != SirenStage.Horn)
                                        {
                                            activeVeh.SirenStage = SirenStage.Horn;
                                            Utils.Sirens.Update(activeVeh, false);
                                        }
                                    }
                                    else
                                    {
                                        activeVeh.SirenStage = SirenStage.Off;
                                        Utils.Sirens.Update(activeVeh, false);
                                        manActive = false;
                                    }
                                }
                                else
                                {
                                    if (hornButtonDown)
                                    {
                                        if (sirenStageBeforeHorn == SirenStage.Off)
                                        {
                                            sirenStageBeforeHorn = activeVeh.SirenStage;
                                        }
                                        if (activeVeh.SirenStage != SirenStage.Horn)
                                        {
                                            activeVeh.SirenStage = SirenStage.Horn;
                                            Utils.Sirens.Update(activeVeh, false);
                                        }
                                    }
                                    else if (manButtonDown)
                                    {
                                        if (sirenStageBeforeHorn == SirenStage.Off)
                                        {
                                            sirenStageBeforeHorn = activeVeh.SirenStage;
                                        }
                                        if (activeVeh.SirenStage != Utils.Sirens.GetNextStage(sirenStageBeforeHorn))
                                        {
                                            activeVeh.SirenStage = Utils.Sirens.GetNextStage(sirenStageBeforeHorn);
                                            Utils.Sirens.Update(activeVeh, false);
                                        }
                                    }
                                    else if (sirenStageBeforeHorn != SirenStage.Off)
                                    {
                                        activeVeh.SirenStage = sirenStageBeforeHorn;
                                        Utils.Sirens.Update(activeVeh, false);

                                        sirenStageBeforeHorn = SirenStage.Off;
                                    }

                                    if (sirenStageBeforeHorn == SirenStage.Off)
                                    {
                                        if (tone1ButtonDown)
                                        {
                                            if (activeVeh.SirenStage == SirenStage.One)
                                            {
                                                activeVeh.SirenStage = SirenStage.Off;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                            else
                                            {
                                                activeVeh.SirenStage = SirenStage.One;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                        }
                                        else if (tone2ButtonDown)
                                        {
                                            if (activeVeh.SirenStage == SirenStage.Two)
                                            {
                                                activeVeh.SirenStage = SirenStage.Off;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                            else
                                            {
                                                activeVeh.SirenStage = SirenStage.Two;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                        }
                                        else if (tone3ButtonDown)
                                        {
                                            if (activeVeh.SirenStage == SirenStage.Warning)
                                            {
                                                activeVeh.SirenStage = SirenStage.Off;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                            else
                                            {
                                                activeVeh.SirenStage = SirenStage.Warning;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                        }
                                        else if (tone4ButtonDown)
                                        {
                                            if (activeVeh.SirenStage == SirenStage.Warning2)
                                            {
                                                activeVeh.SirenStage = SirenStage.Off;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                            else
                                            {
                                                activeVeh.SirenStage = SirenStage.Warning2;
                                                Utils.Sirens.Update(activeVeh, false);
                                            }
                                        }
                                    }
                                }
                                if (Controls.IsDLSControlDown(DLSControls.SIREN_AUX))
                                {
                                    if (activeVeh.AuxOn)
                                    {
                                        Sound.ClearTempSoundID(activeVeh.AuxID);
                                        activeVeh.AuxOn = false;
                                    }
                                    else
                                    {
                                        activeVeh.AuxID = Sound.TempSoundID();
                                        activeVeh.AuxOn = true;
                                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVeh.AuxID, "VEHICLES_HORNS_SIREN_1", activeVeh.Vehicle, 0, 0, 0);
                                    }
                                }
                                else if (activeVeh.LightStage != LightStage.Off
                                            && Controls.IsDLSControlDown(DLSControls.SIREN_TOGGLE))
                                {
                                    if (activeVeh.SirenStage == SirenStage.Off)
                                        Utils.Sirens.MoveUpStage(activeVeh);
                                    else
                                    {
                                        activeVeh.SirenStage = SirenStage.Off;
                                        activeVeh.IsScanOn = false;
                                        Utils.Sirens.Update(activeVeh);
                                    }
                                }
                                else if (Controls.IsDLSControlDown(DLSControls.SIREN_SCAN))
                                {
                                    if (!activeVeh.IsScanOn)
                                    {
                                        scanTimer.Start();
                                        scanTiming = random.Next(2, 8);
                                        activeVeh.SirenStage = (SirenStage)random.Next(1, 4);
                                        Utils.Sirens.Update(activeVeh, false);
                                    }
                                    if (activeVeh.IsScanOn)
                                    {
                                        activeVeh.SirenStage = SirenStage.Off;
                                        Utils.Sirens.Update(activeVeh, false);
                                    }
                                    activeVeh.IsScanOn = !activeVeh.IsScanOn;
                                }
                                if (activeVeh.IsScanOn && !manButtonDown && !hornButtonDown && scanTimer.ElapsedMilliseconds >= (scanTiming * 1000))
                                {
                                    scanTimer.Restart();
                                    scanTiming = random.Next(2, 8);
                                    SirenStage old = activeVeh.SirenStage;
                                    SirenStage _new = (SirenStage)random.Next(1, 4);
                                    while (old == _new)
                                    {
                                        GameFiber.Yield();
                                        _new = (SirenStage)random.Next(1, 4);
                                    }
                                    activeVeh.SirenStage = _new;
                                    Utils.Sirens.Update(activeVeh, false);
                                }
                                if (activeVeh.IsScanOn && (tone1ButtonDown || tone2ButtonDown || tone3ButtonDown || tone4ButtonDown))
                                    activeVeh.IsScanOn = false;
                                if (!activeVeh.IsScanOn && scanTimer.IsRunning)
                                    scanTimer.Reset();
                                #endregion Siren Manager
                            }
                        }
                    }
                }
            }
        }
    }
}
