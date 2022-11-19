using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TGBotFood.Models;

namespace TGBotFood.Services;

public class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _botConfig;

    public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
                            IServiceProvider serviceProvider,
                            IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAdress = $@"{_botConfig.HostAdress}/bot/{_botConfig.Token}";

        _logger.LogInformation("setting webhook");

        await botClient.SendTextMessageAsync(
           chatId: 1228455041,
           text: "Your bot fucking working");

        await botClient.SetWebhookAsync(
            url:webhookAdress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        _logger.LogInformation("WebHook Removing");

        await botClient.SendTextMessageAsync(
            chatId: 1228455041,
            text: "What the fuck is going on");
    }
}
