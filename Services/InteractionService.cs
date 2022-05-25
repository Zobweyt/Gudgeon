using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using Gudgeon.TypeConverters;
using Gudgeon.TypeReaders;

namespace Gudgeon.Services
{
    internal sealed partial class InteractionHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _service;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public InteractionHandler(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider, Discord.Interactions.InteractionService service, IHostEnvironment environment, IConfiguration configuration) : base(client, logger)
        {
            _provider = provider;
            _service = service;
            _environment = environment;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Client.InteractionCreated += InteractionCreatedAsync;
            _service.InteractionExecuted += InteractionExecutedAsync;

            _service.AddTypeConverter<ulong>(new UlongTypeConverter());
            _service.AddTypeReader<ulong>(new UlongTypeReader());

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

            await Client.WaitForReadyAsync(stoppingToken);
            await RegisterCommandsAsync();
        }

        private async Task RegisterCommandsAsync()
        {
            if (_environment.IsDevelopment())
            {
                await Client.Rest.DeleteAllGlobalCommandsAsync();
                await _service.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("DevGuild"));
            }
            else
            {
                await Client.Rest.BulkOverwriteGuildCommands(Array.Empty<ApplicationCommandProperties>(), _configuration.GetValue<ulong>("DevGuild"));
                await _service.RegisterCommandsGloballyAsync();
            }
        }
    }
}