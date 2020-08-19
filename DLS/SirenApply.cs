namespace DLS
{
    using DLS.Utils;
    using Rage;
    internal static class SirenApply
    {
        public static void ApplySirenSettingsToEmergencyLighting(SirenSetting setting, EmergencyLighting els)
        {
            els.TimeMultiplier = setting.TimeMultiplier;
            els.LightFalloffMax = setting.LightFalloffMax;
            els.LightFalloffExponent = setting.LightFalloffExponent;
            els.LightInnerConeAngle = setting.LightInnerConeAngle;
            els.LightOuterConeAngle = setting.LightOuterConeAngle;
            els.LightOffset = setting.LightOffset;
            els.TextureHash = setting.TextureHash;
            els.SequencerBpm = setting.SequencerBPM;
            els.UseRealLights = setting.UseRealLights;
            els.LeftHeadLightSequence = setting.LeftHeadLightSequencer;
            els.LeftHeadLightMultiples = setting.LeftHeadLightMultiples;
            els.RightHeadLightSequence = setting.RightHeadLightSequencer;
            els.RightHeadLightMultiples = setting.RightHeadLightMultiples;
            els.LeftTailLightSequence = setting.LeftTailLightSequencer;
            els.LeftTailLightMultiples = setting.LeftTailLightMultiples;
            els.RightTailLightSequence = setting.RightTailLightSequencer;
            els.RightTailLightMultiples = setting.RightTailLightMultiples;

            for (int i = 0; i < setting.Sirens.Length; i++)
            {
                SirenEntry entry = setting.Sirens[i];
                EmergencyLight light = els.Lights[i];

                // Main light settings                
                light.Color = entry.LightColor;
                light.Intensity = entry.Intensity;
                light.LightGroup = entry.LightGroup;
                light.Rotate = entry.Rotate;
                light.Scale = entry.Scale;
                light.ScaleFactor = entry.ScaleFactor;
                light.Flash = entry.Flash;
                light.SpotLight = entry.SpotLight;
                light.CastShadows = entry.CastShadows;
                light.Light = entry.Light;

                // Corona settings
                light.CoronaIntensity = entry.Corona.CoronaIntensity;
                light.CoronaSize = entry.Corona.CoronaSize;
                light.CoronaPull = entry.Corona.CoronaPull;
                light.CoronaFaceCamera = entry.Corona.CoronaFaceCamera;

                // Rotation settings
                light.RotationDelta = entry.Rotation.DeltaDeg;
                light.RotationStart = entry.Rotation.StartDeg;
                light.RotationSpeed = entry.Rotation.Speed;
                light.RotationSequence = entry.Rotation.Sequence;
                light.RotationMultiples = entry.Rotation.Multiples;
                light.RotationDirection = entry.Rotation.Direction;
                light.RotationSynchronizeToBpm = entry.Rotation.SyncToBPM;

                // Flash settings
                light.FlashinessDelta = entry.Flashiness.DeltaDeg;
                light.FlashinessStart = entry.Flashiness.StartDeg;
                light.FlashinessSpeed = entry.Flashiness.Speed;
                light.FlashinessSequenceRaw = entry.Flashiness.Sequence;
                light.FlashinessMultiples = entry.Flashiness.Multiples;
                light.FlashinessDirection = entry.Flashiness.Direction;
                light.FlashinessSynchronizeToBpm = entry.Flashiness.SyncToBPM;
            }
        }
    }
}