using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace motw{
    public class CommandModule {
        [Command("ping")] 
        [Description("Returns the bot's ping.")] 
        [Aliases("latency")] 
        public async Task Ping(CommandContext ctx) 
        {
            await ctx.TriggerTypingAsync();

            var embed = new DiscordEmbedBuilder{
                Title = "Pong!",
                Description = $"{DiscordEmoji.FromName(ctx.Client, ":ping_pong:")} The bot's ping is currently {ctx.Client.Ping}ms",
                Color = DiscordColor.Red
            };

            embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
            await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("help")] 
        [Description("Sends you some help!")] 
        [Aliases("?")] 
        public async Task Help(CommandContext ctx) 
        {
            await ctx.TriggerTypingAsync();

            var embed = new DiscordEmbedBuilder{
                Title = "Heres some commands you can use to help out during the game!",
                Color = DiscordColor.Red
            };

            embed.AddField(name: "help", value: "Brings up this message!\n(usage: `.help`, aliases: `.?`)",inline: false);
            embed.AddField(name: "ping", value: "Shows the latency of the bot.\n(usage: `.ping`, aliases: `.latency`)",inline: false);
            embed.AddField(name: "roll", value: "Rolls two d6 to give a random result! Does NOT add any rating modifiers.\n(usage: `.roll`, aliases: `.r`)",inline: false);
            embed.AddField(name: "roll+", value: "Rolls two d6 and adds a specified rating modifier to give you your total! This will simply function like roll if you are not playing.\n(usage: `.roll+ <rating>`, aliases: `.r+`)",inline: false);
            embed.AddField(name: "sheet", value: "Shows the ratings and moves of a character! This will only work if you are playing or specify someone who is.\n(usage: `.sheet <character name or player name (blank for yourself)>`, aliases: `.sh`)",inline: false);
            embed.AddField(name: "hold", value: "Awards players Hold and removes their Hold when they use it. KEEPER ONLY.\n(usage: `.hold <+/- number of Hold> <character name or player name>`, aliases: `.h`)",inline: false);
            embed.AddField(name: "move", value: "Shows information on a specified move or a list of all moves!\n(usage: `.move <move name or \"list\"`, aliases: `.mv`)",inline: false);

            embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
            await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("roll")]
        [Description("Rolls 2d6 for a random number between 2 and 12.")]
        [Aliases("r")]
        public async Task Roll(CommandContext ctx){ 
            await ctx.TriggerTypingAsync();

            var embed = new DiscordEmbedBuilder{
                    Title = $"{ctx.User.Username.ToString()} rolls the dice!",
                    Description = $"You rolled **{new Random().Next(2,12).ToString()}**!",
                    Color = DiscordColor.Red
            }; 
            
            embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
            await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("roll+")]
        [Description("Rolls 2d6 for a random number between 2 and 12 and adds one of your ratings.")]
        [Aliases("r+")]
        public async Task RollPlus(CommandContext ctx, string rating = "[null]"){ 
            await ctx.TriggerTypingAsync();

            int result = new Random().Next(2,12);
            bool isPlayer = false;
            Player player = new Player(0, string.Empty, string.Empty, new Dictionary<string, int>(), 0, string.Empty, new Dictionary<string, string>());

            foreach (var i in config.players){ if(i.id == ctx.User.Id) { isPlayer = true; player = i; } }

            if (isPlayer){ 
                var embed = player.ratings.ContainsKey(rating) ? new DiscordEmbedBuilder{
                    Title = player.charName + " rolls the dice!",
                    Description = $"You rolled **{result.ToString()}**, plus your **{rating}** rating of **{player.ratings[rating].ToString()}** for a total of **{(result + player.ratings[rating]).ToString()}**!",
                    Color = DiscordColor.Red
                }:
                new DiscordEmbedBuilder{
                    Title = "Oops!",
                    Description = $"{rating} is not a valid rating! Try again.",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false); }
            else { 
                var embed = new DiscordEmbedBuilder {
                    Title = "Oops!",
                    Description = $"You aren't playing the game, but you would have rolled **{result.ToString()}** without any modifiers!",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            }
        }
        
        [Command("sheet")]
        [Description("Shows information on your character or another player's character!")]
        [Aliases("sh")]
        public async Task Sheet(CommandContext ctx, string name = "h"){
            await ctx.TriggerTypingAsync();

            bool isPlayer = false;
            Player player = new Player(0, string.Empty, string.Empty, new Dictionary<string, int>(), 0, string.Empty, new Dictionary<string, string>());

            foreach (var i in config.players){ 
                if(i.id == ctx.User.Id) { isPlayer = true; player = i; }
                else if(i.name.ToLower() == name.ToLower()) { isPlayer = true; player = i; }
                else if(i.charName.ToLower() == name.ToLower()) { isPlayer = true; player = i; }
            }
            
            if (isPlayer){
                var embed = new DiscordEmbedBuilder{
                    Title = $"{player.name}'s Sheet:",
                    Description = $"{player.name} is playing as {player.charName}, the {player.hunterClass}.\n\n**Charm:** {player.ratings["charm"]}, **Cool:** {player.ratings["cool"]}, **Sharp:** {player.ratings["sharp"]}, **Tough:** {player.ratings["tough"]}, **Weird:** {player.ratings["weird"]}\n**Holds** {player.holds}",
                    Color = DiscordColor.Red
                };
                foreach (string i in player.moves.Keys){ embed.AddField(name: i, value: player.moves[i], inline: false); }

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
             }
            else { 
                var embed = new DiscordEmbedBuilder{
                    Title = "Oops!",
                    Description = ":x: **That person isn't playing the game, dumbass.**",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            }
        }

        [Command("hold")]
        [Description("")]
        [Aliases("h")]
        public async Task Hold(CommandContext ctx, int num, string name = null){ 
            await ctx.TriggerTypingAsync();

            bool isPlayer = false;
            Player player = new Player(0, string.Empty, string.Empty, new Dictionary<string, int>(), 0, string.Empty, new Dictionary<string, string>());

            foreach (var i in config.players){ 
                if(i.id == ctx.User.Id) { isPlayer = true; player = i; }
                else if(i.name.ToLower() == name.ToLower()) { isPlayer = true; player = i; }
                else if(i.charName.ToLower() == name.ToLower()) { isPlayer = true; player = i; }
            }

            if (isPlayer && ctx.User.Id == 140593733220564992){
                player.holds+= num;
                var embed = new DiscordEmbedBuilder{
                    Title = "Hold awarded!",
                    Description = $"{player.charName} now has hold {player.holds}!",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            }
            else if(ctx.User.Id != 140593733220564992){
                var embed = new DiscordEmbedBuilder{
                    Title = "Oops!",
                    Description = $"Only the keeper can use that command!",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            }
            else { 
                var embed = new DiscordEmbedBuilder{
                    Title = "Oops!",
                    Description = ":x: **That person isn't playing the game, dumbass.**",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            } 
        }

        [Command("move")] 
        [Description("Looks up the description of a move.")] 
        [Aliases("mv")]
        public async Task Move(CommandContext ctx, [RemainingText]string _move = "") 
        {
            await ctx.TriggerTypingAsync();
            string move = _move.ToLower().Replace(" ", string.Empty);

            if(move.ToLower() == "list") {
                var embed = new DiscordEmbedBuilder{
                    Title = "Here's a list of all of the moves!",
                    Description = "• Kick Some Ass\n• Act Under Pressure\n• Help Out\n• Investigate a Mystery\n• Manipulate Someone\n• Protect Someone\n• Read a Bad Situation\n• Use Magic\n\n** (Use `.move <move>` for more info)**",
                    Color = DiscordColor.Red
                };

                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);

            }
            else {
                var embed = config.moves.ContainsKey(move) ? new DiscordEmbedBuilder{
                    Title = $"Here's what I found on the move {_move}:",
                    Description = config.moves[move],
                    Color = DiscordColor.Red
                }:
                new DiscordEmbedBuilder{
                    Title = "Oops!",
                    Description = ":x: **I can't find that move. Did you spell it right?.**",
                    Color = DiscordColor.Red
                };
                
                embed.WithFooter(text: "Coded in DSharpPlus by @cowsauce#6969", icon_url: "https://cdn.discordapp.com/avatars/645064200502378506/a_3e72d6460efd8dc6349f89d551aa78f9.gif?size=256");
                await ctx.RespondAsync(embed: embed).ConfigureAwait(false);
            }
            
        }
    }
}
