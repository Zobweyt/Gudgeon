using Discord;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using Gudgeon.Common.Customization.Help;

namespace Gudgeon.Modules.Help
{
    public partial class HelpModule : InteractionModuleBase<SocketInteractionContext>
    {
       [SlashCommand("help", "Get the list of avaible commands")]
        public async Task HelpAsync()
        {
            var embed = new GudgeonEmbedBuilder()
                .WithTitle("Categories")
                .WithDescription(
                "Please use the Select Menu below to\n" +
                "explore the corresponding category.")
                .AddField("Bot Links",
               $"• Official bot **[GitHub]({Links.GudgeonRepository})**.")
                .WithFooter($"Requested by {Context.User.Username}#{Context.User.Discriminator}")
                .WithColor(Colors.Default)
                .Build();
            
            var menu = new SelectMenuBuilder()
                .WithCustomId($"help-menu:{Context.User.Id}")
                .AddOption("Games", "help-games-category", emote: Emoji.Parse(HelpEmojis.Computer))
                .AddOption("Information", "help-information-category", emote: Emoji.Parse(HelpEmojis.InformationSource))
                .AddOption("Admin", "help-admin-category", emote: Emoji.Parse(HelpEmojis.Shield))
                .AddOption("Owner", "help-owner-category", emote: Emoji.Parse(HelpEmojis.Gear));

            var component = new ComponentBuilder()
                .WithSelectMenu(menu)
                .Build();

            await RespondAsync(
                embed: embed,
                components: component)
                .ConfigureAwait(false);
        }
    }
}