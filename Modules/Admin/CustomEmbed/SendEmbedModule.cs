using Discord;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Admin.CustomEmbed
{
    public partial class CustomEmbedModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("send-embed", "Sends an embed message in the current channel")]
        public async Task SendEmbedAsync()
        {
            await RespondWithModalAsync<EmbedConfigurationModal>("apply-embed-sending");
        }

        [ModalInteraction("apply-embed-sending")]
        public async Task ApplyEmbedSendingAsync(EmbedConfigurationModal modal)
        {
            var userEmbed = new GudgeonEmbedBuilder()
                .WithTitle(modal.EmbedTitle)
                .WithDescription(modal.EmbedDescription)
                .WithFooter("Last updated at")
                .WithCurrentTimestamp()
                .WithColor(Colors.Default)
                .Build();

            await Context.Channel.SendMessageAsync(embed: userEmbed);

            var embed = new GudgeonEmbedBuilder()
                .WithStyle(new SuccessStyle(), "Embed sent!")
                .WithDescription("You can hide this message.")
                .Build();

            await RespondAsync(embed: embed, ephemeral: true);
        }
    }
}