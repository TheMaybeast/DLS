using Rage;
using System;
using System.Collections.Generic;

namespace DLS
{
    public static class TextureHash
    {
        static TextureHash()
        {
            string[] textureNames = new string[]
            {
                "VehicleLight_car_oldsquare",
                "VehicleLight_car_standardmodern",
                "VehicleLight_car_standard70s",
                "VehicleLight_misc_searchlight",
                "VehicleLight_misc_squarelight",
                "VehicleLight_bicycle",
                "VehicleLight_car_LED1",
                "VehicleLight_car_LED2",
                "VehicleLight_bike_sport",
                "VehicleLight_bike_round",
                "VehicleLight_car_utility",
                "VehicleLight_car_antique",
                "VehicleLight_sirenlight"
            };

            foreach (string name in textureNames)
                LightTextureHashes[Game.GetHashKey(name)] = name;
        }

        public const string defaultLightTexture = "VehicleLight_sirenlight";
        public static Dictionary<uint, string> LightTextureHashes { get; } = new Dictionary<uint, string> { };

        public static uint StringToHash(string textureName)
        {
            try
            {
                return Convert.ToUInt32(textureName, 16);
            }
            catch (FormatException)
            {
                uint hash = Game.GetHashKey(textureName);
                if (!LightTextureHashes.ContainsKey(hash))
                    LightTextureHashes[hash] = textureName;
                return hash;
            }
        }

        public static string HashToString(uint textureHash)
        {
            if (LightTextureHashes.TryGetValue(textureHash, out string textureName))
                return textureName;
            else
                return string.Format("0x{0:X8}", textureHash);
        }
    }
}
