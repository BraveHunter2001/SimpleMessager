using DAL.Enities;

namespace DAL;

internal class StupidContext
{
    public List<Message> Messages { get; set; }

    public StupidContext()
    {
        Messages = new List<Message>();
    }
}