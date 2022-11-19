using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGBotFood.Services;

public class HandleUpdateService
{
    private readonly ILogger _logger;
    private readonly ITelegramBotClient _botClient;

    public HandleUpdateService(ILogger logger,  
                               ITelegramBotClient botClient)
    {
        _logger = logger;
        _botClient = botClient;
    }
    public async  Task EchoAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageRecieved(update.Message),
            UpdateType.CallbackQuery => BotOnCallbackQueryRecieved(update.CallbackQuery),
            _ => UnknownUpdateTypeHandler(update)
        };
        try
        {
            await handler;
        }
        catch(Exception ex)
        {
            await HandlerErrorAsync(ex);
        }
    }

    private Task UnknownUpdateTypeHandler(Update update)
    {
        _logger.LogInformation($"Unknown update type : {update.Type}");

        return Task.CompletedTask;
    }

    private async Task BotOnCallbackQueryRecieved(CallbackQuery callbackQuery)
    {
        await _botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: $"{callbackQuery.Data}");
    }

    private async Task  BotOnMessageRecieved(Message message)
    {
        _logger.LogInformation($"Recieve Message {message.Type}");
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Recieve message bot");
    }
    public Task HandlerErrorAsync(Exception ex)
    {
        object ErrorMessage = ex switch
        {
            ApiRequestException apiRequest => $"Telegram API Error:\n{apiRequest.ErrorCode}",
            _ => ex.ToString
        };
            
        _logger.LogInformation(ErrorMessage.ToString());

        return Task.CompletedTask;
    }
}
