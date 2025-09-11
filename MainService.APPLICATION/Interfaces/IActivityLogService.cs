using MainService.Application.DTOs;

namespace MainService.APPLICATION.Interfaces
{
    /// <summary>
    /// Defines a service for logging activity messages.
    /// </summary>
    public interface IActivityLogService
    {
        /// <summary>
        /// Logs a structured activity message.
        /// </summary>
        /// <param name="log">The log message object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task LogAsync(ActivityLogMessage log, CancellationToken cancellationToken = default);

        /// <summary>
        /// Convenience methods for common log levels
        /// </summary>
        Task Info(string message, string? userId = null, string? action = null);
        Task Warn(string message, string? userId = null, string? action = null);
        Task Error(string message, string? userId = null, string? action = null);
        Task Debug(string message, string? userId = null, string? action = null);
    }
}
