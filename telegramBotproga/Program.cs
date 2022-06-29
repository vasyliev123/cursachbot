using telegramBotproga;
using telegramBotproga.Clients;
MyBot myBot = new MyBot();
await myBot.Start();

LyricsClient lyricsClient = new LyricsClient();
ArtistClient artistClient = new ArtistClient();
Console.WriteLine(await artistClient.GetArtistAsync("4CzUzn54Cp9TQr6a7JIlMZ"));
 
//Console.WriteLine(lyricsClient.GetLyricsAsync("Manowar", "Manowar").Result.Lyrics);
Console.ReadKey();