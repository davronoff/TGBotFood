using Telegram.Bot;
using TGBotFood.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services.AddScoped<HandleUpdateService>();
builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
                    new TelegramBotClient(builder.Configuration.GetSection("BotConfiguration:Token").Value , httpClient));

builder.Services.AddSwaggerGen();
////builder.Services.AddDbContext<>
///
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseEndpoints(endpoints =>
{
    var token = builder.Configuration.GetSection("BotConfiguration:Token").Value;
    endpoints.MapControllerRoute(name: "tgwebhook",
        pattern: $"bot/{token}",
        new { controller = "Webhook", action = "Post" });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
