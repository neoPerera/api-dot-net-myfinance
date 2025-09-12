
using Confluent.Kafka;
using MyFinanceService.APPLICATION.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MyFinanceService.APPLICATION.Services
{
    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"];

            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync(string message,string topic, CancellationToken cancellationToken = default)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
