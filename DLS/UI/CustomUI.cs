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

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
