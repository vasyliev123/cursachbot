using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using Newtonsoft.Json;
 
using telegramBotproga.Clients;
using telegramBotproga.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace telegramBotproga
{
    public class Temp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class MyBot
    {
        TelegramBotClient botClient = new TelegramBotClient("5298590715:AAFDG61W0e6goDcCSCrx37cIDFTMhB9LeQk");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        static List<Temp> output;


         
        static Dictionary<string, List<string>> dictionary ;
        LyricsClient lyricsClient1 = new LyricsClient();
        public async Task Start()
        {
           dictionary = new Dictionary<string, List<string>>();
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
             

            Console.WriteLine($"Bot {botMe.Username} is working");
            Console.ReadKey();
        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

         
      

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(dictionary.ContainsKey(update.Message.Chat.Id.ToString()))
            if(update.Type == UpdateType.Message && update?.Message?.Text != null&& dictionary[update.Message.Chat.Id.ToString()][0] == "Waiting for artist"&& update.Message.Text.Contains("/"))
            {
                    
                        var a = update.Message.Text.Split("/");
                        var lyrics = lyricsClient1.GetLyricsAsync(a[0], a[1]).Result.Lyrics;
                    dictionary[update.Message.Chat.Id.ToString()][1] = a[0];
                    dictionary[update.Message.Chat.Id.ToString()][2] = a[1];
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, lyrics, replyMarkup: SearchLyricsReplyKeyboard());
                        return;
            }
            if (dictionary.ContainsKey(update.Message.Chat.Id.ToString()))
                if (update.Type == UpdateType.Message && update?.Message?.Text != null && dictionary[update.Message.Chat.Id.ToString()][0] == "text to Mp3")
            {
                    //https://drive.google.com/drive/folders/1Myw5G-0FtZlq4DXXbAzC8jLMMoJzNzx8?usp=sharing
                    //https://drive.google.com/file/d/1bwfoFR31T-m96w62j-EkeQGAk-c6aYZq/view?usp=sharing
                    Mp3Client mp3Client = new Mp3Client();
                var a = await mp3Client.GetMP3Async(update?.Message?.Text);
                var fileName = $"D:/Proga Laby/telegramBotproga/telegramBotproga/Mp3Files/{a}.mp3";
                string path =  fileName;

                var fi1 = new FileInfo(path);
                Console.WriteLine(path);
                if (fi1.Exists)
                {
                        //await botClient.Send (update.Message.Chat.Id, audio: "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3");
                        //await botClient.SendVoiceAsync(update.Message.Chat.Id, voice: "/a.mp3");
                        using (var stream = System.IO.File.OpenRead($"D:/Proga Laby/telegramBotproga/telegramBotproga/Mp3Files/{a}.mp3"))
                        {
                            await botClient.SendAudioAsync(update.Message.Chat.Id, audio: stream,replyMarkup:ConvertReplyKeyboard());
                            dictionary[update.Message.Chat.Id.ToString()][0] = "text to Mp32";
                        }
                }
                    
                
                return;
            }
            if (dictionary.ContainsKey(update.Message.Chat.Id.ToString()))
                if (update.Type == UpdateType.Message && update?.Message?.Text != null && dictionary[update.Message.Chat.Id.ToString()][0] == "artist page")
                {
                    ArtistIdClient artistIdClient = new ArtistIdClient();
                    ArtistClient artistClient = new ArtistClient();
                    string id = artistIdClient.ArtistIdGet(update.Message.Text).Result.Artists.Items[0].Id;
                    var genres = artistIdClient.ArtistIdGet(update.Message.Text).Result.Artists.Items[0].Genres;
                    var albums = artistClient.GetArtistAsync(id).Result.Items;
                    var url = artistIdClient.ArtistIdGet(update.Message.Text).Result.Artists.Items[0].Images[0].Url;
                    var name = artistIdClient.ArtistIdGet(update.Message.Text).Result.Artists.Items[0].Name;
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("====================\n");
                    stringBuilder.Append($"\tInfo about {name}\n");
                    stringBuilder.Append("====================\n");
                    stringBuilder.Append("Artists genres:\n");
                    stringBuilder.Append("====================\n");
                    for (int i = 0; i < genres.Count-1; i++)
                    {
                        stringBuilder.Append(genres[i]+"\n");
                    }

                    stringBuilder.Append("====================\n");
                    stringBuilder.Append("Artists albums:\n");
                    stringBuilder.Append("====================\n");
                    for (int i = 0; i < albums.Count-1; i++)
                    {
                        stringBuilder.Append(albums[i].Name + "\n");
                    }
                    stringBuilder.Append("====================\n");
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, stringBuilder.ToString());
                    await botClient.SendPhotoAsync(update.Message.Chat.Id, photo: url,replyMarkup: ArtistReplyKeyboard());
                    dictionary[update.Message.Chat.Id.ToString()][0] = "post artist page";
                    return;
                }

            
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
            

            if (update?.Type == UpdateType.CallbackQuery)
            {
                await HandlerCallbackQuery(botClient, update.CallbackQuery);
            }
            

            
        }
        

        
        private async Task HandlerCallbackQuery(ITelegramBotClient botClient, CallbackQuery? callbackQuery)
        {
             
        }
        //
        private ReplyKeyboardMarkup MainmenuReplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {

                new KeyboardButton[] { "Search Lyrics","Search artist" },

               
                new KeyboardButton[] { "Convert text to Mp3" }

            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        private ReplyKeyboardMarkup ConvertReplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {

                new KeyboardButton[] { "Convert more","💤" },


               

            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        private ReplyKeyboardMarkup ArtistReplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {

                new KeyboardButton[] { "Continue Searching","💤" },
 

            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        private ReplyKeyboardMarkup SearchLyricsReplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                 new KeyboardButton[] { "Search more Lyrics" ,"💤"},
                 

                 

            })
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
        private ReplyKeyboardRemove RemoveReplyKeyboard()
        {

            ReplyKeyboardRemove replyKeyboardRemove = new ReplyKeyboardRemove();
            return replyKeyboardRemove;
        }
        private static string status = "Main menu";
        public static string artist = "";
        public static string title = "";
        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
               Console.Write(message.Text);
                if (!dictionary.ContainsKey(message.Chat.Id.ToString()))
                {
                    var list = new List<string>() { "Main menu","",""};
                    dictionary.Add(message.Chat.Id.ToString(),list);

                    using (StreamWriter writer = new StreamWriter("./users.json", false))
                    {
                        writer.Write(JsonConvert.SerializeObject(dictionary));

                    }

                }
                else
                {
                    var list = new List<string>() { "Main menu", "", "" };
                    dictionary[message.Chat.Id.ToString()] = list;

                    using (StreamWriter writer = new StreamWriter("./users.json", false))
                    {
                        writer.Write(JsonConvert.SerializeObject(dictionary));

                    }
                }

                await botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to main menu! Choose an option:", replyMarkup: MainmenuReplyKeyboard());
                return;
            }

            if(dictionary.ContainsKey(message.Chat.Id.ToString()))
            if((message.Text == "Search Lyrics" && dictionary[message.Chat.Id.ToString()][0] == "Main menu")||
                    (message.Text == "Search more Lyrics" && dictionary[message.Chat.Id.ToString()][0] == "Waiting for artist"))
            {
                dictionary[message.Chat.Id.ToString()][0] = "Waiting for artist";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter song artist and title (ex: artist/title):");
            }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Add to favorites" && dictionary[message.Chat.Id.ToString()][0] == "Waiting for artist")){
                    
                    MongoDBModel mongoDBModel = new MongoDBModel { ID = message.Chat.Id.ToString(), artist = dictionary[message.Chat.Id.ToString()][1], title= dictionary[message.Chat.Id.ToString()][1] };
                    Console.WriteLine(mongoDBModel);
                    MongoDBClient mongoDBClient = new MongoDBClient();

                    mongoDBClient.dbCollection.InsertOne(mongoDBModel);
                    
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Added to favorites!", replyMarkup: SearchLyricsReplyKeyboard());

                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Favorites" && dictionary[message.Chat.Id.ToString()][0] == "Main menu"))
                {

                    MongoDBModel mongoDBModel = new MongoDBModel { ID = message.Chat.Id.ToString(), artist = dictionary[message.Chat.Id.ToString()][1], title = dictionary[message.Chat.Id.ToString()][1] };
                    Console.WriteLine(mongoDBModel);
                    MongoDBClient mongoDBClient = new MongoDBClient();

                    mongoDBClient.dbCollection.InsertOne(mongoDBModel);

                    await botClient.SendTextMessageAsync(message.Chat.Id, "Added to favorites!", replyMarkup: SearchLyricsReplyKeyboard());

                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "💤" && dictionary[message.Chat.Id.ToString()][0] == "Waiting for artist")){
                    dictionary[message.Chat.Id.ToString()][0] = "Main menu";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to main menu! Choose an option:", replyMarkup: MainmenuReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Search artist" && dictionary[message.Chat.Id.ToString()][0] == "Main menu"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "artist page";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Enter Artist you want to know more about:", replyMarkup: RemoveReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Continue Searching" && dictionary[message.Chat.Id.ToString()][0] == "post artist page"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "artist page";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Enter Artist you want to know more about:", replyMarkup: RemoveReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "💤" && dictionary[message.Chat.Id.ToString()][0] == "post artist page"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "Main menu";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to main menu! Choose an option:", replyMarkup: MainmenuReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Convert text to Mp3" && dictionary[message.Chat.Id.ToString()][0] == "Main menu"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "text to Mp3";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Enter text you want to convert:", replyMarkup: RemoveReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "💤" && dictionary[message.Chat.Id.ToString()][0] == "text to Mp32"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "Main menu";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Welcome to main menu! Choose an option:", replyMarkup: MainmenuReplyKeyboard());
                }
            if (dictionary.ContainsKey(message.Chat.Id.ToString()))
                if ((message.Text == "Convert more" && dictionary[message.Chat.Id.ToString()][0] == "text to Mp32"))
                {
                    dictionary[message.Chat.Id.ToString()][0] = "text to Mp3";
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Enter text you want to convert:", replyMarkup: RemoveReplyKeyboard());
                }

        }
    }
}
