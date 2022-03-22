using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Attributes;
using Gudgeon.Common.Customization.Help;
using Gudgeon.Common.Customization;

namespace Gudgeon.Modules.Help
{
    public partial class HelpModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region help-menu

        [DoUserCheck]
        [ComponentInteraction($"help-menu:*", runMode: RunMode.Async)]
        public async Task HelpMenuAsync(string[] selectedCategory)
        {
            switch (selectedCategory[0])
            {
                case "help-games-category":
                    await UpdateToGamesCategory();
                    return;
                case "help-information-category":
                    await UpdateToInformationCategory();
                    return;
                case "help-admin-category":
                    await UpdateToAdminCategory();
                    return;
                case "help-owner-category":
                    await UpdateToOwnerCategory();
                    return;
            }
        }



        #endregion

        #region Categories

        private async Task UpdateToGamesCategory()
        {
            var embed = new GudgeonEmbedBuilder()
                 .WithTitle("Games")
                 .AddField("/2048", "The classic 2048 game in Discord.")
                 .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | Use the selection menu below for navigation.")
                 .WithColor(Colors.Default)
                 .Build();

            var menu = new SelectMenuBuilder()
                .WithCustomId($"help-menu:{Context.User.Id}")
                .AddOption("Information", "help-information-category", emote: Emoji.Parse(HelpEmojis.InformationSource))
                .AddOption("Admin", "help-admin-category", emote: Emoji.Parse(HelpEmojis.Shield))
                .AddOption("Owner", "help-owner-category", emote: Emoji.Parse(HelpEmojis.Gear));

            await ApplyUpdate(embed, menu);
        }
        private async Task UpdateToInformationCategory()
        {
            var embed = new GudgeonEmbedBuilder()
                 .WithTitle("Information")
                 .AddField("/help", "Displays information about all the bot's commands.")
                 .AddField("/user-info [user-mention]", "Displays information about a user.")
                 .AddField("/channel-info [channel-mention]", "Displays information about a channel.")
                 .AddField("/guild-info", "Displays information about the guild.")
                 .AddField("/bot-state", "Displays information about the bot state.")
                 .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | Use the selection menu below for navigation.")
                 .WithColor(Colors.Default)
                 .Build();

            var menu = new SelectMenuBuilder()
                .WithCustomId($"help-menu:{Context.User.Id}")
                .AddOption("Games", "help-games-category", emote: Emoji.Parse(HelpEmojis.Computer))
                .AddOption("Admin", "help-admin-category", emote: Emoji.Parse(HelpEmojis.Shield))
                .AddOption("Owner", "help-owner-category", emote: Emoji.Parse(HelpEmojis.Gear));

            await ApplyUpdate(embed, menu);
        }
        private async Task UpdateToAdminCategory()
        {
            var embed = new GudgeonEmbedBuilder()
                 .WithTitle("Admin")
                 .AddField($"/clear [amount] | {HelpEmojis.Lock}", "Deletes a channel's messages.")
                 .AddField($"/send-embed | {HelpEmojis.Lock}", "Sends an embed message in the current channel.")
                 .AddField($"/edit-embed [message-id] | {HelpEmojis.Lock}", "Edits an embed message by ID in the current channel.")
                 .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | Use the selection menu below for navigation.")
                 .WithColor(Colors.Default)
                 .Build();

            var menu = new SelectMenuBuilder()
                .WithCustomId($"help-menu:{Context.User.Id}")
                .AddOption("Games", "help-games-category", emote: Emoji.Parse(HelpEmojis.Computer))
                .AddOption("Information", "help-information-category", emote: Emoji.Parse(HelpEmojis.InformationSource))
                .AddOption("Owner", "help-owner-category", emote: Emoji.Parse(HelpEmojis.Gear));

            await ApplyUpdate(embed, menu);
        }
        private async Task UpdateToOwnerCategory()
        {
            var embed = new GudgeonEmbedBuilder()
                 .WithTitle("Owner")
                 .AddField($"/clear-cache | {HelpEmojis.Lock}", "Purge bot's cache.")
                 .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | Use the selection menu below for navigation.")
                 .WithColor(Colors.Default)
                 .Build();

            var menu = new SelectMenuBuilder()
                .WithCustomId($"help-menu:{Context.User.Id}")
                .AddOption("Games", "help-games-category", emote: Emoji.Parse(HelpEmojis.Computer))
                .AddOption("Information", "help-information-category", emote: Emoji.Parse(HelpEmojis.InformationSource))
                .AddOption("Admin", "help-admin-category", emote: Emoji.Parse(HelpEmojis.Shield));

            await ApplyUpdate(embed, menu);
        }

        private async Task ApplyUpdate(Embed embed, SelectMenuBuilder menu)
        {
            var component = new ComponentBuilder()
                .WithSelectMenu(menu)
                .Build();

            await (Context.Interaction as SocketMessageComponent).UpdateAsync(x =>
            {
                x.Embed = embed;
                x.Components = component;
            });
        }

        #endregion
    }
}