using Confluent.Kafka;
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
                    var consumeData = consumer.Consume();
                    var message = JsonConvert.DeserializeObject<T>(consumeData.Message.Value);
                    if (message is not null)
                        action(message);
                }
            });
        }
    }
}
