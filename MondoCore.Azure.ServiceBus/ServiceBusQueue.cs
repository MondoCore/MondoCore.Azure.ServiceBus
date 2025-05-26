using MondoCore.Common;

using Azure.Messaging.ServiceBus;
using Azure.Core;

namespace MondoCore.Azure.ServiceBus
{
    public class ServiceBusQueue : IMessageQueue, IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public ServiceBusQueue(string connectionString, string queueOrTopicName)
        {
            _client = new ServiceBusClient(connectionString);
            _queueName = queueOrTopicName;
        }
        
        public ServiceBusQueue(string uri, string queueOrTopicName, TokenCredential credential)
        {
            _client = new ServiceBusClient(uri, credential);
            _queueName = queueOrTopicName;
        }
        
        #region IMessageQueue

        public Task Delete(IMessage message)
        {
            throw new NotSupportedException();
        }

        public Task<IMessage> Retrieve()
        {
            throw new NotSupportedException();
        }

        public async Task Send(string message, DateTimeOffset? sendOn = null)
        {
            await using var sender = _client.CreateSender(_queueName);

            var msg = new ServiceBusMessage(message);

            if(sendOn == null)
                await sender.SendMessageAsync(msg);
           else
                await sender.ScheduleMessageAsync(msg, sendOn!.Value);
        }

        #endregion

        #region IAsyncDisposable

        public ValueTask DisposeAsync()
        {
            return _client.DisposeAsync();;
        }

        #endregion
    }
}
