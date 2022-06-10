using System.Xml.Serialization;

namespace BoardGameGeek.RestAPI.Models
{
    [XmlRoot(ElementName = "items")]
    public class BoardgameSearchResponse
    {
        [XmlElement(ElementName = "item")]
        public List<BoardgameItem> Boardgames{ get; set; }

        public BoardgameSearchResponse()
        {
            Boardgames = new List<BoardgameItem>();
        }

        public BoardgameItem this[string name] => Boardgames.FirstOrDefault(s => string.Equals(s.Id, name, StringComparison.OrdinalIgnoreCase));
    }
}
