using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBotproga.Models;

namespace telegramBotproga.Clients
{
    internal class ArtistClient
    {
        HttpClient _client;
       

        public ArtistClient()
        {

            _client = new HttpClient();

        }
        public async Task<ArtistModel> GetArtistAsync(string id)
        {
            ArtistModel? result = new ArtistModel();
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://localhost:7018/Artist?id={id}"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "application/json");
                request.Headers.TryAddWithoutValidation("Authorization", Constants.apikey);

                var response = await _client.SendAsync(request);
                var content = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<ArtistModel>(content);
                return result;
            }

        }
    }
}
