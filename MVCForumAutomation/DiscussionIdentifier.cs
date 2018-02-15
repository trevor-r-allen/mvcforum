namespace MVCForumAutomation
{
    public class DiscussionIdentifier
    {
        public DiscussionIdentifier(string title)
        {
            Title = title;
        }

        public string Title { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var other = obj as DiscussionIdentifier;
            return other != null && Equals(other);
        }

        protected bool Equals(DiscussionIdentifier other)
        {
            return string.Equals(Title, other.Title);
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}