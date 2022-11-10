using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AFDemo.Trigger
{
    public class QueueTriggeredFunction
    {
        //https://medium.com/swlh/how-to-use-azure-queue-triggered-functions-and-why-7f651c9d3f8c
        //https://www.programmingwithwolfgang.com/azure-functions-process-queue-messages/
        //https://stackoverflow.com/questions/13087872/test-azure-service-bus-locally-without-any-subscription-or-login
        //[FunctionName("OrderNameUpdateFunction")]
        //public static void Run([ServiceBusTrigger("CustomerQueue", Connection = "YourConnectionString")] string myQueueItem, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        //}
    }
}