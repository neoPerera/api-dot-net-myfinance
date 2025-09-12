using MyFinanceService.Application.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using System.IO;

namespace MyFinanceService.APPLICATION.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<ActivityLogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLogService(
            IKafkaProducerService kafkaProducer,
            ILogger<ActivityLogService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetUserIdFromJwt()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
                return null;

            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user.FindFirst("sub")?.Value
                   ?? user.Identity?.Name
                   ?? "UNKNOWN";
        }

        private async Task LogAsync(ActivityLogMessage log, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(log);
            await _kafkaProducer.ProduceAsync(json, "activitylog", cancellationToken);
        }

        private Task Log(
            string level,
            string message,
            string? userId,
            string fileName,
            string memberName,
            int lineNumber,
            object metaData = null)
        {
            var action = $"{fileName}::{memberName} line {lineNumber}";

            return LogAsync(new ActivityLogMessage
            {
                Level = level,
                Message = message,
                UserId = userId ?? GetUserIdFromJwt(),
                Action = action,
                Metadata = metaData
            });
        }

        public Task Info(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Information.ToString(), message, userId, Path.GetFileName(filePath), memberName, lineNumber);

        public Task Warn(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Warning.ToString(), message, userId, Path.GetFileName(filePath), memberName, lineNumber);

        public Task Error(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Error.ToString(), message, userId, Path.GetFileName(filePath), memberName, lineNumber);

        public Task Debug<T>(
            string message,
            T? variable = default,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerArgumentExpression("variable")] string variableName = "")
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return Task.CompletedTask;

            var metadata = new Dictionary<string, object?> { { variableName, variable } };
            var metaJson = JsonConvert.SerializeObject(metadata);

            return Log(LogLevel.Debug.ToString(), message, userId, Path.GetFileName(filePath), memberName, lineNumber, metaJson);
        }

        public Task Debug(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return Task.CompletedTask;

            return Log(LogLevel.Debug.ToString(), message, userId, Path.GetFileName(filePath), memberName, lineNumber);
        }
    }
}
