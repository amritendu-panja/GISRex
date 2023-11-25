namespace Common.Dtos.Messaging
{
    public class MessageBaseDto
    {
        public Guid MessageId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid AuthorGuid { get; set; }
        public string AuthorName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

    }
}
