using System.Xml.Serialization;

namespace BoardGameGeek.RestAPI.Models
{

    public class BoardgameItem
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("yearpublished")]
        public BoardgameYearPublished YearPublished { get; set; }
        [XmlElement("name")]
        public BoardgameName Name { get; set; }
        [XmlElement("thumbnail")]
        public string SmallPictureUrl { get; set; }
    }
}
