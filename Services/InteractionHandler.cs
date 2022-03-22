using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.Common;
using Gudgeon.Common.Styles;

namespace Gudgeon.Services
{
    internal class InteractionHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _service;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public InteractionHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider, InteractionService service, IHostEnvironment environment, IConfiguration configuration) : base(client, logger)
        {
            _provider = provider;
            _service = service;
            _environment = environment;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.InteractionCreated += HandleInteraction;
            Client.ApplicationCommandCreated += Client_ApplicationCommandCreated;

            _service.SlashCommandExecuted += SlashCommandExecuted;
            _service.ComponentCommandExecuted += ComponentCommandExecuted;
            _service.ModalCommandExecuted += ModalCommandExecuted;

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await Client.WaitForReadyAsync(stoppingToken);
            await RegisterCommands();
        }

        private Task Client_ApplicationCommandCreated(SocketApplicationCommand arg)
        {
            throw new NotImplementedException();
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var ctx = new SocketInteractionContext(Client, interaction);
                await _service.ExecuteCommandAsync(ctx, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred whilst attempting to handle interaction.");

                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    var message = await interaction.GetOriginalResponseAsync();
                    await message.DeleteAsync();
                }
            }
        }

        #region Error handling

        private async Task SlashCommandExecuted(SlashCommandInfo command, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await MissingPermissionsRespond(context, result.ErrorReason);
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task ComponentCommandExecuted(ComponentCommandInfo command, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await MissingPermissionsRespond(context, result.ErrorReason);
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task ModalCommandExecuted(ModalCommandInfo command, IInteractionContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await MissingPermissionsRespond(context, result.ErrorReason);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task MissingPermissionsRespond(IInteractionContext context, string errorReason)
        {
            var embed = new GudgeonEmbedBuilder()
                .WithStyle(new ErrorStyle(), "Missing permissions")
                .WithDescription($"• {errorReason}")
                .WithFooter($"{context.User.Username}#{context.User.Discriminator}")
                .Build();

            await context.Interaction.RespondAsync(embed: embed, ephemeral: true);
        }

        #endregion

        private async Task RegisterCommands()
        {
            if (_environment.IsDevelopment())
            {
                await _service.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("DevGuild"));
            }
            else
            {
                await _service.RegisterCommandsGloballyAsync();
            }
        }
    }
}