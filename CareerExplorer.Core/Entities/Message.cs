namespace CareerExplorer.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeSent { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}