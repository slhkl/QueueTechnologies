using ActiveMQ.Discrete;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;

namespace ActiveMQ.Concrete
{
    public class ActiveMQService : IActiveMQService, IDisposable
    {
        #region Members

        private readonly string ActiveMQUri = Environment.GetEnvironmentVariable("ACTIVEMQ_URI");
        private IConnection connection;

        #endregion

        public async Task ConsumeAsync<T>(string queueName, Action<T> action)
        {
            var (session, queue) = await GetQueueAsync(queueName);
            var consumer = await session.CreateConsumerAsync(queue);

            consumer.Listener += (IMessage message) =>
            {
                if (message is ITextMessage text)
                {
                    var data = JsonConvert.DeserializeObject<T>(text.Text);
                    if (data is not null)
                        action(data);
                }
            };
        }

        public async Task PublishAsync<T>(string queueName, T data)
        {
            var (session, queue) = await GetQueueAsync(queueName);
            var producer = await session.CreateProducerAsync(queue);

            var jsonData = JsonConvert.SerializeObject(data);
            var message = await session.CreateTextMessageAsync(jsonData);
            await producer.SendAsync(message);
        }

        private async Task<(ISession, IQueue)> GetQueueAsync(string queueName)
        {
            var session = await GetSessionAsync();
            return (session, await session.GetQueueAsync(queueName));
        }

        private async Task<ISession> GetSessionAsync()
        {
            await GetConnectionAsync();
            await connection.StartAsync();
            return await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge);
        }

        private async Task GetConnectionAsync()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                BrokerUri = new Uri(ActiveMQUri)
            };

            connection = await connectionFactory.CreateConnectionAsync();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
