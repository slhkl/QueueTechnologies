using Data.Login;

namespace Business.Discrete
{
    public interface IPublishService
    {
        Task RegisterViaRabbitMQAsync(Register register);
        Task RegisterViaActiveMQAsync(Register register);
    }
}
