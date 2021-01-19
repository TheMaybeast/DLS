using DLS.Utils;
using Rage;
using Rage.Native;

namespace DLS.Threads
{
    class PlayerController
    {
        private static Vehicle prevVehicle;
        private static ActiveVehicle activeVehicle;
        private static DLSModel dlsModel;
        private static bool isDLS = false;

        internal static void MainLoop()
        {
            while (true)
            {
                Ped playerPed = Game.LocalPlayer.Character;
                if (playerPed.IsInAnyVehicle(false))
                {
                    Vehicle veh = playerPed.CurrentVehicle;
                    // Inside here basic vehicle functionality will be available
                    // eg. Indicators and Internal Lights
                    if (veh.GetPedOnSeat(-1) == playerPed)
                    {
                        if (Settings.SET_INDENABLED)
                        {
                            Game.DisableControlAction(0, (GameControl)Settings.CON_HZRD, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_INDRIGHT, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_INDLEFT, true);
                        }

                        // Adds Brake Light Functionality
                        if (Settings.SET_BRAKELIGHT && NativeFunction.Natives.IS_VEHICLE_STOPPED<bool>(veh))
                            NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(veh, true);

                        // Registers new Vehicle
                        if (activeVehicle == null || prevVehicle != veh)
                        {
                            activeVehicle = veh.GetActiveVehicle();

                            dlsModel = veh.GetDLS();
                            if (dlsModel == null)
                                isDLS = false;
                            else
                                isDLS = true;
                            if (isDLS)
                            {
                                // Resets the UI for the new vehicle
                                if (dlsModel.SpecialModes.SirenUI != "")
                                    UI.Importer.GetCustomSprites(dlsModel.SpecialModes.SirenUI);
                                else
                                {
                                    UI.Importer.ResetSprites();
                                    UI.Importer.GetCustomSprites();
                                }
                            }
                            prevVehicle = veh;
                        }

                        // Inside here additional DLS functionality will be available
                        if (veh.HasSiren && (isDLS || (!isDLS && Settings.SET_SCNDLS)))
                        {
                            Game.DisableControlAction(0, (GameControl)Settings.CON_TOGGLESIREN, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_NEXTSIREN, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_PREVSIREN, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_AUXSIREN, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_NEXTLIGHTS, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_PREVLIGHTS, true);
                            Game.DisableControlAction(0, (GameControl)Settings.CON_HORN, true);
                            

                            // Disables the vehicle's radio as a temporary fix.
                            // TODO: Figure a workaround.
                            NativeFunction.Natives.SET_VEHICLE_RADIO_ENABLED(veh, false);

                            if (!Game.IsPaused)
                            {
                                // (DLS) Move next stage
                                if (Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_NEXTLIGHTS) && isDLS)
                                    Lights.MoveUpStage(activeVehicle);
                                // (DLS) Move previous stage
                                if (Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_PREVLIGHTS) && isDLS)
                                    Lights.MoveDownStage(activeVehicle);
                                // (non-DLS) Toggle lighting
                                if(Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_NEXTLIGHTS) && !isDLS)
                                {
                                    switch (veh.IsSirenOn)
                                    {
                                        case true:
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                            activeVehicle.LightStage = LightStage.Off;
                                            veh.IsSirenOn = false;
                                            activeVehicle.SirenStage = SirenStage.Off;
                                            activeVehicle.IsScanOn = false;
                                            Utils.Sirens.Update(activeVehicle);
                                            break;
                                        case false:
                                            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                            activeVehicle.LightStage = LightStage.Three;
                                            veh.IsSirenOn = true;
                                            veh.IsSirenSilent = true;
                                            break;
                                    }
                                }
                                // (DLS) Traffic Advisory
                                if(activeVehicle.TAType != "off" && 
                                    Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_TA) && isDLS)
                                {
                                    if (activeVehicle.LightStage == LightStage.Off)
                                    {
                                        activeVehicle.LightStage = LightStage.Empty;
                                        veh.ShouldVehiclesYieldToThisVehicle = false;
                                        veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                        Lights.UpdateSB(activeVehicle);
                                        veh.IsSirenOn = true;
                                        veh.IsSirenSilent = true;
                                        Lights.MoveUpStageTA(activeVehicle);
                                    }
                                    else if (activeVehicle.LightStage == LightStage.Empty)
                                    {
                                        Lights.MoveUpStageTA(activeVehicle);
                                        if (activeVehicle.TAStage == TAStage.Off)
                                        {
                                            if (activeVehicle.SBOn)
                                            {
                                                veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                                Lights.UpdateSB(activeVehicle);
                                            }
                                            activeVehicle.LightStage = LightStage.Off;
                                            veh.ShouldVehiclesYieldToThisVehicle = true;
                                            veh.EmergencyLightingOverride = Vehicles.GetEL(veh);
                                            veh.IsSirenOn = false;
                                        }
                                        Lights.UpdateSB(activeVehicle);
                                    }
                                    else
                                    {
                                        Lights.MoveUpStageTA(activeVehicle);
                                    }                                    
                                }
                                // Toggle Aux Siren
                                if(Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_AUXSIREN))
                                {
                                    if (activeVehicle.AuxOn)
                                    {
                                        Sound.ClearTempSoundID(activeVehicle.AuxID);
                                        activeVehicle.AuxOn = false;
                                    }
                                    else
                                    {
                                        activeVehicle.AuxID = Sound.TempSoundID();
                                        activeVehicle.AuxOn = true;
                                        NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(activeVehicle.AuxID, "VEHICLES_HORNS_SIREN_1", activeVehicle.Vehicle, 0, 0, 0);
                                    }
                                }
                                // Siren Switches
                                if(activeVehicle.LightStage == LightStage.Three)
                                {
                                    if(Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_TOGGLESIREN))
                                    {
                                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, Settings.SET_AUDIONAME, Settings.SET_AUDIOREF, true);
                                        if (activeVehicle.SirenStage == SirenStage.Off)
                                            activeVehicle.SirenStage = SirenStage.One;
                                        else
                                            activeVehicle.SirenStage = SirenStage.Off;
                                        Utils.Sirens.Update(activeVehicle, isDLS);
                                    }
                                }
                                if(activeVehicle.SirenStage > SirenStage.Off)
                                {
                                    // Move Up Siren Stage
                                    if(Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_NEXTSIREN))
                                        Utils.Sirens.MoveUpStage(activeVehicle, isDLS, dlsModel);
                                    // Move Down Siren Stage
                                    if (Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_PREVSIREN))
                                        Utils.Sirens.MoveDownStage(activeVehicle, isDLS, dlsModel);
                                }

                                // Manual
                                bool actv_manu;                                
                                if (activeVehicle.SirenStage == SirenStage.Off)
                                {
                                    if (Controls.IsDisabledControlPressed(0, (GameControl)Settings.CON_NEXTSIREN))
                                        actv_manu = true;
                                    else
                                        actv_manu = false;
                                }
                                else
                                    actv_manu = false;

                                // Horn
                                bool actv_horn;                                
                                if (Controls.IsDisabledControlPressed(0, (GameControl)Settings.CON_HORN))
                                    actv_horn = true;
                                else
                                    actv_horn = false;

                                // Manage Horn and Manual siren
                                int hman_state = 0;
                                if (actv_horn && !actv_manu)
                                    hman_state = 1;
                                else if (!actv_horn && actv_manu)
                                    hman_state = 2;
                                else if (actv_horn && actv_manu)
                                    hman_state = 3;

                                Utils.Sirens.SetAirManuState(activeVehicle, isDLS, hman_state);
                            }
                        }

                        // Indicators
                        if (Settings.SET_INDENABLED && !Game.IsPaused)
                        {
                            // Left Indicator
                            if(Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_INDLEFT) && NativeFunction.Natives.xA571D46727E2B718<bool>(0))
                            {
                                if (activeVehicle.IndStatus == IndStatus.Left)
                                {
                                    activeVehicle.IndStatus = IndStatus.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                else
                                {
                                    activeVehicle.IndStatus = IndStatus.Left;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                Lights.UpdateIndicator(activeVehicle);
                            }
                            // Right Indicator
                            if (Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_INDRIGHT) && NativeFunction.Natives.xA571D46727E2B718<bool>(0))
                            {
                                if (activeVehicle.IndStatus == IndStatus.Right)
                                {
                                    activeVehicle.IndStatus = IndStatus.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                else
                                {
                                    activeVehicle.IndStatus = IndStatus.Right;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                Lights.UpdateIndicator(activeVehicle);
                            }
                            // Hazards
                            if (Controls.IsDisabledControlJustReleased(0, (GameControl)Settings.CON_HZRD) && NativeFunction.Natives.xA571D46727E2B718<bool>(0))
                            {
                                if (activeVehicle.IndStatus == IndStatus.Both)
                                {
                                    activeVehicle.IndStatus = IndStatus.Off;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                else
                                {
                                    activeVehicle.IndStatus = IndStatus.Both;
                                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "NAV_UP_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
                                }
                                Lights.UpdateIndicator(activeVehicle);
                            }
                        }
                    }
                }
                GameFiber.Yield();
            }
        }
    }
}
