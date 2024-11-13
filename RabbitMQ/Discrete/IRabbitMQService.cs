namespace RabbitMQ.Discrete
{
    public interface IRabbitMQService
    {
        Task PublishAsync<T>(string queueName, T data);
        Task ConsumeAsync<T>(string queueName, Action<T> action);
    }
}
