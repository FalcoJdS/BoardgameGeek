using System.Xml.Serialization;

namespace BoardGameGeek.RestAPI.Models
{
    public class BoardgameName
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
