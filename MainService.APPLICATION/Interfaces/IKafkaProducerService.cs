namespace MainService.APPLICATION.Interfaces
{
    public interface IKafkaProducerService
    {
        Task ProduceAsync(string message, CancellationToken cancellationToken = default);
    }
}
