namespace BoardGameGeek.RestAPI.Models
{
    public class BoardgameSearchInput
    {
        public string SearchString { get; set; }
        public BoardgameSearchInput()
        {
            SearchString = "";
        }
    }
}
