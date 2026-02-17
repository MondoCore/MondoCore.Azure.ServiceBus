
using Azure.Identity;

using MondoCore.Azure.TestHelpers;

namespace MondoCore.Azure.ServiceBus.FunctionalTests
{
    [TestClass]
    public sealed class ServiceBusQueue_TTests
    {
        [TestMethod]
        public async Task ServiceBusQueue_Send()
        {
            var config = TestConfiguration.Load();

            var sb = new ServiceBusQueue<Car>(config.ConnectionString, "test");

            await sb.Send(new Car { Make = "Chevy", Model = "Corvette", Year = 1956 });
        }
    }

    file class Car     
    {
        public string Make  { get; set; } = "";
        public string Model { get; set; } = "";
        public int    Year  { get; set; }
    }
}
