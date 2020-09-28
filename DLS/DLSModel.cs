using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Drawing;

namespace DLS
{
    [XmlRoot("Model")]
    public class DLSModel
    {
        [XmlElement("Models")]
        public string Models { get; set; } = "";

        [XmlElement("SpecialModes")]
        public SpecialModes SpecialModes { get; set; } = new SpecialModes();

        [XmlElement("SoundSettings")]
        public SoundSettings SoundSettings { get; set; } = new SoundSettings();

        [XmlElement("TrafficAdvisory")]
        public TrafficAdvisory TrafficAdvisory { get; set; } = new TrafficAdvisory();

        [XmlElement("Extras")]
        public StageExtras StageExtras { get; set; } = new StageExtras();

        [XmlElement("Sirens")]
        public Sirens Sirens { get; set; } = new Sirens();

        public List<LightStage> AvailableLightStages { get; set; }

        public List<SirenStage> AvailableSirenStages { get; set; }
    }

    public class TrafficAdvisory
    {
        [XmlElement("Type")]
        public string Type = "off";

        [XmlElement("DivergeOnly")]
        public string DivergeOnly = "false";

        [XmlElement("AutoEnableStages")]
        public string AutoEnableStages;

        [XmlElement("DefaultEnabledDirection")]
        public string DefaultEnabledDirection = "diverge";

        [XmlElement("AutoDisableStages")]
        public string AutoDisableStages;

        [XmlElement("TAgroup")]
        public string TAgroup = "default";

        [XmlElement("DefaultTApattern")]
        public string DefaultTApattern = "default";

        [XmlElement("EnvColor")]
        public SirenColor Color { get; set; }

        [XmlElement("Sirens")]
        public string Sirens { get; set; } = "";

        // Will be deprecated in DLS v2.0, utilize Sirens instead.
        [XmlElement("L")]
        public string l;

        [XmlElement("EL")]
        public string el;

        [XmlElement("CL")]
        public string cl;

        [XmlElement("C")]
        public string c;

        [XmlElement("CR")]
        public string cr;

        [XmlElement("ER")]
        public string er;

        [XmlElement("R")]
        public string r;
    }

    public class SirenColor
    {
        [XmlIgnore]
        public Color ColorValue { get; set; }

        [XmlText]
        public string ColorString
        {
            get => string.Format("0x{0:X8}", ColorValue.ToArgb());
            set
            {
                ColorValue = Color.FromArgb(Convert.ToInt32(value, 16));
            }
        }
    }

    public class WailSetup
    {
        [XmlElement("WailSetupEnabled")]
        public string WailSetupEnabled = "false";

        [XmlElement("WailLightStage")]
        public string WailLightStage;

        [XmlElement("WailSirenTone")]
        public string WailSirenTone;
    }

    public class Sirens
    {
        [XmlElement("Stage1")]
        public SirenSetting Stage1Setting { get; set; }

        [XmlElement("Stage2")]
        public SirenSetting Stage2Setting { get; set; }

        [XmlElement("Stage3")]
        public SirenSetting Stage3Setting { get; set; }

        [XmlElement("CustomStage1")]
        public SirenSetting CustomStage1 { get; set; }

        [XmlElement("CustomStage2")]
        public SirenSetting CustomStage2 { get; set; }
    }

    public class StageExtras
    {
        [XmlArray("Off")]
        [XmlArrayItem("Extra")]
        public ExtraState[] OffExtras { get; set; } = new ExtraState[0];

        [XmlArray("TAEnabled")]
        [XmlArrayItem("Extra")]
        public ExtraState[] TAExtras { get; set; } = new ExtraState[0];

        [XmlArray("SBEnabled")]
        [XmlArrayItem("Extra")]
        public ExtraState[] SBExtras { get; set; } = new ExtraState[0];

        [XmlArray("Stage1")]
        [XmlArrayItem("Extra")]
        public ExtraState[] Stage1Extras { get; set; } = new ExtraState[0];

        [XmlArray("Stage2")]
        [XmlArrayItem("Extra")]
        public ExtraState[] Stage2Extras { get; set; } = new ExtraState[0];

        [XmlArray("Stage3")]
        [XmlArrayItem("Extra")]
        public ExtraState[] Stage3Extras { get; set; } = new ExtraState[0];

        [XmlArray("CustomStage1")]
        [XmlArrayItem("Extra")]
        public ExtraState[] CustomStage1Extras { get; set; } = new ExtraState[0];

        [XmlArray("CustomStage2")]
        [XmlArrayItem("Extra")]
        public ExtraState[] CustomStage2Extras { get; set; } = new ExtraState[0];
    }

    public class ExtraState
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }
    }

    public class SoundSettings
    {
        [XmlElement("Tone1")]
        public string Tone1 { get; set; } = "";

        [XmlElement("Tone2")]
        public string Tone2 { get; set; } = "";

        [XmlElement("Tone3")]
        public string Tone3 { get; set; } = "";

        [XmlElement("Tone4")]
        public string Tone4 { get; set; } = "";

        [XmlElement("Horn")]
        public string Horn { get; set; } = "";

        [XmlElement("AirHornInterruptsSiren")]
        public string AirHornInterruptsSiren { get; set; } = "false";

        [XmlElement("SirenKillOverride")]
        public string SirenKillOverride { get; set; } = "false";
    }

    public class SpecialModes
    {
        [XmlElement("SirenUI")]
        public string SirenUI { get; set; }

        [XmlElement("PresetSirenOnLeaveVehicle")]
        public string PresetSirenOnLeaveVehicle { get; set; }

        [XmlElement("WailSetup")]
        public WailSetup WailSetup { get; set; } = new WailSetup();

        [XmlElement("SteadyBurn")]
        public SteadyBurn SteadyBurn { get; set; } = new SteadyBurn();

        [XmlElement("LSAIYield")]
        public LSAIYield LSAIYield { get; set; } = new LSAIYield();
    }

    public class LSAIYield
    {
        [XmlElement("Stage1Yield")]
        public string Stage1Yield { get; set; } = "false";

        [XmlElement("Stage2Yield")]
        public string Stage2Yield { get; set; } = "false";

        [XmlElement("Stage3Yield")]
        public string Stage3Yield { get; set; } = "true";

        [XmlElement("Custom1Yield")]
        public string Custom1Yield { get; set; } = "false";

        [XmlElement("Custom2Yield")]
        public string Custom2Yield { get; set; } = "false";
    }

    public class SteadyBurn
    {
        [XmlElement("SteadyBurnEnabled")]
        public string SteadyBurnEnabled { get; set; } = "false";

        [XmlElement("Pattern")]
        public string Pattern { get; set; }

        [XmlElement("Sirens")]
        public string Sirens { get; set; }

        [XmlElement("EnvColor")]
        public SirenColor Color { get; set; }
    }
}