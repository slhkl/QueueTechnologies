namespace Kafka.Discrete
{
    public interface IKafkaService
    {
        Task PublishAsync<T>(string topicName, T data);
        Task ConsumeAsync<T>(string topicName, Action<T> action);
    }
}
