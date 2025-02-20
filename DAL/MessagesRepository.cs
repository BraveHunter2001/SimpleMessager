using DAL.Enities;
using Npgsql;

namespace DAL;

public interface IMessagesRepository
{
    void SaveMessage(Message message);
    List<Message> GetMessagesByDatePeriod(TimeSpan interval);
}

internal class MessagesRepository(NpgsqlDataSource dataSource) : IMessagesRepository
{
    public void SaveMessage(Message message)
    {
        using var command = dataSource.CreateCommand();
        command.CommandText = """
                              INSERT INTO "Messages"
                                  ("OrderNumber", "Text", "SentDateTime") VALUES
                                  (@OrderNumber, @Text, @SentDateTime) RETURNING "Id"
                              """;

        command.Parameters.AddWithValue("OrderNumber", message.OrderNumber);
        command.Parameters.AddWithValue("Text", message.Text);
        command.Parameters.AddWithValue("SentDateTime", message.SentDateTime);

        var id = command.ExecuteScalar();

        message.Id = (int) (id ?? 0);
    }

    public List<Message> GetMessagesByDatePeriod(TimeSpan interval)
    {
        var startDate = DateTime.Now - interval;

        using var command = dataSource.CreateCommand();
        command.CommandText = """
                              SELECT "Id", "OrderNumber", "Text", "SentDateTime"
                              FROM "Messages"
                              where "SentDateTime" >= @startDate
                              ORDER BY "SentDateTime" DESC;
                              """;
        command.Parameters.AddWithValue("startDate", startDate);
        using var reader = command.ExecuteReader();

        List<Message> messages = new();
        while (reader.Read())
        {
            messages.Add(new Message
            {
                Id = (int) reader["Id"],
                OrderNumber = (int) reader["OrderNumber"],
                Text = (string) reader["Text"],
                SentDateTime = (DateTime) reader["SentDateTime"]
            });
        }

        return messages;
    }
}