﻿using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;

namespace Gudgeon.Services
{
    public class BotStatusService : DiscordClientService
    {
        public BotStatusService(DiscordSocketClient client, ILogger<DiscordClientService> logger) : base(client, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);
            Logger.LogInformation("Client is ready!");

            await Client.SetGameAsync("/help", type: ActivityType.Watching);
        }
    }
}