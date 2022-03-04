using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;

namespace Gudgeon.Services
{
    internal class InteractionHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _service;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;

        public InteractionHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider, InteractionService service, IHostEnvironment environment, IConfiguration configuration) : base(client, logger)
        {
            _provider = provider;
            _service = service;
            _environment = environment;
            _configuration = configuration;
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.InteractionCreated += HandleInteraction;

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await Client.WaitForReadyAsync(stoppingToken);
            
            if (_environment.IsDevelopment())
            {
                await _service.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("DevGuild"));
            }
            else
            {
                await _service.RegisterCommandsGloballyAsync();
            }

        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new SocketInteractionContext(Client, arg);
                await _service.ExecuteCommandAsync(ctx, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred whilst attempting to handle interaction.");

                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    var msg = await arg.GetOriginalResponseAsync();
                    await msg.DeleteAsync();
                }
            }
        }
    }

}