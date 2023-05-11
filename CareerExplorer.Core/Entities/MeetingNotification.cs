
namespace CareerExplorer.Core.Entities
{
    public class MeetingNotification
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? MeetingLink { get; set; }
        public bool IsAccepted { get; set; } = false;
        public DateTime Date { get; set; }
    }
}