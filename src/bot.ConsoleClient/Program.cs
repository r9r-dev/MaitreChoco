using System;
using System.Collections.Generic;
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
        private static WordGenerator _wg = new WordGenerator();

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

            await Task.Run(async () =>
            {
                ReadKey();
                await _client.StopAsync();
                _client.Dispose();
            });

            await Task.Delay(Timeout.Infinite);
        }

        private static async Task DiscordOnMessageReceived(SocketMessage msg)
        {
            if (msg.Author.Id == _client.CurrentUser.Id) return;

            if (msg.Content.ToLower().Contains("merci") && msg.MentionedUsers.Any(x => x.Id == _client.CurrentUser.Id))
            {
                await msg.Channel.SendMessageAsync($"Ya pas de quoi :yum:");
                return;
            }

            var cleanMsg = msg.Content;
            foreach (var tag in msg.Tags)
            {
                cleanMsg = cleanMsg.Replace($"<@{tag.Key}>", "");
            }

            if (msg.Channel.Name == "tests" || msg.Channel.Name == "chocolatines")
            {
                var prediction = await _luis.Predict(cleanMsg);

                WriteLine($"{msg} ({prediction.TopScoringIntent.Name})");

                switch (prediction.TopScoringIntent.Name)
                {
                    case "GetWeather":
                        if (prediction.Entities.ContainsKey("City"))
                        {
                            var city = prediction.Entities["City"].First().Value;
                            var observation = _wu.GetConditions(QueryType.GlobalCity, new QueryOptions() { Language = "FR", City = city, Country = "France" }).CurrentObservation;

                            await msg.Channel.SendMessageAsync($"Il fait {observation.TempC:N0}°C à {observation.DisplayLocation.City}. Le temps est {observation.Weather.ToLower().Replace("pluie", "pluvieux")}.");

                        }

                        break;
                    case "CreateWord":
                        var lang = Language.FR;
                        var langue = "français";
                        if (prediction.Entities.ContainsKey("WordLanguage"))
                        {
                            langue = prediction.Entities["WordLanguage"].First().Value;
                            switch (prediction.Entities["WordLanguage"].First().Value)
                            {
                                case "espagnol":
                                    lang = Language.ES;
                                    break;
                                case "français":
                                    lang = Language.FR;
                                    break;
                                case "italien":
                                    lang = Language.IT;
                                    break;
                                case "suédois":
                                    lang = Language.SE;
                                    break;
                                case "anglais":
                                    lang = Language.EN;
                                    break;
                                default:
                                    await msg.Channel.SendMessageAsync($"Je ne connais pas la langue \"{langue}\". Je connais l'espagnol, le français, l'italien, le suédois et l'anglais.");
                                    langue = string.Empty;
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(langue))
                        {
                            if (prediction.Entities.ContainsKey("LettersQty"))
                            {
                                var qty = int.Parse(prediction.Entities["LettersQty"].First().Value);
                                if (qty < 3) await msg.Channel.SendMessageAsync($"Je préfère éviter d'inventer des mots de moins de 3 lettres.");
                                else await msg.Channel.SendMessageAsync($"J'ai inventé le mot {langue} {_wg.GetWord(lang, qty)}.");
                            }
                            else
                            {
                                await msg.Channel.SendMessageAsync($"J'ai inventé le mot {langue} {_wg.GetWord(lang)}.");
                            }
                        }
                        break;
                    case "Greetings":
                        await msg.Channel.SendMessageAsync($"ChocoJour {msg.Author.Mention} !");
                        break;
                    case "GoodNight":
                        await msg.Channel.SendMessageAsync($"Bonne nuit {msg.Author.Mention} !");
                        break;
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
