using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Kafka.Discrete;
using Newtonsoft.Json;

namespace Kafka.Concrete
{
    public class KafkaService : IKafkaService
    {
        #region Members 

        private readonly string KafkaUri = Environment.GetEnvironmentVariable("KAFKA_URI");

        #endregion

        public async Task PublishAsync<T>(string topicName, T data)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = KafkaUri
            };

            var jsonData = JsonConvert.SerializeObject(data);

            var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(topicName, new Message<Null, string> { Value = jsonData });
        }

        public async Task ConsumeAsync<T>(string topicName, Action<T> action)
        {
            await CheckTopicAsync(topicName);

            var config = new ConsumerConfig()
            {
                BootstrapServers = KafkaUri,
                GroupId = topicName
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(topicName);

            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var consumeData = consumer.Consume();
                        var message = JsonConvert.DeserializeObject<T>(consumeData.Message.Value);
                        if (message is not null)
                            action(message);
                    }
                    catch
                    {
                        Task.Delay(TimeSpan.FromSeconds(2));
                    }
                }
            });
        }

        private async Task CheckTopicAsync(string topicName)
        {
            var adminClient = new AdminClientBuilder(
                new AdminClientConfig
                {
                    BootstrapServers = KafkaUri
                }
            ).Build();

            var metaData = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
            bool topicExists = metaData.Topics.Exists(t => t.Topic == topicName);

            if (!topicExists)
            {
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topicName
                    }
                });
            }
        }
    }
}
