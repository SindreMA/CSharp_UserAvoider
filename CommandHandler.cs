using System;
using Discord;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Addons.EmojiTools;
using System.Text.RegularExpressions;
using System.Linq;

namespace TestEasyBot
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += _client_MessageReceived;
            _client.UserVoiceStateUpdated += _client_UserVoiceStateUpdated;

        }

        private async Task _client_MessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPost = 0;
            if (msg.HasCharPrefix('!', ref argPost))
            {
                var result = _service.ExecuteAsync(context, argPost, null);
                if (!result.Result.IsSuccess && result.Result.Error != CommandError.UnknownCommand && !result.Result.ErrorReason.Contains("Objektreferanse er ikke satt til en objektforekomst"))
                {
                    Program.Log(result.Result.ErrorReason, ConsoleColor.Red);
                }
                Program.Log("Invoked " + msg + " in " + context.Channel + " with " + result.Result, ConsoleColor.Magenta);
            }
            else
            {
                Program.Log(context.Channel + "-" + context.User.Username + " : " + msg, ConsoleColor.White);
            }
        }

        private Task _client_UserVoiceStateUpdated(SocketUser user, SocketVoiceState _old, SocketVoiceState _new)
        {
            SocketGuild guild = null;
            if (_old.VoiceChannel != null) guild = _old.VoiceChannel.Guild;
            if (_new.VoiceChannel != null) guild = _new.VoiceChannel.Guild;
            //Joins a support channel
            if (_new.VoiceChannel != null)
            {
                if (Program.guildSettings.Exists(x=> x.GuildID == guild.Id && x.UserID == user.Id))
                {
                    var listOfChannels = guild.VoiceChannels.ToList().Where(x => !x.Users.Any(c => c.Id == user.Id)).ToList();
                    Random rnd = new Random();
                    int r = rnd.Next(listOfChannels.Count());
                    var randomVoiceChannel = listOfChannels[r];

                    foreach (var item in _new.VoiceChannel.Users.Where(x=> x.Id != user.Id))
                    {
                        item.ModifyAsync(x => x.Channel = randomVoiceChannel);
                    }
                }
            }
            //Leaves a support channel
            if (_old.VoiceChannel != null)
            {

            }
            return null;
        }

    }
}
