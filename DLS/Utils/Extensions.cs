using DLS.UI;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DLS.Utils
{
    internal static class Extensions
    {
        internal static void Draw(this Sprite sprite, Rage.Graphics graphics)
        {
            Texture texture = sprite.Texture;
            var origRes = Game.Resolution;
            float aspectRaidou = origRes.Width / (float)origRes.Height;
            PointF pos = new PointF(sprite.Position.X / (1080 * aspectRaidou), sprite.Position.Y / 1080f);
            SizeF siz = new SizeF(sprite.Size.Width / (1080 * aspectRaidou), sprite.Size.Height / 1080f);
            if (texture != null)
                graphics.DrawTexture(texture, pos.X * Game.Resolution.Width, pos.Y * Game.Resolution.Height, siz.Width * Game.Resolution.Width, siz.Height * Game.Resolution.Height);
        }
        internal static DLSModel GetDLS(this Vehicle veh)
        {
            if (!veh)
                return null;
            if (Entrypoint.DLSModelsDict.ContainsKey(veh.Model))
                return Entrypoint.DLSModelsDict[veh.Model];
            return null;
        }
        internal static ActiveVehicle GetActiveVehicle(this Vehicle veh)
        {
            if (!veh)
                return null;
            for (int i = 0; i < Entrypoint.activeVehicles.Count; i++)
            {
                if (Entrypoint.activeVehicles[i].Vehicle == veh)
                    return Entrypoint.activeVehicles[i];
            }
            return null;
        }
        internal static void ToLog(this string log)
        {
            if (Entrypoint.LogToConsole) Game.LogTrivial(log);

            string path = @"Plugins/DLS.log";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("[" + DateTime.Now.ToString() + "] " + log);
                writer.Close();
            }
        }
        internal static int ToInt32(this string text, [CallerMemberName] string callingMethod = null)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(text);
            }
            catch (Exception e)
            {
                string message = "ERROR: " + e.Message + " ( " + text + " ) [" + callingMethod + "() -> " + MethodBase.GetCurrentMethod().Name + "()]";
                (message).ToLog();
                Game.LogTrivial(message);
            }
            return i;
        }
        internal static float ToFloat(this string text, [CallerMemberName] string callingMethod = null)
        {
            float i = 0f;
            try
            {
                i = float.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                string message = "ERROR: " + e.Message + " ( " + text + " ) [" + callingMethod + "() -> " + MethodBase.GetCurrentMethod().Name + "()]";
                (message).ToLog();
                Game.LogTrivial(message);
            }
            return i;
        }
        internal static Color HexToColor(this string text, [CallerMemberName] string callingMethod = null)
        {
            Color i = new Color();
            try
            {
                i = ColorTranslator.FromHtml("#" + text);
            }
            catch (Exception e)
            {
                string message = "ERROR: " + e.Message + " ( " + text + " ) [" + callingMethod + "() -> " + MethodBase.GetCurrentMethod().Name + "()]";
                (message).ToLog();
                Game.LogTrivial(message);
            }
            return i;
        }
        internal static bool ToBoolean(this string text, [CallerMemberName] string callingMethod = null)
        {
            bool i = false;
            try
            {
                i = Convert.ToBoolean(text);
            }
            catch (Exception e)
            {
                string message = "ERROR: " + e.Message + " ( " + text + " ) [" + callingMethod + "() -> " + MethodBase.GetCurrentMethod().Name + "()]";
                (message).ToLog();
                Game.LogTrivial(message);
            }
            return i;
        }
        internal static bool IsPlayerVehicle(this Vehicle veh)
        {
            ActiveVehicle vehActive = veh.GetActiveVehicle();
            if (Game.LocalPlayer.Character.CurrentVehicle == veh || Game.LocalPlayer.Character.LastVehicle == veh)
                return true;
            foreach (ActiveVehicle activeVeh in Entrypoint.activeVehicles)
            {
                if (activeVeh == vehActive && activeVeh.PlayerVehicle)
                    return true;
            }
            return false;
        }
        internal static int GetIndexFromTAPatternName(this TAgroup taGroup, string name)
        {
            foreach (TApattern taP in taGroup.TaPatterns)
            {
                if (taP.Name == name)
                    return taGroup.TaPatterns.IndexOf(taP);
            }
            return 999;
        }
        internal static bool DoesVehicleHaveLightStage(this DLSModel dlsModel, LightStage lightStage)
        {
            return dlsModel.AvailableLightStages.Contains(lightStage);
        }
        internal static bool DoesVehicleHaveSirenStage(this DLSModel dlsModel, SirenStage sirenStage)
        {
            return dlsModel.AvailableSirenStages.Contains(sirenStage);
        }

        internal static T Next<T>(this List<T> list, T currentItem)
        {
            int index = list.IndexOf(currentItem);
            index = (index + 1) % list.Count;
            return list[index];
        }

        internal static T Previous<T>(this List<T> list, T currentItem)
        {
            int index = list.IndexOf(currentItem);
            index = (list.Count + index - 1) % list.Count;
            return list[index];
        }

        internal static SirenStage NextSirenStage(this List<SirenStage> list, SirenStage currentItem, bool includeFirst = true)
        {
            if (!list.Contains(currentItem))
                return currentItem;
            int index = list.IndexOf(currentItem) + 1;
            if (index > list.Count - 1)
            {
                if (includeFirst)
                    index = 0;
                else
                    index = 1;
            }
            return list[index];
        }

        public static List<List<T>> Chunk<T>(IEnumerable<T> data, int numArrays)
        {
            var size = data.Count() / numArrays;
            return data
                  .Select((x, i) => new { Index = i, Value = x })
                  .GroupBy(x => x.Index / size)
                  .Select(x => x.Select(v => v.Value).ToList())
                  .ToList();
        }
    }
}
