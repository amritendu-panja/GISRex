namespace Common.Dtos.Messaging
{
    public class CommentBaseDto
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid TargetMessageId { get; set; }
        public Guid TargetCommentId { get; set; }
    }
}
