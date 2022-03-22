using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Attributes;
using Gudgeon.Common;
using Gudgeon.Common.Customization;

namespace Gudgeon.Modules.General
{
    public partial class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {
        private Embed BotStateEmbed
        {
            get 
            { 
                return new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Links.GudgeonAvatarUrl)
                .WithTitle("Bot state")
                .WithDescription(
                $"• Status: **{Context.Client.Status}**\n" +
                $"• Latency: **{Context.Client.Latency} ms**\n" +
                $"• Guilds count: **{Context.Client.Guilds.Count}**\n" +
                $"• Written on: **C#, [Discord Addons Hosting](https://github.com/Hawxy/Discord.Addons.Hosting), [Fergun Interactive](https://github.com/d4n3436/Fergun.Interactive) and [Interaction Framework](https://github.com/discord-net/Discord.Net/tree/dev/samples/InteractionFramework).**\n" +
                $"• Developed by: **Zobweyt#5122**\n")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .WithColor(Colors.Default)
                .Build();
            }
        }
        private MessageComponent BotStateButtons
        {
            get
            {
                return new ComponentBuilder()
               .WithButton("Reload", $"bot-state-reload:{Context.User.Id}", ButtonStyle.Secondary)
               .WithButton("GitHub", style: ButtonStyle.Link, url: Links.GudgeonRepository)
               .Build();
            }
        }

        [SlashCommand("bot-state", "Displays information about the bot state")]
        public async Task BotStateAsync()
        {
            await RespondAsync(embed: BotStateEmbed, components: BotStateButtons);
        }

        [DoUserCheck]
        [ComponentInteraction("bot-state-reload:*")]
        public async Task BotStateReloadAsync()
        {
            await (Context.Interaction as SocketMessageComponent).UpdateAsync(x =>
            {
                x.Embed = BotStateEmbed;
                x.Components = BotStateButtons;
            });
        }
    }
}
