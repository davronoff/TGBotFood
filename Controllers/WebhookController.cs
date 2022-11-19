using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TGBotFood.Services;

namespace TGBotFood.Controllers
{   
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HandleUpdateService handleUpdate,
                                            [FromBody] Update update)
        {
            await handleUpdate.EchoAsync(update);
            return Ok();

        }
    }
}
