using ActiveMQ.Concrete;
using ActiveMQ.Discrete;
using Business.Concrete;
using Business.Discrete;
using Kafka.Concrete;
using Kafka.Discrete;
using RabbitMQ.Concrete;
using RabbitMQ.Discrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPublishService, PublishService>();
builder.Services.AddScoped<IConsumeService, ConsumeService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IActiveMQService, ActiveMQService>();
builder.Services.AddScoped<IKafkaService, KafkaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

ThreadPool.QueueUserWorkItem(async delegate
{
    using var scope = app.Services.CreateAsyncScope();
    var consumeService = scope.ServiceProvider.GetRequiredService<IConsumeService>();
    await consumeService.RegisterViaRabbitMQAsync();
    await consumeService.RegisterViaActiveMQAsync();
    await consumeService.RegisterViaKafkaAsync();
});

app.Run();
