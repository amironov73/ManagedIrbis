using System.Xml.Serialization;

namespace UnitTests.AM.Xml
{
    [XmlRoot("canary")]
    public class CanaryClass
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("age")]
        public int Age { get; set; }
    }
}
