using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Common;

namespace Gudgeon.Modules.Information
{
    public class InformationModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region user-info

        [SlashCommand("user-info", "Displays information about a user.", runMode: RunMode.Async)]
        public async Task UserInfoAsync([Summary("user", "A user to show information about.")] SocketGuildUser? user = null)
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
                $"• Joined: **{user?.JoinedAt.Value:dd.MM.yyyy}**\n" +
                $"• Discord account creation: **{user.CreatedAt:dd.MM.yyyy}**\n")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .Build();

            var button = new ComponentBuilder()
                .WithButton("View avatar", style: ButtonStyle.Link, url: user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .Build();

            await RespondAsync(
                embed: embed, 
                components: button)
                .ConfigureAwait(false);
        }

        #endregion

        #region channel-info

        [SlashCommand("channel-info", "Displays information about a channel.", runMode: RunMode.Async)]
        public async Task ChannelInfoAsync([Summary("channel", "A channel to show information about.")] IGuildChannel? channel = null)
        {
            if (channel is null)
            {
                channel = Context.Interaction.Channel as IGuildChannel;
            }

            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle("Channel info")
                .WithDescription(
                $"• Name: **{channel.Name}**\n" +
                $"• Channel ID: **{channel.Id}**\n" +
                $"• Channel creation: **{channel.CreatedAt:dd.MM.yyyy}**\n")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .Build();

            await RespondAsync(
                embed: embed)
                .ConfigureAwait(false);
        }

        #endregion

        #region server-info

        [SlashCommand("server-info", "Displays information about the guild.", runMode: RunMode.Async)]
        public async Task GuildInfoAsync()
        {
            var guild = Context.Guild;

            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle("Guild info")
                .WithDescription(
                $"• Name: **{guild.Name}**\n" +
                $"• Membercount: **{guild.MemberCount} members**\n" +
                $"• Online members: **{guild.Users.Where(x => x.Status != UserStatus.Offline).Count()} members**\n" +
                $"• Guild ID: **{guild.Id}**\n" +
                $"• Guild creation: **{guild.CreatedAt:dd.MM.yyyy}**\n")
                .WithFooter($"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .Build();

            var button = new ComponentBuilder()
               .WithButton("View icon", style: ButtonStyle.Link, url: guild.IconUrl)
               .Build();

            await RespondAsync(embed: embed, components: button).ConfigureAwait(false);
        }

        #endregion

        #region bot-state

        [SlashCommand("bot-state", "Displays information about the bot state.", runMode: RunMode.Async)]
        public async Task BotStateAsync()
        {
            var embed = new GudgeonEmbedBuilder()
                .WithThumbnailUrl(
                Context.Interaction.User.GetAvatarUrl() ?? Context.Interaction.User.GetDefaultAvatarUrl())
                .WithTitle(
                "Bot state")
                .WithDescription(
                $"• Status: **{Context.Client.Status}**\n" +
                $"• Latency: **{Context.Client.Latency} ms**\n" +
                $"• Guilds count: **{Context.Client.Guilds.Count}**\n" +
                $"• Written on: **C#, Discord.NET**\n" +
                $"• Developed by: **Zobweyt#5122**\n")
                .WithFooter(
                $"Requsted by {Context.User.Username}#{Context.User.Discriminator}")
                .Build();

            var button = new ComponentBuilder()
               .WithButton(
                "GitHub", style: ButtonStyle.Link, url: @"https://github.com/Zobweyt")
               .Build();

            await RespondAsync(
                embed: embed, 
                components: button)
                .ConfigureAwait(false);
        }

        #endregion
    }
}