using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Admin
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.ReadMessageHistory)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    [RequireBotPermission(ChannelPermission.ManageMessages)]
    public class ClearModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("clear", "Deletes a channel's messages")]
        public async Task ClearAsync([Summary("amount", "The number of messages to clean up (2-500)")] int amount)
        {
            var embed = new GudgeonEmbedBuilder();
            
            if (amount > 500)
            {
                embed
                    .WithStyle(new ErrorStyle(), "Overflow error!")
                    .WithDescription("You can't clear more than 500 messages.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator}");
            }
            else if (amount < 2)
            {
                embed
                    .WithStyle(new ErrorStyle(), "Overflow error!")
                    .WithDescription("You can't clear less than 2 message.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator}");
            }
            else
            {
                var messages = (IReadOnlyCollection<IMessage>?)await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                embed
                    .WithStyle(new SuccessStyle())
                    .WithDescription($"{messages?.Count ?? 0} messages deleted.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator}");
            }

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}