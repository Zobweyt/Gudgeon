using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Clear
{
    public class ClearModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region clear

        [SlashCommand("clear", "Deletes a channel's messages.", runMode: RunMode.Async)]
        [RequireUserPermission(ChannelPermission.ReadMessageHistory)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task ClearAsync([Summary("amount", "The number of messages to clean up.")] int amount)
        {
            var embed = new GudgeonEmbedBuilder();

            if (amount > 100)
            {
                embed
                    .WithStyle(new ErrorStyle(), "Overflow error!")
                    .WithDescription("You can't clear more than 100 messages.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | this message dissapear in 8 seconds.");
            }
            else if (amount < 1)
            {
                embed
                    .WithStyle(new ErrorStyle(), "Overflow error!")
                    .WithDescription("You can't clear less than 1 message.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | this message dissapear in 8 seconds.");
            }
            else
            {
                var messages = (IReadOnlyCollection<IMessage>?)await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                embed
                    .WithStyle(new SuccessStyle())
                    .WithDescription($"{messages?.Count ?? 0} messages deleted.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | this message dissapear in 8 seconds.");
            }

            await RespondAsync(
                embed: embed.Build())
                .ConfigureAwait(false);

            await Task.Delay(8000);
            await DeleteOriginalResponseAsync();
        }

        #endregion
    }
}