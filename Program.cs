using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Fergun.Interactive;
using Gudgeon.Services;

namespace Gudgeon
{
    internal class Program
    {
        private static async Task Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureDiscordHost((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 200,
                        GatewayIntents = GatewayIntents.All
                    };

                    if (string.IsNullOrEmpty(context.Configuration["Token"]))
                    {
                        throw new ArgumentException("The token is null or empty. Enter a valid token in appsettings.json or appsettings.Development.json.");
                    }

                    config.Token = context.Configuration["Token"];
                })
                .UseInteractionService((context, config) =>
                {
                    config.LogLevel = LogSeverity.Debug;
                    config.UseCompiledLambda = true;
                })
                .ConfigureServices((context, services) =>
                {
                    services
                    .AddHostedService<InteractionHandler>()
                    .AddHostedService<BotStatusService>()
                    .AddTransient<InteractiveService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}