using MainService.Application.DTOs;
using System.Runtime.CompilerServices;

namespace MainService.APPLICATION.Interfaces
{
    /// <summary>
    /// Defines a service for logging activity messages.
    /// </summary>
    public interface IActivityLogService
    {
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
        Task Error<T>(
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
