using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBotproga.Models;

namespace telegramBotproga.Clients
{
    internal class ArtistIdClient
    {
        HttpClient _client;
        public ArtistIdClient()
        {
            _client = new HttpClient();

        }
        public async Task<ArtistIdModel> ArtistIdGet(string artist)
        {
            ArtistIdModel? result = new ArtistIdModel();

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://localhost:7018/ArtistId?artist={artist}"))
            {
                 
                try
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    request.Headers.TryAddWithoutValidation("Authorization", Constants.apikey);
                    var response = await _client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var content = response.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<ArtistIdModel>(content);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return result;
                }
            }

        }
    }
}
