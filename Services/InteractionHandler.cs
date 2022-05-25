using Discord;
using Discord.Addons.Hosting;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Styles;

namespace Gudgeon.Services
{
    internal sealed partial class InteractionHandler : DiscordClientService
    {
        private static readonly string?[] ErrorEmbedNames =
        {
            null, // UnknownCommand
            "Convert Failed",
            null, // BadArgs
            "Exception",
            null, // Unsuccessful
            "Unmet Precondition",
            null, // ParseFailed
        };
        private static readonly string?[] ErrorEmbedFooters =
        {
            null, // UnknownCommand
            "Please check the provided parameters", // ConvertFailed
            null, // BadArgs
            "If that keeps happening again, please contact us by using /feedback", // Exception
            null, // Unsuccessful
            null, // Unmet Precondition
            null, // ParseFailed
        };

        private async Task InteractionCreatedAsync(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(Client, interaction);
                await _service.ExecuteCommandAsync(context, _provider);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Exception occurred whilst attempting to handle interaction.");

                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    var message = await interaction.GetOriginalResponseAsync();
                    await message.DeleteAsync();
                }
            }
        }
        private async Task InteractionExecutedAsync(ICommandInfo command, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                int error = (int)result.Error;
                string? name = ErrorEmbedNames[error];

                if (name != null)
                {
                    await ErrorResponseAsync(context, name, result.ErrorReason, ErrorEmbedFooters[error]);
                }
            }
        }

        private async Task ErrorResponseAsync(IInteractionContext context, string name, string description, string? footer = null)
        {
            var embed = new GudgeonEmbedBuilder()
                .WithStyle(new ErrorStyle(), name)
                .WithDescription(description)
                .WithFooter(footer)
                .Build();

            await context.Interaction.RespondAsync(embed: embed, ephemeral: true);
        }
    }
}