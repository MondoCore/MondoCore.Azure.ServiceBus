using Azure.Identity;

using MondoCore.Azure.TestHelpers;

namespace MondoCore.Azure.ServiceBus.FunctionalTests
{
    [TestClass]
    public sealed class ServiceBusQueueTests
    {
        [TestMethod]
        public async Task ServiceBusQueue_Send()
        {
            var config = TestConfiguration.Load();

            var sb = new ServiceBusQueue(config.ConnectionString, "test");

            await sb.Send("Bob's your uncle");
        }
    }
}
