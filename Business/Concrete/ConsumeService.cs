using ActiveMQ.Discrete;
using Business.Discrete;
using Data.Login;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class ConsumeService : IConsumeService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IActiveMQService _activeMQService;

        public ConsumeService(IRabbitMQService rabbitMQService, IActiveMQService activeMQService)
        {
            _rabbitMQService = rabbitMQService;
            _activeMQService = activeMQService;
        }

        public async Task RegisterViaRabbitMQAsync()
        {
            await _rabbitMQService.ConsumeAsync<Register>(RegisterQueueName, e =>
            {
                SendMail(nameof(RegisterViaRabbitMQAsync), e);
            });
        }

        public async Task RegisterViaActiveMQAsync()
        {
            await _activeMQService.ConsumeAsync<Register>(RegisterQueueName, e =>
            {
                SendMail(nameof(RegisterViaActiveMQAsync), e);
            });
        }

        private void SendMail(string callerMethod, Register register)
        {
            Console.WriteLine($"{callerMethod}: {register.Name}, {register.Email}, {register.Password}");
        }
    }
}
