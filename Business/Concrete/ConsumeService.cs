using ActiveMQ.Discrete;
using Business.Discrete;
using Data.Login;
using Kafka.Discrete;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class ConsumeService : IConsumeService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IActiveMQService _activeMQService;
        private readonly IKafkaService _kafkaService;

        public ConsumeService(IRabbitMQService rabbitMQService, IActiveMQService activeMQService, IKafkaService kafkaService)
        {
            _rabbitMQService = rabbitMQService;
            _activeMQService = activeMQService;
            _kafkaService = kafkaService;
        }

        public async Task RegisterViaRabbitMQAsync()
        {
            await _rabbitMQService.ConsumeAsync<Register>(RegisterQueueName, e => SendMail(nameof(RegisterViaRabbitMQAsync), e));
        }

        public async Task RegisterViaActiveMQAsync()
        {
            await _activeMQService.ConsumeAsync<Register>(RegisterQueueName, e => SendMail(nameof(RegisterViaActiveMQAsync), e));
        }

        public async Task RegisterViaKafkaAsync()
        {
            await _kafkaService.ConsumeAsync<Register>(RegisterQueueName, e => SendMail(nameof(RegisterViaKafkaAsync), e));
        }

        private void SendMail(string callerMethod, Register register)
        {
            Console.WriteLine($"{callerMethod}: {register.Name}, {register.Email}, {register.Password}");
        }
    }
}
