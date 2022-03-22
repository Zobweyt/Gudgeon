using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Admin.CustomEmbed
{
    public partial class CustomEmbedModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("edit-embed", "Edits an embed message by ID in the current channel")]
        public async Task EditEmbedAsync(string messageId)
        {
            ulong.TryParse(messageId, out ulong id);

            var message = id == 0
                ? null
                : await Context.Channel.GetMessageAsync(id);

            if (message != null && message.Author.IsBot)
            {
                await RespondWithModalAsync<EmbedConfigurationModal>($"apply-embed-editing:{id}");
            }
            else
            {
                var embed = new GudgeonEmbedBuilder()
                    .WithStyle(new ErrorStyle(), "Invalid ID!")
                    .WithDescription($"The `{messageId}` is invalid.")
                    .Build();

                await RespondAsync(embed: embed, ephemeral: true);
            }
        }

        [ModalInteraction("apply-embed-editing:*")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ApplyEmbedEditingAsync(string id, EmbedConfigurationModal modal)
        {
            var userEmbed = new GudgeonEmbedBuilder()
                .WithTitle(modal.EmbedTitle)
                .WithDescription(modal.EmbedDescription)
                .WithFooter("Last updated at")
                .WithCurrentTimestamp()
                .WithColor(Colors.Default)
                .Build();

            var message = await Context.Channel.GetMessageAsync(ulong.Parse(id));

            if (message as SocketUserMessage == null)
            {
                await (message as RestUserMessage).ModifyAsync(x =>
                {
                    x.Embed = userEmbed;
                });
            }
            else
            {
                await (message as SocketUserMessage).ModifyAsync(x =>
                {
                    x.Embed = userEmbed;
                });
            }

            var embed = new GudgeonEmbedBuilder()
                .WithStyle(new SuccessStyle(), "Embed edited!")
                .WithDescription("You can hide this message.")
                .Build();

            await RespondAsync(embed: embed, ephemeral: true);
        }
    }
}