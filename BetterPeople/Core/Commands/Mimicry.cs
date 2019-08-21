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
        public async Task Become(IGuildUser User = null)
        {
            if (User == null)
            {
                await Context.Channel.SendMessageAsync("You need to specify a person");
                return;
            }
            if (User.IsBot)
            {
                await Context.Channel.SendMessageAsync("That one's already of a superior kind");
                return;
            }


            Targets.TargetList.Remove(Targets.TargetList.Find(x => x.ServerId == Context.Guild.Id));
            Targets.TargetList.Add(new Target(Context.Guild.Id, User));

            string become = "\\**smokebombs** now I became a better " + User.Mention;
            await Context.Channel.SendMessageAsync(become);

            IGuildUser Client = Context.Guild.GetUser(611889386874732544);

            await Client.ModifyAsync(x => x.Nickname = "Better " + User.Username);
/*
            #region rolesstuff
            List<IRole> roles = new List<IRole>();

            foreach (ulong id in Client.RoleIds)
                roles.Add(Context.Guild.GetRole(id));

            await Client.RemoveRolesAsync(roles);

            roles.Clear();

            foreach (ulong id in User.RoleIds)
                roles.Add(Context.Guild.GetRole(id));

            await Client.AddRolesAsync(roles);
#endregion*/
        }

        [Command("stopit"), Alias("staph"), Summary("Become yourself")]
        public async Task stop()
        {
            IGuildUser Client = Context.Guild.GetUser(611889386874732544);

            await Client.ModifyAsync(x => x.Nickname = Client.Username);
            await Context.Channel.SendMessageAsync("Task failed succesfully");
            Targets.TargetList.Remove(Targets.TargetList.Find(x => x.ServerId == Context.Guild.Id));
            /*
            List<IRole> roles = new List<IRole>();

            foreach (ulong id in Client.RoleIds)
                roles.Add(Context.Guild.GetRole(id));

            await Client.RemoveRolesAsync(roles);*/
        }
    }
}

