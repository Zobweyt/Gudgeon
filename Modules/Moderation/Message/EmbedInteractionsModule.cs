using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Styles;
using Gudgeon.Modules.Moderation.Message.Addons;
using Gudgeon.Modules.Moderation.Message.Modals;

namespace Gudgeon.Modules.Moderation.Message
{
    [RequireBotPermission(GuildPermission.SendMessages)]
    [RequireBotPermission(GuildPermission.EmbedLinks)]
    public partial class EmbedInteractionsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [ModalInteraction("embed_send_modal_submit")]
        public async Task EmbedSendModalSubmitAsync(EmbedModal embed)
        {
            await Context.Channel.SendMessageAsync(embed: EmbedConstructor.Build(embed));
            await ResultResponseSender.SuccessResponseAsync(Context.Interaction);
        }

        [ModalInteraction("embed_edit_modal_submit:*")]
        public async Task EmbedModifyModalSubmitAsync(ulong id, EmbedModal embed)
        {
            IMessage message = await Context.Channel.GetMessageAsync(id);

            if (message as SocketUserMessage == null)
            {
                await (message as RestUserMessage).ModifyAsync(x => { x.Embed = EmbedConstructor.Build(embed); });
            }
            else
            {
                await (message as SocketUserMessage).ModifyAsync(x => { x.Embed = EmbedConstructor.Build(embed); });
            }

            await ResultResponseSender.SuccessResponseAsync(Context.Interaction);
        }
    }
}