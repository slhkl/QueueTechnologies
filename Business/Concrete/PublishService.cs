using ActiveMQ.Discrete;
using Business.Discrete;
using Data.Login;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class PublishService : IPublishService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitmqService;
        private readonly IActiveMQService _activeMQService;

        public PublishService(IRabbitMQService rabbitmqService, IActiveMQService activeMQService)
        {
            _rabbitmqService = rabbitmqService;
            _activeMQService = activeMQService;
        }

        public async Task RegisterViaRabbitMQAsync(Register register)
        {
            await _rabbitmqService.PublishAsync(RegisterQueueName, register);
        }

        public async Task RegisterViaActiveMQAsync(Register register)
        {
            await _activeMQService.PublishAsync(RegisterQueueName, register);
        }
    }
}
