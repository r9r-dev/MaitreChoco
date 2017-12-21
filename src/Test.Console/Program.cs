using Discord;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using static System.Console;

namespace Test.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var discord = new DiscordSocketClient();
            discord.Log += DiscordOnLog;
            var token = File.ReadAllText("maitrechoco.token");
            await discord.LoginAsync(TokenType.Bot, token);
            await discord.StartAsync();


            await Task.Delay(Timeout.Infinite);
        }

        private static Task DiscordOnLog(LogMessage msg)
        {
            WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
