namespace ActiveMQ.Discrete
{
    public interface IActiveMQService
    {
        Task ConsumeAsync<T>(string queueName, Action<T> action);
        Task PublishAsync<T>(string queueName, T data);
    }
}
