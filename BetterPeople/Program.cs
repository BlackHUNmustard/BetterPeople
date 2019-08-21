using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using BetterPeople.Data;


namespace BetterPeople
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
        =>  new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            string Token = "";
            using (var ReadToken = new StreamReader(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.txt"))))
                Token = ReadToken.ReadToEnd();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task Client_Log(LogMessage Message)
        {
            Console.WriteLine($"{DateTime.Now} @ {Message.Source}] {Message.Message}");
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task Client_Ready()
        {
        }

        private async Task Client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);


            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;
            if (!Message.HasStringPrefix("bp.", ref ArgPos))
            {
                Target localtarget = Targets.TargetList.Find(x => x.ServerId == Context.Guild.Id);
                if (Targets.TargetList.Exists(x => x.ServerId == Context.Guild.Id))
                    if (Targets.TargetList.Exists(x => x.ClientId == Context.User.Id))
                        await Context.Channel.SendMessageAsync(Context.Message.Content);
                return;
            }


            var Result = await Commands.ExecuteAsync(Context, ArgPos, null);

            if(!Result.IsSuccess)
                Console.WriteLine($"{DateTime.Now} @ Commands] Command execution failure [{Context.Message.Content} | {Result.ErrorReason}]");
        }
    }
}
