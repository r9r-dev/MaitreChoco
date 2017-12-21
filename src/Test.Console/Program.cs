using Discord;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using static System.Console;

namespace Test.Console
{
    class Program
    {
        private static DiscordSocketClient _client;
        
        static async Task Main(string[] args)
        {
            _client = new DiscordSocketClient();
            _client.Log += DiscordOnLog;
            _client.MessageReceived += DiscordOnMessageReceived;
            var token = File.ReadAllText("maitrechoco.token");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private static async Task DiscordOnMessageReceived(SocketMessage msg)
        {
            if (msg.Author.Id == _client.CurrentUser.Id) return;
            
            if (msg.MentionedUsers.Any(x => x.Id == _client.CurrentUser.Id))
            {
                string message = msg.Content.Replace($"{_client.CurrentUser.Id}", msg.Author.Id.ToString());
                await msg.Channel.SendMessageAsync(message);
            }
        }

        private static Task DiscordOnLog(LogMessage msg)
        {
            WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
