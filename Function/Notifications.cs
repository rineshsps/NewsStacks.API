using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace NewsStacks.Function
{
    public static class Notifications
    {
        [FunctionName("Notifications")]
        public static void Run([QueueTrigger("qa1", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            //email notification 
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
