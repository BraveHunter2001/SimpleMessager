using DAL;
using DAL.Enities;

namespace Services;

public interface IMessagesService
{
    int SaveMessage(Message message, Action<Message>? onSend);
    List<Message> GetMessagesByDatePeriod(TimeSpan minutePeriod);
}

public class MessagesService(
    IMessagesRepository messagesRepository
) : IMessagesService
{
    public int SaveMessage(Message message, Action<Message>? onSend)
    {
        message.SentDateTime = DateTime.Now;

        messagesRepository.SaveMessage(message);
        onSend?.Invoke(message);

        return message.OrderNumber;
    }

    public List<Message> GetMessagesByDatePeriod(TimeSpan minutePeriod)
        => messagesRepository.GetMessagesByDatePeriod(minutePeriod);
}