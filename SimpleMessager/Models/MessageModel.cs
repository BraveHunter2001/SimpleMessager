using DAL.Enities;

namespace SimpleMessagerApi.Models;

public class MessageModel
{
    public int OrderNumber { get; set; }
    public required string Text { get; set; }

    public Message ToMessage() => new Message
    {
        OrderNumber = OrderNumber,
        Text = Text,
    };
}