﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
namespace TestEasyBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();
        private CommandHandler _handler;
        private DiscordSocketClient _client;
        public class Settings
        {
            public ulong GuildID { get; set; }
            public ulong UserID { get; set; }
        }
        public static List<Settings> guildSettings = new List<Settings>();
        public async Task StartAsync()
        {

            Log("Starting up the bot", ConsoleColor.Green);

            _client = new DiscordSocketClient();
            new CommandHandler(_client);
            Log("Logging in...", ConsoleColor.Green);
            await _client.LoginAsync(TokenType.Bot, "#################################################");
            Log("Connecting...", ConsoleColor.Green);
            _client.GuildMembersDownloaded += _client_GuildMembersDownloaded;

            await _client.StartAsync();

            await Task.Delay(-1);

            _handler = new CommandHandler(_client);

        }

        private Task _client_GuildMembersDownloaded(SocketGuild arg)
        {
            Log(arg.Name + " connected!", ConsoleColor.Green);
            return null;
        }



        public static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + " : " + message, color);
            Console.ResetColor();
        }

    }
    
}
