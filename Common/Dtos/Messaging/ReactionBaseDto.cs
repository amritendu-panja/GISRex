namespace Common.Dtos.Messaging
{
    public class ReactionBaseDto
    {
        public int ReactionId { get; set; }
        public int ReactionType { get; set; }
        public int TargetMessageId { get; set; }
        public int TargetCommentId { get; set; }
    }
}
