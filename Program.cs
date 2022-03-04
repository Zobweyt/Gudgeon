using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Gudgeon.Services;

namespace Gudgeon
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
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
                    .AddHostedService<BotStatusService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}