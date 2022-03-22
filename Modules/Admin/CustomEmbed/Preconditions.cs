using Discord;
using Discord.Interactions;

namespace Gudgeon.Modules.Admin.CustomEmbed
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    [RequireBotPermission(ChannelPermission.EmbedLinks)]
    public partial class CustomEmbedModule : InteractionModuleBase<SocketInteractionContext>
    {
    }
}
