using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestEasyBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("avoid")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task ShowQueue(ulong user)
        {

            if (Program.guildSettings.Exists (x=> x.GuildID == Context.Guild.Id))
            {
                Program.Settings st = Program.guildSettings.Single(x => x.GuildID == Context.Guild.Id);
                st.UserID = user;
            }
            else
            {
                Program.Settings st = new Program.Settings();
                st.GuildID = Context.Guild.Id;
                st.UserID = user;
                Program.guildSettings.Add(st);
            }
            await Context.Channel.SendMessageAsync("User is now getting avoided!");
        }
    }
}