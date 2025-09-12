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
        private readonly AsyncLocal<Dictionary<string, object?>> _currentChangeLog = new();
        private readonly AsyncLocal<Dictionary<string, int>> _changeCounters = new();

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
                UserId = GetUserIdFromJwt(),
                Action = action,
                Metadata = metaData
            });
        }

        public Task Info(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Information.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber);



        public Task Warn(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Warning.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber);

        public Task Error(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0) =>
            Log(LogLevel.Error.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber);

        public Task Debug(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return Task.CompletedTask;

            return Log(LogLevel.Debug.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber);
        }
        public Task Info<T>(
        string message,
        T? variable = default,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerArgumentExpression("variable")] string variableName = "") =>
        Log(LogLevel.Information.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber, GetMetaDate(variableName, variable));

        public Task Debug<T>(
            string message,
            T? variable = default,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerArgumentExpression("variable")] string variableName = "")
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return Task.CompletedTask;


            return Log(LogLevel.Debug.ToString(), message, Path.GetFileName(filePath), memberName, lineNumber, GetMetaDate(variableName, variable));
        }

        public Task Error<T>(
        T? variable = default,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerArgumentExpression("variable")] string variableName = "") =>
        Log(LogLevel.Error.ToString(), "An Error occured while execution", Path.GetFileName(filePath), memberName, lineNumber, GetMetaDate(variableName, variable));

        public void ChangeLog<T>(T variable, [CallerArgumentExpression("variable")] string variableName = "")
        {
            if (_currentChangeLog.Value == null)
                _currentChangeLog.Value = new Dictionary<string, object?>();

            if (!_currentChangeLog.Value.ContainsKey(variableName))
                _currentChangeLog.Value[variableName] = new Dictionary<int, object?>();

            var snapshots = (Dictionary<int, object?>)_currentChangeLog.Value[variableName]!;

            var index = snapshots.Count;

            // deep copy to avoid reference mutation
            snapshots[index] = DeepCopy(variable);
        }
        public async Task FlushAsync(string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (_currentChangeLog.Value == null || !_currentChangeLog.Value.Any())
                return;

            var metaData = _currentChangeLog.Value;

            // Clear the temporary storage for next session
            _currentChangeLog.Value = null;

            // Send to log
            await LogAsync(new ActivityLogMessage
            {
                Level = LogLevel.Information.ToString(),
                Message = message,
                UserId = userId ?? GetUserIdFromJwt(),
                Action = $"{Path.GetFileName(filePath)}::{memberName} line {lineNumber}",
                Metadata = metaData
            });
        }

        private object GetMetaDate<T>(string variableName, T? variable)
        {
            var metadata = new Dictionary<string, object?> { { variableName, variable } };
            return JsonConvert.SerializeObject(metadata);
        }
        private T DeepCopy<T>(T source)
        {
            if (source == null)
                return default!;

            var json = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(json)!;
        }
    }
}
