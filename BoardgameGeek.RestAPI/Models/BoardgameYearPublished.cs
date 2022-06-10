using System.Xml.Serialization;

namespace BoardGameGeek.RestAPI.Models
{
    public class BoardgameYearPublished
    {
        [XmlAttribute("value")]
        public string Year { get; set; }
    }
}