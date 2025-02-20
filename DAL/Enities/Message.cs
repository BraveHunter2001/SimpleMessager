namespace DAL.Enities;

public class Message
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public required string Text { get; set; }
    public DateTime SentDateTime { get; set; }
}