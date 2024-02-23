using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctoinServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctoinServiceHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Item>> GetItemsForSearchDB()
        {
            var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

            return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctoinServiceUrl"] + "/api/auctions?date=" + lastUpdated);
        }
    }
}