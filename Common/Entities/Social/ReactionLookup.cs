namespace Common.Entities.Social
{
    public class ReactionLookup
    {
        public int ReactionLookupId { get; private set; }
        public string ReactionName { get; private set; }
        public string ReactionLogo { get; private set;}

        public List<Reaction> Reactions { get; set; }

        public ReactionLookup(string reactionName, string reactionLogo)
        {
            ReactionName = reactionName;
            ReactionLogo = reactionLogo;
        }

        protected ReactionLookup()
        {
        }
    }
}
