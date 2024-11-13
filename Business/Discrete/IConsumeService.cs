namespace Business.Discrete
{
    public interface IConsumeService
    {
        Task RegisterViaRabbitMQAsync();
    }
}
