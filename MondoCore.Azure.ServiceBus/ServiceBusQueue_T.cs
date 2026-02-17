using System.Text.Json;

using Azure.Messaging.ServiceBus;
using Azure.Core;

using MondoCore.Common;

namespace MondoCore.Azure.ServiceBus
{
    /// <summary>
    /// Provides an implementation of a message queue using Azure Service Bus for sending messages of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of message to be sent through the queue. Must be a reference type.</typeparam>
    public class ServiceBusQueue<T> : IMessageQueue<T>, IAsyncDisposable where T : class
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

        #region IMessageQueue<T>

        /// <summary>
        /// Sends a message to the ServiceBus message queue. If sendOn is specified, the message will be scheduled to be sent at the specified time.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sendOn"></param>
        /// <param name="correlationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Send(T message, DateTimeOffset? sendOn = null,string? correlationId = null, CancellationToken cancellationToken = default)
        {
            await using var sender = _client.CreateSender(_queueName);

            var json = JsonSerializer.Serialize(message);
            var msg  = new ServiceBusMessage(json);

            if(correlationId != null)
                msg.CorrelationId = correlationId;  

            if (sendOn == null)
                await sender.SendMessageAsync(msg, cancellationToken);
           else
                await sender.ScheduleMessageAsync(msg, sendOn!.Value, cancellationToken);
        }

        #endregion

        #region IAsyncDisposable

        public ValueTask DisposeAsync()
        {
            return _client.DisposeAsync();
        }

        #endregion
    }
}
