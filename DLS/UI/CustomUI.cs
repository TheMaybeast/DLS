using System.Xml.Serialization;

namespace DLS.UI
{
    [XmlRoot("UI")]
    public class CustomUI
    {
        [XmlElement("SWidth")]
        public string SWidth { get; set; } = "550";

        [XmlElement("SHeight")]
        public string SHeight { get; set; } = "220";

        [XmlElement("SOffsetX")]
        public string SOffsetX { get; set; } = "1920";

        [XmlElement("SOffsetY")]
        public string SOffsetY { get; set; } = "1080";

        public int Width { get; set; }

        public int Height { get; set; }

        public int OffsetX { get; set; }
        
        public int OffsetY { get; set; }
    }
}
