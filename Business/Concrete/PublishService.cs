using Business.Discrete;
using Data.Login;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class PublishService : IPublishService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitmqService;

        public PublishService(IRabbitMQService rabbitmqService)
        {
            _rabbitmqService = rabbitmqService;
        }

        public async Task RegisterViaRabbitMQAsync(Register register)
        {
            await _rabbitmqService.PublishAsync(RegisterQueueName, register);
        }
    }
}
