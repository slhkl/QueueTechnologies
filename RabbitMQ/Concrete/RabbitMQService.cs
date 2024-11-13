using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Discrete;
using System.Text;

namespace RabbitMQ.Concrete
{
    public class RabbitMQService : IRabbitMQService
    {
        #region Members

        private readonly string RabbitMQUri = Environment.GetEnvironmentVariable("RABBITMQ_URI");

        #endregion

        public async Task ConsumeAsync<T>(string queueName, Action<T> action)
        {
            var queue = await GetQueueAsync(queueName);

            var consumer = new AsyncEventingBasicConsumer(queue);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var bytesJsonData = ea.Body.ToArray();
                var jsonData = Encoding.UTF8.GetString(bytesJsonData);
                var data = JsonConvert.DeserializeObject<T>(jsonData);

                if (data is not null)
                    action(data);

                return Task.CompletedTask;
            };

            await queue.BasicConsumeAsync(queueName, true, consumer);
        }

        public async Task PublishAsync<T>(string queueName, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var bytesJsonData = Encoding.UTF8.GetBytes(jsonData);

            var queue = await GetQueueAsync(queueName);
            await queue.BasicPublishAsync(string.Empty, queueName, bytesJsonData);
        }

        private async Task<IChannel> GetQueueAsync(string queueName)
        {
            var channel = await GetChannelAsync();
            await channel.QueueDeclareAsync(queueName, true, false, false);
            return channel;
        }

        private async Task<IChannel> GetChannelAsync()
        {
            var connection = await GetConnectionAsync();
            return await connection.CreateChannelAsync();
        }

        private async Task<IConnection> GetConnectionAsync()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(RabbitMQUri)
            };

            return await connectionFactory.CreateConnectionAsync();
        }
    }
}
