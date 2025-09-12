using CommonLogWorker.Models;
using Confluent.Kafka;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
namespace CommonLogWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMongoCollection<ActivityLog> _collection;
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly string _groupId;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;

            _bootstrapServers = config["Kafka:BootstrapServers"];
            _topic = config["Kafka:Topic"];
            var mongoClient = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = mongoClient.GetDatabase(config["MongoDB:Database"]);
            _collection = database.GetCollection<ActivityLog>(config["MongoDB:Collection"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "activitylog-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_topic);

            _logger.LogInformation("KafkaMongoLogWorker started, consuming messages on topic: {topic} ...", _topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result != null)
                    {
                        _logger.LogInformation("Received: {msg}", result.Message.Value);

                        // Parse JSON
                        var json = JObject.Parse(result.Message.Value);

                        var log = new ActivityLog
                        {
                            Timestamp = json["Timestamp"]?.Value<DateTime>() ?? DateTime.UtcNow,
                            Level = json["Level"]?.Value<string>() ?? "INFO",
                            Service = json["Service"]?.Value<string>() ?? "TestLogger",
                            UserId = json["UserId"]?.Value<string>(),
                            Action = json["Action"]?.Value<string>() ?? string.Empty,
                            Message = json["Message"]?.Value<string>() ?? string.Empty,
                            Metadata = json["Metadata"] != null
                                ? BsonDocument.Parse(json["Metadata"].ToString())
                                : null
                        };

                        await _collection.InsertOneAsync(log);
                        _logger.LogInformation("Inserted structured log into MongoDB");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming Kafka message");
                }
            }
        }
    }
}
