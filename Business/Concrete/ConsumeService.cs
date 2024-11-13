using Business.Discrete;
using Data.Login;
using RabbitMQ.Discrete;

namespace Business.Concrete
{
    public class ConsumeService : IConsumeService
    {
        private readonly string RegisterQueueName = "Register";
        private readonly IRabbitMQService _rabbitMQService;

        public ConsumeService(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public async Task RegisterViaRabbitMQAsync()
        {
            await _rabbitMQService.ConsumeAsync<Register>(RegisterQueueName, e =>
            {
                Console.WriteLine($"{e.Name}, {e.Email}, {e.Password}");
            });
        }
    }
}
