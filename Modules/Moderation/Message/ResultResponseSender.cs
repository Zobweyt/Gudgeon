using Discord;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Styles;
using Gudgeon.Modules.Moderation.Message.Addons;

namespace Gudgeon.Modules.Moderation.Message
{
    class ResultResponseSender
    {
        private static readonly string[] ErrorMessages =
        {
            "The message is invalid.",
            "The author of the message is not the bot.",
            "The message does not contain only 1 embed.",
            "The message received as a result of command execution."
        };

        public static async Task HandleErrorAsync(SocketInteraction interaction, MessageEditingError? error)
        {
            await ErrorResponseAsync(interaction, ErrorMessages[(int)error]);
        }

        private static async Task ErrorResponseAsync(SocketInteraction interaction, string description)
        {
            Embed embed = new GudgeonEmbedBuilder()
                .WithStyle(new ErrorStyle())
                .WithDescription(description)
                .Build();

            await interaction.RespondAsync(embed: embed, ephemeral: true);
        }

        public static async Task SuccessResponseAsync(SocketInteraction interaction)
        {
            Embed embed = new GudgeonEmbedBuilder()
                .WithStyle(new SuccessStyle())
                .WithDescription("Embed has been successfully processed.")
                .Build();

            await interaction.RespondAsync(embed: embed, ephemeral: true);
        }
    }
}