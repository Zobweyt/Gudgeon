using Discord;
using Discord.Interactions;
using Fergun.Interactive;

namespace Gudgeon.Modules.Games.TwentyFortyEight
{
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    [RequireBotPermission(GuildPermission.UseExternalEmojis)]
    public partial class TilesModule : InteractionModuleBase<SocketInteractionContext>
    {
        private InteractiveService _interactive;
        public TilesModule(InteractiveService interactive)
        {
            _interactive = interactive;
        }
    }
}