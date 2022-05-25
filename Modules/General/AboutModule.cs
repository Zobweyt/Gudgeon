using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Attributes;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using static Gudgeon.Common.Customization.Links;

namespace Gudgeon.Modules.General
{
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    public class AboutModule : InteractionModuleBase<SocketInteractionContext>
    {
        private Embed InfoEmbed
        {
            get 
            {
                return new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .AddField(
                "📁 | Versions",
                $"• **[Gudgeon]({GudgeonRepository}) - 1.0.0**\n" +
                $"• **[.NET]({Dotnet}) - 6.0**\n" +
                $"• **[.NET Framework]({DotnetFramework}) - 4.8**\n" +
                $"• **[Discord.NET.Interactions]({DiscordNetInteractions}) - 3.7.0**\n" +
                $"• **[Discord.Addons.Hosting]({DiscordAddonsHosting}) - 5.1.0**\n" +
                $"• **[Fergun.Interactive]({FergunInteractive}) - 1.5.4**")
                .WithFooter("Zobweyt#5122", $"{ZobweytAvatarUrl}")
                .WithColor(Colors.Default)
                .Build();
            }
        }
        private MessageComponent ReloadButton
        {
            get
            {
                return new ComponentBuilder()
               .WithButton("Reload", $"info-reload:{Context.User.Id}", ButtonStyle.Secondary)
               .Build();
            }
        }

        [SlashCommand("about", "Provides information about the application")]
        public async Task BotStateAsync()
        {
            await RespondAsync(embed: InfoEmbed, components: ReloadButton);
        }

        [DoUserCheck]
        [ComponentInteraction("info-reload:*")]
        public async Task BotStateReloadAsync()
        {
            await (Context.Interaction as SocketMessageComponent).UpdateAsync(x =>
            {
                x.Embed = InfoEmbed;
                x.Components = ReloadButton;
            });
        }
    }
}