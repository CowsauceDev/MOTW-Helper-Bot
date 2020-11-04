using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;

namespace motw{
    public class Bot{
        public DiscordClient client { get; private set; }
        public CommandsNextModule commands;
        public async Task start() {
            var clientConfig = new DiscordConfiguration {
                Token = "[Place Bot Token Here]",
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                UseInternalLogHandler = true
                };

            var config = new CommandsNextConfiguration{
                StringPrefix = ".",
                CaseSensitive = false,
                EnableDefaultHelp = false,

            };


            client = new DiscordClient(clientConfig);
            commands = client.UseCommandsNext(config); 

            client.Ready += OnClientReady;
            commands.RegisterCommands<CommandModule>();

            await client.ConnectAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private async Task OnClientReady(ReadyEventArgs e){
            await client.UpdateStatusAsync(game: new DiscordGame(".help"));
        }
    }
}
