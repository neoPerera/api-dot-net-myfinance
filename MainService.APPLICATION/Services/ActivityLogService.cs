using MainService.Application.DTOs;
using MainService.APPLICATION.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MainService.APPLICATION.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<ActivityLogService> _logger;

        public ActivityLogService(IKafkaProducerService kafkaProducer, ILogger<ActivityLogService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task LogAsync(ActivityLogMessage log, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(log);
            await _kafkaProducer.ProduceAsync(json, "activitylog", cancellationToken);
        }

        public Task Info(string message, string? userId = null, string? action = null) =>
            LogAsync(new ActivityLogMessage
            {
                Level = LogLevel.Information.ToString(),
                Message = message,
                UserId = userId,
                Action = action
            });

        public Task Warn(string message, string? userId = null, string? action = null) =>
            LogAsync(new ActivityLogMessage
            {
                Level = LogLevel.Warning.ToString(),
                Message = message,
                UserId = userId,
                Action = action
            });

        public Task Error(string message, string? userId = null, string? action = null) =>
            LogAsync(new ActivityLogMessage
            {
                Level = LogLevel.Error.ToString(),
                Message = message,
                UserId = userId,
                Action = action
            });

        public Task Debug(string message, string? userId = null, string? action = null)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                // Do nothing if debug logging is not enabled
                return Task.CompletedTask;
            }

            return LogAsync(new ActivityLogMessage
            {
                Level = LogLevel.Debug.ToString(),
                Message = message,
                UserId = userId,
                Action = action
            });
        }
    }
}
