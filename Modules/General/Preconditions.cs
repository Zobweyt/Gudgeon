using Discord;
using Discord.Interactions;

namespace Gudgeon.Modules.General
{
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    public partial class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {
    }
}
