using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegramBotproga.Clients
{
    public class Mp3Client
    {

        //public async Task GetLyricsAsync(string text, string lang)
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri("https://shazam.p.rapidapi.com/search?term=kiss%20the%20rain&locale=en-US&offset=0&limit=5"),
        //        Headers =
        //        {
        //{ "X-RapidAPI-Key", "f9319d662dmsh714ab35a10bdf07p1faeb1jsnaf87613a4e69" },
        //{ "X-RapidAPI-Host", "shazam.p.rapidapi.com" },
        //        },
        //    };
        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine(body);
        //    }

        //public void SaveMp3File()
        //{
        //    //var apiKey = "<API key>";
        //    //var isSSL = false;
        //    //var text = "Hello, world!";
        //    //var lang = Languages.English_UnitedStates;

        //    //var voiceParams = new VoiceParameters(text, lang)
        //    //{
        //    //    AudioCodec = AudioCodec.MP3,
        //    //    AudioFormat = AudioFormat.Format_44KHZ.AF_44khz_16bit_stereo,
        //    //    IsBase64 = false,
        //    //    IsSsml = false,
        //    //    SpeedRate = 0
        //    //};

        //    //var voiceProvider = new VoiceProvider(apiKey, isSSL);
        //    //var voice = voiceProvider.Speech<byte[]>(voiceParams);

        //    //var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "voice.mp3");
        //    //File.WriteAllBytes(fileName, voice);
        //}
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async Task<int> GetMP3Async(string text)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://voicerss-text-to-speech.p.rapidapi.com/?key=4181cd8366824ac69566e1d48eb4e97e&src={text}&hl=en-us&r=0&c=mp3&f=8khz_8bit_mono"),
                Headers =
    {
        { "X-RapidAPI-Key", "f9319d662dmsh714ab35a10bdf07p1faeb1jsnaf87613a4e69" },
        { "X-RapidAPI-Host", "voicerss-text-to-speech.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsByteArrayAsync().Result;
                Random random = new Random();
                
                int a = random.Next(1,21121121);
                using (var stream = System.IO.File.OpenWrite($"D:/Proga Laby/telegramBotproga/telegramBotproga/Mp3Files/{a}.mp3"))
                stream.Write(content);
                return a;
            }
             

        }

    }

}