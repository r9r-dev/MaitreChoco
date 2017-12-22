using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CreativeGurus.Weather.Wunderground;
using CreativeGurus.Weather.Wunderground.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Cognitive.LUIS;
using static System.Console;

namespace bot.ConsoleClient
{
    class Program
    {
        private static DiscordSocketClient _client;
        private static WeatherClient _wu;
        private static LuisClient _luis;

        private static readonly string _discordtoken = File.ReadAllText("maitrechoco.token");
        private static readonly string _wutoken = File.ReadAllText("wunderground.token");
        private static readonly string _luistoken = File.ReadAllText("luis.token");

        static async Task Main(string[] args)
        {
            _client = new DiscordSocketClient();
            _wu = new WeatherClient(_wutoken);
            _luis = new LuisClient("ef487147-41bf-4514-9f70-1c539f24943c", _luistoken, true, "westeurope");

            _client.Log += DiscordOnLog;
            _client.MessageReceived += DiscordOnMessageReceived;
            
            await _client.LoginAsync(TokenType.Bot, _discordtoken);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private static async Task DiscordOnMessageReceived(SocketMessage msg)
        {
            if (msg.Author.Id == _client.CurrentUser.Id) return;

            if (msg.Channel.Name == "tests" || msg.Channel.Name == "chocolatines")
            {
                var prediction = await _luis.Predict(msg.Content);
                if (prediction.TopScoringIntent.Name == "GetWeather")
                {
                    if (prediction.Entities.ContainsKey("City"))
                    {
                        var city = prediction.Entities["City"].First().Value;
                        var observation = _wu.GetConditions(QueryType.GlobalCity, new QueryOptions() { Language = "FR", City = city, Country = "France" }).CurrentObservation;
                        
                        await msg.Channel.SendMessageAsync($"Il fait {observation.TempC:N0}°C à {observation.DisplayLocation.City}. Le temps est {observation.Weather.ToLower()}.");
                    }
                }
                else if (prediction.TopScoringIntent.Name == "Greetings")
                {
                    await msg.Channel.SendMessageAsync($"ChocoJour {msg.Author.Mention} !");
                }
                
            }
        }

        private static Task DiscordOnLog(LogMessage msg)
        {
            WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
