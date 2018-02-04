namespace MVCForumAutomation
{
    public class ScreenshotEventArgs
    {
        public string Filename { get; }

        public ScreenshotEventArgs(string filename)
        {
            Filename = filename;
        }
    }
}