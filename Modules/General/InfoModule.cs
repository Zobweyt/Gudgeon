using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Customization;

namespace Gudgeon.Modules.General
{
    public partial class InfoModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("user-info", "Displays information about a user")]
        public async Task UserInfoAsync([Summary("user", "A user to show information about")] SocketGuildUser? user = null)
        {
            if (user == null)
            {
                user = Context.Interaction.User as SocketGuildUser;
            }

            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithTitle("User info")
                .WithDescription(
                $"• Type: **{ (user.IsBot ? "Bot" : "User") } " +
                $"{(user.Id == Context.Guild.OwnerId ? "(Server owner)" : "")}**\n" +
                $"• Name: **{user.Username}#{user.Discriminator}**\n" +
                $"• Nickname: **{user?.Nickname ?? "-"}**\n" +
                $"• User ID: **{user.Id}**\n" +
                $"• Joined: **<t:{user.JoinedAt.Value.ToUnixTimeSeconds()}:R>** (<t:{user.JoinedAt.Value.ToUnixTimeSeconds()}:F>)\n" +
                $"• Discord account creation: **<t:{user.CreatedAt.ToUnixTimeSeconds()}:R>** (<t:{user.CreatedAt.ToUnixTimeSeconds()}:F>)")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .WithColor(Colors.Default)
                .Build();

            var button = new ComponentBuilder()
                .WithButton("View avatar", style: ButtonStyle.Link, url: user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .Build();

            await RespondAsync(embed: embed, components: button);
        }

        [SlashCommand("channel-info", "Displays information about a channel")]
        public async Task ChannelInfoAsync([Summary("channel", "A channel to show information about")] IGuildChannel? channel = null)
        {
            if (channel == null)
            {
                channel = Context.Interaction.Channel as IGuildChannel;
            }

            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle("Channel info")
                .WithDescription(
                $"• Name: **{channel.Name}**\n" +
                $"• Channel ID: **{channel.Id}**\n" +
                $"• Channel creation: **<t:{channel.CreatedAt.ToUnixTimeSeconds()}:R>** (<t:{channel.CreatedAt.ToUnixTimeSeconds()}:F>)")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .WithColor(Colors.Default)
                .Build();

            await RespondAsync(embed: embed);
        }

        [SlashCommand("guild-info", "Displays information about the guild")]
        public async Task GuildInfoAsync()
        {
            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle("Guild info")
                .WithDescription(
                $"• Name: **{Context.Guild.Name}**\n" +
                $"• Members: **{Context.Guild.MemberCount} members**\n" +
                $"• Online members: **{Context.Guild.Users.Where(x => x.Status != UserStatus.Offline).Count()} members**\n" +
                $"• Guild ID: **{Context.Guild.Id}**\n" +
                $"• Guild creation: **<t:{Context.Guild.CreatedAt.ToUnixTimeSeconds()}:R>** (<t:{Context.Guild.CreatedAt.ToUnixTimeSeconds()}:F>)")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .WithColor(Colors.Default)
                .Build();

            var button = new ComponentBuilder()
               .WithButton("View icon", style: ButtonStyle.Link, url: Context.Guild.IconUrl)
               .Build();

            await RespondAsync(embed: embed, components: button);
        }
    }
}