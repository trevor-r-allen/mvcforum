using TestAutomationEssentials.Common.Configuration;

namespace MVCForumAutomation.Infrastructure
{
    public class TestEnvironment : ConfigurationBase
    {
        protected override string XmlNamespace
        {
            get { return "http://TestAutomation.MVCForum.com/TestEnvironment.xsd"; }
        }

        [ConfigurationParameter("http://localhost:8080/")]
        public string URL { get; set; }
    }
}