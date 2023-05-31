namespace CreateTopicsAndSubscriptionsWithFilters
{
    using Azure.Messaging.ServiceBus.Administration;
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        // Service Bus Administration Client object to create topics and subscriptions
        static ServiceBusAdministrationClient adminClient;

        // connection string to the Service Bus namespace
        static readonly string connectionString = "Endpoint=sb://xuanan.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IhG/ARHE9f6palXyqsM9UusOCrq+w0bq6iuF1dgP+Hc=";

        // name of the Service Bus topic
        static readonly string topicName = "processedimage";

        // names of subscriptions to the topic
        static readonly string subscriptionAllImage = "AllImage";
        static readonly string subscriptionColorBlue = "blue-color";
        static readonly string subscriptionWhiteImage = "white-image";
        static readonly string subscriptionWhiteImageCorrelation = "white-image-correlation";

        public static async Task Main()
        {
            try
            {

                Console.WriteLine("Creating the Service Bus Administration Client object");
                adminClient = new ServiceBusAdministrationClient(connectionString);

                Console.WriteLine($"Creating the subscription {subscriptionAllImage} for the topic with a True filter ");
                // Create a True Rule filter with an expression that always evaluates to true
                // It's equivalent to using SQL rule filter with 1=1 as the expression
                await adminClient.CreateSubscriptionAsync(
                        new CreateSubscriptionOptions(topicName, subscriptionAllImage),
                        new CreateRuleOptions("AllImage", new TrueRuleFilter()));


                Console.WriteLine($"Creating the subscription {subscriptionColorBlue} with a SQL filter");
                // Create a SQL filter with color set to blue and quantity to 10
                await adminClient.CreateSubscriptionAsync(
                        new CreateSubscriptionOptions(topicName, subscriptionColorBlue),
                        new CreateRuleOptions("BlueImage", new SqlRuleFilter("color-blue > 100")));

                Console.WriteLine($"Creating the subscription {subscriptionWhiteImage} with a SQL filter");
                // Create a SQL filter with color equals to red and a SQL action with a set of statements
                await adminClient.CreateSubscriptionAsync(topicName, subscriptionWhiteImage);
                // remove the $Default rule
                await adminClient.DeleteRuleAsync(topicName, subscriptionWhiteImage, "$Default");
                // now create the new rule. notice that user. prefix is used for the user/application property
                await adminClient.CreateRuleAsync(topicName, subscriptionWhiteImage, new CreateRuleOptions
                {
                    Name = "WhiteImage",
                    Filter = new SqlRuleFilter("user.color-red = 0 and user.color-green = 0 and user.color-blue = 0"),
                    Action = new SqlRuleAction("set sys.CorrelationId = 'white-image'")

                }
                );

                Console.WriteLine($"Creating the subscription {subscriptionWhiteImageCorrelation} with a correlation filter");
                // Create a correlation filter with color set to Red and priority set to High
                await adminClient.CreateSubscriptionAsync(
                        new CreateSubscriptionOptions(topicName, subscriptionWhiteImageCorrelation),
                        new CreateRuleOptions("WhiteImage", new CorrelationRuleFilter() {CorrelationId = "white-image" }));

                // delete resources
                //await adminClient.DeleteTopicAsync(topicName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}