using Discord;
using Discord.Interactions;

namespace Gudgeon.Modules.Help
{
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    public partial class HelpModule : InteractionModuleBase<SocketInteractionContext>
    {
    }
}
