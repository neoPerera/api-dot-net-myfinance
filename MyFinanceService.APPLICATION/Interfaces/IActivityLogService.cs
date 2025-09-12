using MyFinanceService.Application.DTOs;
using System.Runtime.CompilerServices;

namespace MyFinanceService.APPLICATION.Interfaces
{
    /// <summary>
    /// Defines a service for logging activity messages.
    /// </summary>
    public interface IActivityLogService
    {

        /// <summary>
        /// Convenience methods for common log levels
        /// </summary>
        Task Info(
                    string message,
                    string? userId = null,
                    [CallerFilePath] string filePath = "",
                    [CallerMemberName] string memberName = "",
                    [CallerLineNumber] int lineNumber = 0);

        Task Warn(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Error(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Debug(
            string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Debug<T>(
            string message,
            T? variable = default,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerArgumentExpression("variable")] string variableName = "");
    }
}
