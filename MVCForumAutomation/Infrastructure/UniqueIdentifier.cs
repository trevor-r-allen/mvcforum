using System;

namespace MVCForumAutomation.Infrastructure
{
    public static class UniqueIdentifier
    {
        public static string For(string entityType)
        {
            return $"{entityType}-{Guid.NewGuid()}";
        }
    }
}
