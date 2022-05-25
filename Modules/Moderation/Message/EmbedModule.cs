using Discord;
using Discord.Interactions;
using Gudgeon.Modules.Moderation.Message.Addons;
using Gudgeon.Modules.Moderation.Message.Modals;

namespace Gudgeon.Modules.Moderation.Message
{
    [Group("embed", "Embed commands")]
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(GuildPermission.SendMessages)]
    [RequireBotPermission(GuildPermission.EmbedLinks)]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class EmbedModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("send", "Sends an embed to the current channel")]
        public async Task EmbedSendAsync()
            => await RespondWithModalAsync<EmbedModal>("embed_send_modal_submit");

        [SlashCommand("edit", "Edits the embed in the current channel")]
        public async Task EmbedEditAsync([Summary("id", "The ID of a message in the current channel")] ulong id)
        {
            IMessage? message = await Context.Channel.GetMessageAsync(id);
            MessageEditingError? error = MessageErrorChecker.Check(message, Context.Client.CurrentUser.Id);

            if (error.HasValue)
            {
                await ResultResponseSender.HandleErrorAsync(Context.Interaction, error);
            }
            else
            {
                await RespondWithModalAsync(ProcessedEmbedModal.Build(message));
            }
        }
    }
}