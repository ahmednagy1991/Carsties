using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data
{
    public class DBInitializer
    {
        public static async Task InitDB(WebApplication app)
        {
            await DB.InitAsync("SearchDB",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnection")));

            await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

            var count = await DB.CountAsync<Item>();
            if (count == 0)
            {
                Console.WriteLine("No items in database");
                var auctions = await File.ReadAllTextAsync("Data/auctions.json");
                var items = JsonSerializer.Deserialize<List<Item>>(auctions, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                await DB.SaveAsync<Item>(items);
            }
        }
    }
}