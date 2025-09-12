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
                    [CallerFilePath] string filePath = "",
                    [CallerMemberName] string memberName = "",
                    [CallerLineNumber] int lineNumber = 0);

        Task Warn(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Error(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Debug(
            string message,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);

        Task Info<T>(
            string message,
            T? variable = default,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerArgumentExpression("variable")] string variableName = "");

        Task Debug<T>(
            string message,
            T? variable = default,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerArgumentExpression("variable")] string variableName = "");

        Task FlushAsync(string message,
            string? userId = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0);
        void ChangeLog<T>(T variable, [CallerArgumentExpression("variable")] string variableName = "");
    }
}
