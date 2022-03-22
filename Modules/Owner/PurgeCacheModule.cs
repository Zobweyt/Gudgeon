using Discord;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Owner
{
    [RequireOwner]
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    public class PurgeCacheModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("purge-cache", "Purge bot's cache")]
        public async Task ClearCacheAsync()
        {
            var embed = new GudgeonEmbedBuilder()
                .WithTitle("Purging")
                .WithDescription($"Wait while {Context.Client.CurrentUser.Username} purging cache.")
                .WithColor(Colors.Default)
                .Build();

            await RespondAsync(embed: embed, ephemeral: true);

            Context.Client.PurgeChannelCache();
            Context.Client.PurgeDMChannelCache();
            Context.Client.PurgeUserCache();

            var successEmbed = new GudgeonEmbedBuilder()
                .WithStyle(new SuccessStyle(), "Cache purged!")
                .WithDescription("You need to restart the application.")
                .Build();

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = successEmbed;
            });
        }
    }
}