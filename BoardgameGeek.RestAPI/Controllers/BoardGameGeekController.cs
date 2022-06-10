using BoardGameGeek.RestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace BoardGameGeek.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardGameGeekController : ControllerBase
    {

        private IHttpClientFactory _ClientFactory { get; }
        IConfiguration Configuration;
        private string BoardgameSearchUri;
        private string BoardgameByIdUri;

        public BoardGameGeekController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;
            string baseUri = Configuration["ConnectionStrings:BggRootPath"];
            string searchUri = Configuration["ConnectionStrings:BggSearchBaseUri"];
            string idUri = Configuration["ConnectionStrings:BggGetItemBaseUri"];
            BoardgameSearchUri = baseUri + searchUri;
            BoardgameByIdUri = baseUri + idUri;
            _ClientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<BoardgameSearchResponse?> Query(string searchString)
        {
            var boardgameResponses = new BoardgameSearchResponse();
            var request = new HttpRequestMessage(HttpMethod.Get, BoardgameSearchUri + "?query=" + searchString + "&type=boardgame");
            var client = _ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(responseStream))
                {
                    var ser = new XmlSerializer(typeof(BoardgameSearchResponse));
                    boardgameResponses = (BoardgameSearchResponse)ser.Deserialize(reader);
                }
                responseStream.Close();
                if (boardgameResponses != null && boardgameResponses.Boardgames.Any())
                {
                    foreach (var boardgame in boardgameResponses.Boardgames)
                    {
                        if (boardgame.YearPublished == null)
                        {
                            boardgame.YearPublished = new BoardgameYearPublished();
                        }
                        if (boardgame.YearPublished.Year == null)
                        {
                            boardgame.YearPublished.Year = "";
                        }
                    }
                }
                else
                {
                    boardgameResponses = null;
                }
            }
            return boardgameResponses;
        }


        [HttpGet("{id}")]
        public async Task<BoardgameItem?> GetItemById(string id)
        {
            var boardgameItem = new BoardgameItem();
            var boardgameResponses = new BoardgameSearchResponse();
            var request = new HttpRequestMessage(HttpMethod.Get, BoardgameByIdUri + "?id=" + id);
            var client = _ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(responseStream))
                {
                    var ser = new XmlSerializer(typeof(BoardgameSearchResponse));
                    boardgameResponses = (BoardgameSearchResponse)ser.Deserialize(reader);
                }
                responseStream.Close();
                if (boardgameResponses != null && boardgameResponses.Boardgames.Count() == 1)
                {
                    boardgameItem = boardgameResponses.Boardgames.First();
                }
                else
                {
                    boardgameItem = null;
                }
            }
            return boardgameItem;
        }

        [HttpPost]
        public async Task<List<BoardgameItem>> GetItemsById(List<string> ids)
        {
            var result = new List<BoardgameItem>();
            var searchString = "";
            foreach (var id in ids)
            {
                searchString += id + ',';
            }
            if (searchString.Length == 0)
            {
                return result;
            }

            searchString = searchString.Substring(0, searchString.Length - 1);
            var boardgameResponses = new BoardgameSearchResponse();
            var request = new HttpRequestMessage(HttpMethod.Get, BoardgameByIdUri + "?id=" + searchString);
            var client = _ClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(responseStream))
                {
                    var ser = new XmlSerializer(typeof(BoardgameSearchResponse));
                    boardgameResponses = (BoardgameSearchResponse)ser.Deserialize(reader);
                }
                responseStream.Close();
                if (boardgameResponses != null && boardgameResponses.Boardgames.Count() > 0)
                {
                    result = boardgameResponses.Boardgames;
                }
            }
            return result;
        }
    }
}

