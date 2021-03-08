namespace NewsStacks.Settings
{
    public class AppSettings
    {
        public string Environment { get; set; }
        public bool EnableSwagger { get; set; }
        public string NewsContext { get; set; }
        public string AzureWebJobsStorage { get; set; }
        public string QueueName { get; set; }
    }
}
