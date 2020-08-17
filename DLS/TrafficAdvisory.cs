using System.Collections.Generic;
using System.Xml.Serialization;

namespace DLS
{
    [XmlRoot("TAgroup")]
    public class TAgroup
    {
        [XmlElement("TApattern")]
        public List<TApattern> TaPatterns;
    }

    public class TApattern
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Three")]
        public TApatternNumber Three = new TApatternNumber();

        [XmlElement("Four")]
        public TApatternNumber Four = new TApatternNumber();

        [XmlElement("Five")]
        public TApatternNumber Five = new TApatternNumber();

        [XmlElement("Six")]
        public TApatternNumber Six = new TApatternNumber();
    }

    public class TApatternNumber
    {
        [XmlElement("Left")]
        public DirectionTAPattern Left = new DirectionTAPattern();

        [XmlElement("Diverge")]
        public DirectionTAPattern Diverge = new DirectionTAPattern();

        [XmlElement("Right")]
        public DirectionTAPattern Right = new DirectionTAPattern();

        [XmlElement("Warn")]
        public DirectionTAPattern Warn = new DirectionTAPattern();
    }

    public class DirectionTAPattern
    {
        [XmlElement("L")]
        public string L = "00000000000000000000000000000000";

        [XmlElement("EL")]
        public string EL = "00000000000000000000000000000000";

        [XmlElement("CL")]
        public string CL = "00000000000000000000000000000000";

        [XmlElement("C")]
        public string C = "00000000000000000000000000000000";

        [XmlElement("CR")]
        public string CR = "00000000000000000000000000000000";

        [XmlElement("ER")]
        public string ER = "00000000000000000000000000000000";

        [XmlElement("R")]
        public string R = "00000000000000000000000000000000";
    }
}