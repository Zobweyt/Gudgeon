using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Styles;
using Gudgeon.Common.Customization;
using Gudgeon.Modules.Moderation.Clean.Addons;
using static Gudgeon.Modules.Moderation.Clean.Addons.Limit;

namespace Gudgeon.Modules.Admin
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(ChannelPermission.ViewChannel)]
    [RequireBotPermission(ChannelPermission.ReadMessageHistory)]
    [RequireBotPermission(ChannelPermission.SendMessages)]
    [RequireBotPermission(ChannelPermission.ManageMessages)]
    public class CleanModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("clean", "Deletes multiple channel messages")]
        public async Task ClearAsync([Summary("amount", "The number of messages to clean up (2-500)")] int amount)
        {
            if (amount > (int)Upper)
            {
                await OverflowResponseAsync(Upper);
            }
            else if (amount < (int)Lower)
            {
                await OverflowResponseAsync(Lower);
            }
            else
            {
                await MessagesProcessingResponseAsync(amount);
            }

            await CleanOriginalResponseAsync();
        }

        private async Task MessagesProcessingResponseAsync(int amount)
        {
            Embed embed = new GudgeonEmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                    .WithName("Processing")
                    .WithIconUrl(Icons.Garbage);
                })
                .WithDescription($"Please wait while **{amount}** messages is being processed! {AnimatedEmojis.Loading}")
                .WithColor(Colors.Default)
                .Build();

            await RespondAsync(embed: embed);

            await CleanMessagesAsync(amount);
        }
        private async Task OverflowResponseAsync(Limit limit)
        {
            Embed embed = new GudgeonEmbedBuilder()
                    .WithStyle(new ErrorStyle())
                    .WithDescription($"You can not clean {(limit == Upper ? $"more than" : $"less than")} **{(int)limit}** messages.")
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | This message disappears in 8 seconds")
                    .Build();

            await RespondAsync(embed: embed);
        }
        private async Task SuccessDeletionResponseAsync(List<IMessage> youngMessages, IEnumerable<IMessage> capturedMesssages)
        {
            int oldMessages = capturedMesssages.Count() - youngMessages.Count;
            int cleanedMessages = youngMessages.Count;

            string description = "";

            if (capturedMesssages.ToList().Exists(x => x.Timestamp < DateTime.Now.AddDays(-14)))
            {
                description = $"The deletion could not be completed because **{oldMessages - 1}** {(oldMessages == 1 ? "message" : "messages")} is too old!\n";
            }
            description += $"**{cleanedMessages}** {(cleanedMessages == 1 ? "message has been" : "messages have been")} been successfully cleaned.";

            IUserMessage? response = await GetOriginalResponseAsync();

            if (response != null)
            {
                await ModifyOriginalResponseAsync(x =>
                {
                    x.Components = new ComponentBuilder().Build();
                    x.Embed = new GudgeonEmbedBuilder()
                    .WithStyle(new SuccessStyle())
                    .WithDescription(description)
                    .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | This message disappears in 8 seconds")
                    .Build();
                });
            }
        }
        
        private async Task CleanMessagesAsync(int amount)
        {
            IEnumerable<IMessage> capturedMesssages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            List<IMessage> youngMessages = capturedMesssages.Skip(1).ToList().FindAll(x => x.Timestamp > DateTime.Now.AddDays(-14));

            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(youngMessages);

            await SuccessDeletionResponseAsync(youngMessages, capturedMesssages);
        }
        private async Task CleanOriginalResponseAsync()
        {
            await Task.Delay(8000);
            IUserMessage? response = await GetOriginalResponseAsync();

            if (response != null)
            {
                await response.DeleteAsync();
            }
        }
    }
}