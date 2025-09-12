namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(string message,string topic, CancellationToken cancellationToken = default);
    }
}
