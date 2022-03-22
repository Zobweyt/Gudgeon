using Discord;
using Discord.Interactions;
using Gudgeon.Common;
using Gudgeon.Common.Customization;
using Gudgeon.Common.Styles;

namespace Gudgeon.Modules.Games.TwentyFortyEight
{
    public partial class TilesModule : InteractionModuleBase<SocketInteractionContext>
    {
        private MessageComponent BoardShiftingButtons
        {
            get
            {
                return new ComponentBuilder()
                .WithButton(" ", "tiles-empty-1", disabled: true, style: ButtonStyle.Secondary)
                .WithButton(emote: Emoji.Parse("⬆️"), customId: "2048-shift-board-up")
                .WithButton(" ", "tiles-empty-2", disabled: true, style: ButtonStyle.Secondary)
                .WithButton(emote: Emoji.Parse("⬅️"), customId: "2048-shift-board-left", row: 1)
                .WithButton(emote: Emoji.Parse("⬇️"), customId: "2048-shift-board-down", row: 1)
                .WithButton(emote: Emoji.Parse("➡️"), customId: "2048-shift-board-right", row: 1)
                .Build();
            }
        }

        [SlashCommand("2048", "The classic 2048 game")]
        public async Task TwentyFortyEightIntroductionAsync()
        {
            var embed = new GudgeonEmbedBuilder()
                .WithTitle("Introduction")
                .WithDescription(
                "Click the buttons for the direction you want to shift the board.\n" +
                "Tiles with the same value will join together.\n" +
                "Try to reach the 2048 tile!")
                .WithFooter($"{Context.User.Username}#{Context.User.Discriminator} | Use button below to start!")
                .WithColor(Colors.Default)
                .Build();

            var builder = new ComponentBuilder()
                .WithButton("Start", "2048-start", ButtonStyle.Success)
                .Build();

            await RespondAsync(embed: embed, components: builder);

            await TwentyFortyEightVerifyAsync(await GetOriginalResponseAsync());
        }
        private async Task TwentyFortyEightVerifyAsync(IUserMessage message)
        {
            while (true)
            {
                var result = await _interactive.NextMessageComponentAsync(
                x => x.Message.Id == message.Id,
                timeout: TimeSpan.FromMinutes(5));

                if (result.IsTimeout)
                {
                    await DeleteOriginalResponseAsync();
                    await Context.Channel.SendMessageAsync("Timeout!\n\n");
                    return;
                }

                if (Context.User.Id != result.Value.User.Id)
                {
                    var embed = new GudgeonEmbedBuilder()
                        .WithStyle(new ErrorStyle(), "Missing permissions")
                        .WithDescription($"• Only {Context.User.Mention} can control this menu.")
                        .WithFooter($"{result.Value.User.Username}#{result.Value.User.Discriminator}")
                        .Build();
                    await RespondAsync(embed: embed, ephemeral: true);
                    continue;
                }

                Board board = new();

                await result?.Value.UpdateAsync(x =>
                {
                    x.Embed = null;
                    x.Content = board.BuildBoardMessage(Context);
                    x.Components = BoardShiftingButtons;
                });

                await TwentyFortyEightStartAsync(message, board);
            }
        }

        private async Task TwentyFortyEightStartAsync(IUserMessage message, Board board)
        {
            while (true)
            {
                if (board.IsFull && board.CanMove == false)
                {
                    await DeleteOriginalResponseAsync();
                    await Context.Channel.SendMessageAsync("The board is full!\n\n" + board.BuildBoardMessage(Context));
                    return;
                }
                else if (board.HasTwentyFortyEightTile)
                {
                    await DeleteOriginalResponseAsync();
                    await Context.Channel.SendMessageAsync("Win!\n\n" + board.BuildBoardMessage(Context));
                    return;
                }
                else
                {
                    await TwentyFortyEightContinueAsync(message, board);
                    continue;
                }
            }
        }
        private async Task TwentyFortyEightContinueAsync(IUserMessage message, Board board)
        {
            while (true)
            {
                var result = await _interactive.NextMessageComponentAsync(
                x => x.Message.Id == message.Id,
                timeout: TimeSpan.FromMinutes(5));

                if (result.IsTimeout)
                {
                    await DeleteOriginalResponseAsync();
                    await Context.Channel.SendMessageAsync("Timeout!\n\n" + board.BuildBoardMessage(Context));
                    return;
                }
                else if (Context.User.Id != result.Value.User.Id)
                {
                    var embed = new GudgeonEmbedBuilder()
                        .WithStyle(new ErrorStyle(), "Missing permissions")
                        .WithDescription($"• Only {Context.User.Mention} can control this menu.")
                        .WithFooter($"{result.Value.User.Username}#{result.Value.User.Discriminator}")
                        .Build();
                    await RespondAsync(embed: embed, ephemeral: true);
                    continue;
                }
                else
                {
                    board.ShiftBoard(result.Value.Data.CustomId);

                    await result.Value.UpdateAsync(x =>
                    {
                        x.Content = board.BuildBoardMessage(Context);
                        x.Components = BoardShiftingButtons;
                    });

                    break;
                }
            }
        }
    }
}