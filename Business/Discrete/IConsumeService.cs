namespace Business.Discrete
{
    public interface IConsumeService
    {
        Task RegisterViaRabbitMQAsync();
        Task RegisterViaActiveMQAsync();
        Task RegisterViaKafkaAsync();
    }
}
