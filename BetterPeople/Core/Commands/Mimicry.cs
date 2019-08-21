using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using BetterPeople.Data;

namespace BetterPeople.Core.Commands
{

    public class Mimicry : ModuleBase<SocketCommandContext>
    {
        [Command("become"), Alias("copy"), Summary("Become someone")]
        public async Task Become(IUser User = null)
        {
            if (User == null)
            {
                await Context.Channel.SendMessageAsync("you need to specify a person");
                return;
            }
            if (User.IsBot)
            {
                await Context.Channel.SendMessageAsync("that one's already of a superior kind");
                return;
            }

            Targets.TargetList.Add(new Target(Context.Guild.Id, User));

            string become = "\\**smokebombs** now I became a better " + User.Mention;
            await Context.Channel.SendMessageAsync(become);
        }

        [Command("stopit"), Alias("staph"), Summary("Become yourself")]
        public async Task stop()
        {

            await Context.Channel.SendMessageAsync("Task failed succesfully");
            Targets.TargetList.Remove(Targets.TargetList.Find(x => x.ServerId == Context.Guild.Id));
        }
    }
}

