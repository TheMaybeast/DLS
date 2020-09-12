using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DLS.Utils
{
    class Vehicles
    {
        public static Dictionary<Model, DLSModel> GetAllModels()
        {
            string path = @"Plugins\DLS\";
            _ = new DLSModel();
            Dictionary<Model, DLSModel> dictModels = new Dictionary<Model, DLSModel>();
            foreach (string file in Directory.EnumerateFiles(path, "*.xml"))
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(DLSModel));
                    StreamReader streamReader = new StreamReader(file);

                    DLSModel dlsModel = (DLSModel)mySerializer.Deserialize(streamReader);
                    streamReader.Close();

                    dlsModel.AvailableLightStages.Add(LightStage.Off);
                    if (dlsModel.Sirens.Stage1Setting != null)
                        dlsModel.AvailableLightStages.Add(LightStage.One);
                    if (dlsModel.Sirens.Stage2Setting != null)
                        dlsModel.AvailableLightStages.Add(LightStage.Two);
                    if (dlsModel.Sirens.Stage3Setting != null)
                        dlsModel.AvailableLightStages.Add(LightStage.Three);

                    dlsModel.AvailableSirenStages.Add(SirenStage.Off);
                    if (dlsModel.SoundSettings.Tone1 != "")
                        dlsModel.AvailableSirenStages.Add(SirenStage.One);
                    if (dlsModel.SoundSettings.Tone2 != "")
                        dlsModel.AvailableSirenStages.Add(SirenStage.Two);
                    if (dlsModel.SoundSettings.Tone3 != "")
                        dlsModel.AvailableSirenStages.Add(SirenStage.Warning);
                    if (dlsModel.SoundSettings.Tone4 != "")
                        dlsModel.AvailableSirenStages.Add(SirenStage.Warning2);

                    string affectedModels = dlsModel.Models.Trim();
                    if (affectedModels.Length > 0) {
                        foreach (string affectedModel in affectedModels.Split(','))
                        {
                            Model model = new Model(affectedModel);
                            if (!dictModels.ContainsKey(model))
                            {
                                dictModels.Add(model, dlsModel);
                                ("Added: " + affectedModel + " from " + Path.GetFileName(file)).ToLog();
                            }
                            else
                            {
                                ("WARNING: Conflict in " + file).ToLog();
                            }
                        }
                    }
                    else
                    {
                        dictModels.Add(new Model(Path.GetFileNameWithoutExtension(file)), dlsModel);                        
                        ("Added: " + Path.GetFileNameWithoutExtension(file)).ToLog();
                    }
                }
                catch (Exception e)
                {
                    ("VCF IMPORT ERROR (" + Path.GetFileNameWithoutExtension(file) + "): " + e.Message).ToLog();
                    Game.LogTrivial("VCF IMPORT ERROR (" + Path.GetFileNameWithoutExtension(file) + "): " + e.Message);
                }
            }
            return dictModels;
        }

        public static Dictionary<string, TAgroup> GetAllTAgroups()
        {
            string path = @"Plugins\DLS\Traffic Advisory\";
            _ = new TAgroup();
            Dictionary<string, TAgroup> listTAgroups = new Dictionary<string, TAgroup>();
            foreach (string file in Directory.EnumerateFiles(path, "*.xml"))
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TAgroup));
                    StreamReader streamReader = new StreamReader(file);

                    TAgroup taGroup = (TAgroup)mySerializer.Deserialize(streamReader);
                    streamReader.Close();

                    string name = Path.GetFileNameWithoutExtension(file);

                    listTAgroups.Add(name, taGroup);
                    ("Added TAgroup: " + name).ToLog();
                }
                catch (Exception e)
                {
                    ("TAGROUP IMPORT ERROR (" + Path.GetFileNameWithoutExtension(file) + "): " + e.Message).ToLog();
                    Game.LogTrivial("TAGROUP IMPORT ERROR (" + Path.GetFileNameWithoutExtension(file) + "): " + e.Message);
                }
            }
            return listTAgroups;
        }

        public static EmergencyLighting GetEL(Vehicle veh, ActiveVehicle activeVeh = null)
        {
            DLSModel dlsModel = veh.GetDLS();
            if (activeVeh == null)
                activeVeh = veh.GetActiveVehicle();
            string name = veh.Model.Name + " | " + activeVeh.LightStage.ToString() + " | " + activeVeh.TAStage.ToString() + " | " + activeVeh.SBOn.ToString();
            uint key = Game.GetHashKey(name);
            EmergencyLighting eL;
            if (Entrypoint.UsedPool.Count > 0 && Entrypoint.UsedPool.ContainsKey(key))
            {
                eL = Entrypoint.UsedPool[key];
                ("Allocated \"" + name + "\" (" + key + ") for " + veh.Handle + " from Used Pool").ToLog();
            }
            else if (Entrypoint.AvailablePool.Count > 0)
            {
                eL = Entrypoint.AvailablePool[0];
                Entrypoint.AvailablePool.Remove(eL);
                ("Removed \"" + eL.Name + "\" from Available Pool").ToLog();
                ("Allocated \"" + name + "\" (" + key + ") for " + veh.Handle + " from Available Pool").ToLog();
            }
            else
            {
                if (EmergencyLighting.GetByName(name) == null)
                {
                    Model model = new Model("police");
                    eL = model.EmergencyLighting.Clone();
                    eL.Name = name;
                    ("Created \"" + name + "\" (" + key + ") for " + veh.Handle).ToLog();
                }
                else
                {
                    eL = EmergencyLighting.GetByName(name);
                    ("Allocated \"" + name + "\" (" + key + ") for " + veh.Handle + " from Game Memory").ToLog();
                }
            }
            if (activeVeh.LightStage != LightStage.Off && activeVeh.LightStage != LightStage.Empty)
            {
                switch (activeVeh.LightStage)
                {
                    case LightStage.One:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.Stage1Setting, eL);
                        break;
                    case LightStage.Two:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.Stage2Setting, eL);
                        break;
                    case LightStage.Three:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.Stage3Setting, eL);
                        break;
                    case LightStage.CustomOne:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.CustomStage1, eL);
                        break;
                    case LightStage.CustomTwo:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.CustomStage2, eL);
                        break;
                    default:
                        SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.Stage3Setting, eL);
                        break;
                }
            }
            else
            {
                SirenApply.ApplySirenSettingsToEmergencyLighting(dlsModel.Sirens.Stage3Setting, eL);
                eL.LeftHeadLightSequence = "00000000000000000000000000000000";
                eL.LeftTailLightSequence = "00000000000000000000000000000000";
                eL.RightHeadLightSequence = "00000000000000000000000000000000";
                eL.RightTailLightSequence = "00000000000000000000000000000000";
                for (int i = 0; i < eL.Lights.Length; i++)
                {
                    EmergencyLight eLig = eL.Lights[i];
                    eLig.FlashinessSequence = "00000000000000000000000000000000";
                    eLig.RotationSequence = "00000000000000000000000000000000";
                }
            }
            if (!Entrypoint.UsedPool.ContainsKey(key))
                Entrypoint.UsedPool.Add(key, eL);
            activeVeh.CurrentHash = key;
            return eL;
        }

        public static bool GetSirenKill(ActiveVehicle activeVehicle)
        {
            Vehicle veh = activeVehicle.Vehicle;
            if (veh)
            {
                DLSModel dlsModel = veh.GetDLS();
                if (dlsModel != null)
                {
                    bool _ = Entrypoint.SirenKill;
                    if (dlsModel.SoundSettings.SirenKillOverride.ToBoolean() == true)
                        _ = dlsModel.SoundSettings.SirenKillOverride.ToBoolean();
                    return _;
                }
            }
            return false;
        }
    }
}
