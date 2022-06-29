using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using telegramBotproga.Models;

namespace telegramBotproga.Clients
{
    public class LyricsClient
    {

        HttpClient _client;
        private static string _adress;
        private static string _apikey;

        public LyricsClient()
        {
            _adress = Constants.adress;
            _apikey = Constants.apikey;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
        }
        public async Task<LyricsModel>   GetLyricsAsync(string artist, string title)
        {
            LyricsModel? result = null;
            try
            {
                var responce = await _client.GetAsync($"/Lyrics?artist={artist}&title={title}");
                responce.EnsureSuccessStatusCode();
                var content = responce.Content.ReadAsStringAsync().Result;

                //convert to json
                result = JsonConvert.DeserializeObject<LyricsModel>(content);

                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR ERROR{e}");
                return result;
            }
             
        }
        
    }
}
