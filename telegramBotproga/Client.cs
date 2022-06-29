using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using System.Diagnostics;
using Newtonsoft.Json;
namespace telegramBotproga
{
	public class Client
	{
		public Client(string artist,string title, ITelegramBotClient botclient,Message message)
        {
			Artist = artist;
			Title = title;
			botClient = botclient;
			Message = message;
        }
		public static string? Artist { get; set; }
		public static string? Title { get; set; }
		public static Message? Message { get; set; }
		public static ITelegramBotClient? botClient { get; set; }
		HttpClient client = new HttpClient();
		
		
		public async Task GetSongsLirycs()
        {
			var response = await client.GetAsync($"https://api.lyrics.ovh/v1/{Artist}/{Title}");
			response.EnsureSuccessStatusCode();
			Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			Debug.Assert(botClient != null);
			Debug.Assert(Message != null);

			var json = JsonConvert.DeserializeObject<DeserializeResult>(response.Content.ReadAsStringAsync().Result);

			Console.WriteLine(json.Lyrics);
			await botClient.SendTextMessageAsync(Message.Chat.Id, json.Lyrics);

        }

	}
}

