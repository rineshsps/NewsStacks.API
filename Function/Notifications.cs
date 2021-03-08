using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NewsStacks.DTOs;
using NewsStacks.DTOs.Enum;
using System.Text.Json;

namespace NewsStacks.Function
{
    public static class Notifications
    {
        [FunctionName("Notifications")]
        public static void Run([QueueTrigger("qa1", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            //email notification 
            log.LogInformation($"Queue trigger function processed: {myQueueItem}");

            var messsage = JsonSerializer.Deserialize<ArticleMessage>(myQueueItem);

            if (messsage?.MessageType == MessageType.WriterDone)
            {
                //Send mail to Reviwer
                //Send mail to Admin
                log.LogInformation($"Send mail to Reviwer, Admin");
            }
            else if (messsage?.MessageType == MessageType.ReviewerReject)
            {
                //Send mail to writer
                //Send mail to Admin
                log.LogInformation($"Send mail to writer, Admin");

            }
            else if (messsage?.MessageType == MessageType.ReviewerDone)
            {
                //Send mail to editor
                //Send mail to Admin
                log.LogInformation($"Send mail to editor, Admin");

            }
            else if (messsage?.MessageType == MessageType.EditorDone)
            {
                //Send mail to publisher
                //Send mail to Admin
                log.LogInformation($"Send mail to publisher, Admin");

            }
            else if (messsage?.MessageType == MessageType.PublisherDone)
            {
                //Send mail to Admin
                //Send mail to Writer,Reviwer, editor
                //Send mail to User

                log.LogInformation($"Send mail to Writer,Reviwer, editor, Admin & users");

                //If user is DND mode off, then dont' send mail 

            }

        }
    }
}
