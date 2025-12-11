namespace BizCompany.API.DTOs
{
    public class GetMessageDto
    {
        public int Id { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
