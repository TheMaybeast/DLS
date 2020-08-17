using DLS.Utils;
using Rage;
using System;

namespace DLS.Threads
{
    class SpecialModesManager
    {
        public static void ProcessAI()
        {
            uint lastProcessTime = Game.GameTime;
            int timeBetweenChecks = 500;
            int yieldAfterChecks = 50;

            while (true)
            {
                int checksDone = 0;

                if (Entrypoint.AILightsC)
                {
                    Vehicle[] allWorldVehicles = World.GetAllVehicles();
                    foreach (Vehicle veh in allWorldVehicles)
                    {
                        if (veh && veh.HasSiren && veh.GetDLS() != null && !veh.IsPlayerVehicle())
                        {
                            if (veh.GetActiveVehicle() == null)
                            {
                                if (veh.IsSirenOn)
                                {
                                    if (!veh.IsSirenSilent)
                                        Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, false, LightStage.Three, SirenStage.One));
                                    else
                                        Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, false, LightStage.Three, SirenStage.Off));
                                }
                                else
                                    Entrypoint.activeVehicles.Add(new ActiveVehicle(veh, false));
                            }
                            ActiveVehicle activeVeh = veh.GetActiveVehicle();
                            DLSModel dlsModel;
                            if (veh)
                                dlsModel = veh.GetDLS();
                            else
                                dlsModel = null;
                            if (veh.IsSirenOn && veh.IsSirenSilent == false && activeVeh.SirenStage == SirenStage.Off)
                                activeVeh.SirenStage = SirenStage.One;
                            if (veh.IsSirenOn && activeVeh.LightStage == LightStage.Off)
                                activeVeh.LightStage = LightStage.Three;
                            if (dlsModel != null && dlsModel.SpecialModes.PresetSirenOnLeaveVehicle != "none"
                                    && veh.IsSirenOn)
                            {
                                if (!veh.HasDriver)
                                {
                                    if (!veh.IsEngineOn)
                                        veh.IsEngineOn = true;
                                    string presetSiren = dlsModel.SpecialModes.PresetSirenOnLeaveVehicle;
                                    switch (presetSiren)
                                    {
                                        case "stage1":
                                            if (activeVeh.LightStage != LightStage.One)
                                            {
                                                activeVeh.LightStage = LightStage.One;
                                                Lights.Update(activeVeh);
                                            }
                                            break;
                                        case "stage2":
                                            if (activeVeh.LightStage != LightStage.Two)
                                            {
                                                activeVeh.LightStage = LightStage.Two;
                                                Lights.Update(activeVeh);
                                            }
                                            break;
                                        case "stage3":
                                            if (activeVeh.LightStage != LightStage.Three)
                                            {
                                                activeVeh.LightStage = LightStage.Three;
                                                Lights.Update(activeVeh);
                                            }
                                            break;
                                        case "custom1":
                                            if (activeVeh.LightStage != LightStage.CustomOne)
                                            {
                                                activeVeh.LightStage = LightStage.CustomOne;
                                                Lights.Update(activeVeh);
                                            }
                                            break;
                                        case "custom2":
                                            if (activeVeh.LightStage != LightStage.CustomTwo)
                                            {
                                                activeVeh.LightStage = LightStage.CustomTwo;
                                                Lights.Update(activeVeh);
                                            }
                                            break;
                                    }
                                }

                                if (dlsModel.DoesVehicleHaveLightStage(LightStage.Three)
                                    && veh.HasDriver
                                    && veh.EmergencyLighting.Name != dlsModel.Name + " | " + activeVeh.LightStage.ToString() + " | " + activeVeh.TAStage.ToString() + " | " + activeVeh.SBOn.ToString())
                                {
                                    activeVeh.LightStage = LightStage.Three;
                                    Lights.Update(activeVeh);
                                }
                            }
                        }
                    }

                    checksDone++;
                    if (checksDone % yieldAfterChecks == 0)
                    {
                        GameFiber.Yield();
                    }
                }
                GameFiber.Sleep((int)Math.Max(timeBetweenChecks, Game.GameTime - lastProcessTime));
                lastProcessTime = Game.GameTime;
            }
        }

        public static void ProcessPlayer()
        {
            uint lastProcessTime = Game.GameTime;
            int timeBetweenChecks = 100;
            while (true)
            {
                Vehicle veh = Game.LocalPlayer.Character.CurrentVehicle;
                if (veh && veh.HasSiren && veh.GetDLS() != null)
                {
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
                    ActiveVehicle activeVeh = veh.GetActiveVehicle();
                    DLSModel vehDLS;
                    if (veh)
                        vehDLS = veh.GetDLS();
                    else
                        vehDLS = null;
                    if (vehDLS.SpecialModes.WailSetup.WailSetupEnabled.ToBoolean())
                    {
                        if (activeVeh.LightStage != LightStage.Off && activeVeh.LightStage != LightStage.Empty
                            && activeVeh.SirenStage == (SirenStage)vehDLS.SpecialModes.WailSetup.WailSirenTone.ToInt32()
                            && activeVeh.LightStage != (LightStage)vehDLS.SpecialModes.WailSetup.WailLightStage.ToInt32())
                        {
                            activeVeh.TempWailLightStage = activeVeh.LightStage;
                            activeVeh.LightStage = (LightStage)vehDLS.SpecialModes.WailSetup.WailLightStage.ToInt32();
                            Lights.Update(activeVeh);
                            activeVeh.Wailing = true;
                        }
                        if (activeVeh.Wailing
                            && activeVeh.SirenStage != (SirenStage)vehDLS.SpecialModes.WailSetup.WailSirenTone.ToInt32())
                        {
                            activeVeh.LightStage = veh.GetActiveVehicle().TempWailLightStage;
                            Lights.Update(activeVeh);
                            activeVeh.Wailing = false;
                        }
                    }
                }
                GameFiber.Sleep((int)Math.Max(timeBetweenChecks, Game.GameTime - lastProcessTime));
                lastProcessTime = Game.GameTime;
            }
        }
    }
}

