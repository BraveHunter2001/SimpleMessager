using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services;
using SimpleMessagerApi.Hubs;
using SimpleMessagerApi.Models;

namespace SimpleMessagerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController(
    IMessagesService messagesService,
    IHubContext<MessagesHub> hubContext
) : ControllerBase
{
    [HttpPost]
    public IActionResult SendMessage([FromBody] MessageModel messageModel)
    {
        if (messageModel.Text.Length > 128)
            return BadRequest("Text too large");

        var message = messageModel.ToMessage();
        var orderNumber = messagesService.SaveMessage(message,
            (m) => { hubContext.Clients.All.SendAsync("RecieveMessage", m); }
        );

        return Ok(orderNumber);
    }

    [HttpGet]
    public IActionResult GetMessage([FromQuery] TimeSpan period)
    {
        var messages = messagesService.GetMessagesByDatePeriod(period);

        return Ok(messages);
    }
}