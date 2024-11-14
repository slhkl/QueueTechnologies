using ActiveMQ.Discrete;
using Business.Discrete;
using Data.Login;
using Kafka.Discrete;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class PublishService : IPublishService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitmqService;
        private readonly IActiveMQService _activeMQService;
        private readonly IKafkaService _kafkaService;

        public PublishService(IRabbitMQService rabbitmqService, IActiveMQService activeMQService, IKafkaService kafkaService)
        {
            _rabbitmqService = rabbitmqService;
            _activeMQService = activeMQService;
            _kafkaService = kafkaService;
        }

        public async Task RegisterViaRabbitMQAsync(Register register)
        {
            await _rabbitmqService.PublishAsync(RegisterQueueName, register);
        }

        public async Task RegisterViaActiveMQAsync(Register register)
        {
            await _activeMQService.PublishAsync(RegisterQueueName, register);
        }

        public async Task RegisterViaKafkaAsync(Register register)
        {
            await _kafkaService.PublishAsync(RegisterQueueName, register);
        }
    }
}
