using DLS.Utils;
using Rage;
using Rage.Native;
using System.Windows.Forms;

namespace DLS.Threads
{    
    class NPlayerController
    {
        /// <summary>
        /// Stores Information from Previous Loop
        /// </summary>
        private static Vehicle veh;
        private static ActiveVehicle aVeh;
        private static DLSModel dlsModel;

        /// <summary>
        /// Main Loop, checks for player input.
        /// </summary>
        internal static void MainLoop()
        {
            while (true)
            {
                Vehicle v = Game.LocalPlayer.Character.CurrentVehicle;
                if(!Game.IsPaused && v && v.HasSiren && (v.IsEngineOn || v.IsEngineStarting) &&
                    v.Driver == Game.LocalPlayer.Character)
                {
                    // Performs check if current vehicle is the same from last loop, avoid repetitive tasks.
                    if(v == veh)
                    {
                        // If vehicle is non-DLS and SCNDLS is disabled, continues
                        if (dlsModel == null && !Settings.SET_SCNDLS)
                            continue;
                        // Checks for DLS Lock Key
                        if (Controls.IsDLSControlDown(DLSControls.GEN_LOCKALL))
                        {
                            Entrypoint.keysLocked = !Entrypoint.keysLocked;
                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                        }
                        // Adds Brake Light Functionality
                        if (!aVeh.blktOn && Settings.SET_BRAKELIGHT && NativeFunction.Natives.IS_VEHICLE_STOPPED<bool>(v))
                            NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(v, true);
                        // Checks if DLS is locked
                        if (!Entrypoint.keysLocked)
                        {
                            // Disables the vehicle's radio as a temporary fix.
                            // TODO: Figure a workaround.
                            NativeFunction.Natives.SET_VEHICLE_RADIO_ENABLED(v, false);
                            // Check for the UI Toggle Key
                            if (Controls.IsDLSControlDownWithModifier(DLSControls.UI_TOGGLE))
                                UIManager.IsUIOn = !UIManager.IsUIOn;
                            Controls.DisableControls();
                            #region DLS Vehicles
                            if (dlsModel != null)
                            {
                                // Resets back from OOV stage and sets
                                if (aVeh.TempUsed
                                && aVeh.TempLightStage != aVeh.LightStage)
                                {
                                    aVeh.LightStage = aVeh.TempLightStage;
                                    Lights.Update(aVeh);
                                    aVeh.TempUsed = false;
                                }
                                // If vehicle has TA, checks for TAdvisor key
                                else if (aVeh.TAType != "off"
                                && Controls.IsDLSControlDownWithModifier(DLSControls.LIGHT_TADVISOR))
                                {
                                    int index = aVeh.TAgroup.TaPatterns.IndexOf(aVeh.TAgroup.TaPatterns[aVeh.TApatternCurrentIndex]);
                                    aVeh.TApatternCurrentIndex = (index + 1) % aVeh.TAgroup.TaPatterns.Count;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    Lights.UpdateTA(true, aVeh);
                                    continue;
                                }
                                // Checks for vehicle exit for OOV stage
                                else if (Game.IsControlJustPressed(0, GameControl.VehicleExit))
                                {
                                    if (dlsModel.SpecialModes.PresetSirenOnLeaveVehicle != "none")
                                    {
                                        if (aVeh.LightStage != LightStage.Off && aVeh.LightStage != LightStage.Empty)
                                        {
                                            aVeh.TempLightStage = aVeh.LightStage;
                                            aVeh.TempUsed = true;
                                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(veh, LeaveVehicleFlags.None);
                                            GameFiber.Sleep(1000);
                                            if (!veh.IsEngineOn)
                                                veh.IsEngineOn = true;
                                            string presetSiren = dlsModel.SpecialModes.PresetSirenOnLeaveVehicle;
                                            switch (presetSiren)
                                            {
                                                case "stage1":
                                                    aVeh.LightStage = LightStage.One;
                                                    Lights.Update(aVeh);
                                                    break;
                                                case "stage2":
                                                    aVeh.LightStage = LightStage.Two;
                                                    Lights.Update(aVeh);
                                                    break;
                                                case "stage3":
                                                    aVeh.LightStage = LightStage.Three;
                                                    Lights.Update(aVeh);
                                                    break;
                                                case "custom1":
                                                    aVeh.LightStage = LightStage.CustomOne;
                                                    Lights.Update(aVeh);
                                                    break;
                                                case "custom2":
                                                    aVeh.LightStage = LightStage.CustomOne;
                                                    Lights.Update(aVeh);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            Game.LocalPlayer.Character.Tasks.LeaveVehicle(veh, LeaveVehicleFlags.None);
                                        }
                                    }
                                    if (Vehicles.GetSirenKill(aVeh) && aVeh.SirenStage != SirenStage.Off)
                                    {
                                        aVeh.SirenStage = SirenStage.Off;
                                        Utils.Sirens.Update(aVeh);
                                    }
                                }
                                #region Light Controls
                                // Checks for Blackout key
                                if (Controls.IsDLSControlDownWithModifier(DLSControls.LIGHT_SBURN))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    aVeh.blktOn = !aVeh.blktOn;
                                    if (aVeh.blktOn)
                                    {
                                        aVeh.IndStatus = IndStatus.Off;
                                        aVeh.LightStage = LightStage.Off;
                                        aVeh.SBOn = false;
                                        aVeh.TAStage = TAStage.Off;
                                        if (aVeh.IntLightOn)
                                            Lights.ToggleIntLight(aVeh);
                                        Lights.Update(aVeh);
                                        Lights.UpdateIndicator(aVeh);
                                        NativeFunction.Natives.SET_VEHICLE_LIGHTS(veh, 1);
                                        continue;
                                    }
                                    else
                                    {
                                        NativeFunction.Natives.SET_VEHICLE_LIGHTS(veh, 0);
                                        continue;
                                    }
                                }
                                // Checks for skip to LS1
                                else if (dlsModel.DoesVehicleHaveLightStage(LightStage.One)
                                    && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D1))
                                {
                                    if (aVeh.LightStage != LightStage.One)
                                        aVeh.LightStage = LightStage.One;
                                    else
                                        aVeh.LightStage = LightStage.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    Lights.Update(aVeh);
                                }
                                // Checks for skip to LS2
                                else if (dlsModel.DoesVehicleHaveLightStage(LightStage.Two)
                                        && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D2))
                                {
                                    if (aVeh.LightStage != LightStage.Two)
                                        aVeh.LightStage = LightStage.Two;
                                    else
                                        aVeh.LightStage = LightStage.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    Lights.Update(aVeh);
                                }
                                // Checks for skip to LS3
                                else if (dlsModel.DoesVehicleHaveLightStage(LightStage.Three)
                                        && Game.IsAltKeyDownRightNow && Game.IsKeyDown(Keys.D3))
                                {
                                    if (aVeh.LightStage != LightStage.Three)
                                        aVeh.LightStage = LightStage.Three;
                                    else
                                        aVeh.LightStage = LightStage.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    Lights.Update(aVeh);
                                }
                                // Checks for move to previous stage
                                else if (Controls.IsDLSControlDownWithModifier(DLSControls.LIGHT_TOGGLE)) { Lights.MoveDownStage(aVeh); }
                                // Checks for move to next stage
                                else if (Controls.IsDLSControlDown(DLSControls.LIGHT_TOGGLE)) { Lights.MoveUpStage(aVeh); }
                                // Checks for move to next TA key
                                else if (Controls.IsDLSControlDown(DLSControls.LIGHT_TADVISOR)
                                    && dlsModel.TrafficAdvisory.Type.ToLower() != "off")
                                {
                                    if (aVeh.LightStage == LightStage.Off)
                                    {
                                        aVeh.LightStage = LightStage.Empty;
                                        veh.ShouldVehiclesYieldToThisVehicle = false;
                                        veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                        if (aVeh.SBOn)
                                            Lights.UpdateSB(aVeh);
                                        veh.IsSirenOn = true;
                                        veh.IsSirenSilent = true;
                                        Lights.MoveUpStageTA(aVeh);
                                        continue;
                                    }
                                    else if (aVeh.LightStage == LightStage.Empty)
                                    {
                                        Lights.MoveUpStageTA(aVeh);
                                        if (aVeh.TAStage == TAStage.Off)
                                        {
                                            if (aVeh.SBOn)
                                            {
                                                veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                                Lights.UpdateSB(aVeh);
                                                continue;
                                            }
                                            aVeh.LightStage = LightStage.Off;
                                            veh.ShouldVehiclesYieldToThisVehicle = true;
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            veh.IsSirenOn = false;
                                        }
                                        Lights.UpdateSB(aVeh);
                                        continue;
                                    }
                                    Lights.MoveUpStageTA(aVeh);
                                }
                                // Checks for steady burn key
                                else if (Controls.IsDLSControlDown(DLSControls.LIGHT_SBURN)
                                    && dlsModel.SpecialModes.SteadyBurn.SteadyBurnEnabled.ToBoolean())
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    aVeh.SBOn = !aVeh.SBOn;
                                    if (aVeh.SBOn && aVeh.LightStage == LightStage.Off)
                                    {
                                        aVeh.LightStage = LightStage.Empty;
                                        veh.ShouldVehiclesYieldToThisVehicle = false;
                                        veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                        if (aVeh.TAStage != TAStage.Off)
                                            Lights.UpdateTA(true, aVeh);
                                        veh.IsSirenOn = true;
                                        veh.IsSirenSilent = true;
                                    }
                                    else if (!aVeh.SBOn && aVeh.LightStage == LightStage.Empty)
                                    {
                                        if (aVeh.TAStage != TAStage.Off)
                                        {
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            Lights.UpdateTA(true, aVeh);
                                            continue;
                                        }
                                        aVeh.LightStage = LightStage.Off;
                                        veh.ShouldVehiclesYieldToThisVehicle = true;
                                        veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                        veh.IsSirenOn = false;
                                    }
                                    Lights.UpdateSB(aVeh);
                                }
                                // Checks for interior light key
                                else if (Controls.IsDLSControlDown(DLSControls.LIGHT_INTLT))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    Lights.ToggleIntLight(aVeh);
                                }
                                // Checks for indicator left key
                                else if (Settings.SET_INDENABLED
                                    && Controls.IsDLSControlDown(DLSControls.LIGHT_INDL))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    if (aVeh.IndStatus == IndStatus.Left)
                                        aVeh.IndStatus = IndStatus.Off;
                                    else
                                        aVeh.IndStatus = IndStatus.Left;
                                    Lights.UpdateIndicator(aVeh);
                                }
                                // Checks for indicator right key
                                else if (Settings.SET_INDENABLED
                                    && Controls.IsDLSControlDown(DLSControls.LIGHT_INDR))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    if (aVeh.IndStatus == IndStatus.Right)
                                        aVeh.IndStatus = IndStatus.Off;
                                    else
                                        aVeh.IndStatus = IndStatus.Right;
                                    Lights.UpdateIndicator(aVeh);
                                }
                                // Checks for indicator hazards key
                                else if (Settings.SET_INDENABLED
                                    && Controls.IsDLSControlDown(DLSControls.LIGHT_HAZRD))
                                {
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                    if (aVeh.IndStatus == IndStatus.Both)
                                        aVeh.IndStatus = IndStatus.Off;
                                    else
                                        aVeh.IndStatus = IndStatus.Both;
                                    Lights.UpdateIndicator(aVeh);
                                }
                                #endregion
                                #region Siren Controls
                                // Manual and Horn
                                if (aVeh.SirenStage == SirenStage.Off && Controls.IsDLSControlDown(DLSControls.SIREN_MAN))
                                {
                                    
                                }
                                /*
                                    actv_man = true;
                                else
                                    actv_man = false;
                                if (Controls.IsDLSControlDown(DLSControls.SIREN_HORN))
                                    actv_hrn = true;
                                else
                                    actv_hrn = false;
                                int hmanu_state_new = 0;
                                if (actv_hrn == true && actv_man == false)
                                    hmanu_state_new = 1;
                                else if (actv_hrn == false && actv_man == true)
                                    hmanu_state_new = 2;
                                else if (actv_hrn == true && actv_man == true)
                                    hmanu_state_new = 3;*/
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        // Check if aVeh is null and creates it if needed
                        ActiveVehicle _ = v.GetActiveVehicle();
                        if (_ == null)
                        {
                            if (v.IsSirenOn)
                            {
                                if (!v.IsSirenSilent)
                                    Entrypoint.activeVehicles.Add(new ActiveVehicle(v, true, LightStage.Three, SirenStage.One));
                                else
                                    Entrypoint.activeVehicles.Add(new ActiveVehicle(v, true, LightStage.Three, SirenStage.Off));
                            }
                            else
                                Entrypoint.activeVehicles.Add(new ActiveVehicle(v, true));
                        }
                        aVeh = _;

                        // Gets current vehicle's DLS Model
                        dlsModel = v.GetDLS();

                        // Resets the UI for the new vehicle
                        if (dlsModel != null)
                        {
                            if (dlsModel.SpecialModes.SirenUI != "")
                                UI.Importer.GetCustomSprites(dlsModel.SpecialModes.SirenUI);
                            else
                            {
                                UI.Importer.ResetSprites();
                                UI.Importer.GetCustomSprites();
                            }
                        }

                        // Sets the current vehicle
                        veh = v;
                    }                    
                }
                GameFiber.Yield();
            }
        }
    }
}
