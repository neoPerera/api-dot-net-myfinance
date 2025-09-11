
using Microsoft.Extensions.Logging;

namespace MainService.Application.DTOs
{
    public class ActivityLogMessage
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public LogLevel Level { get; set; }
        public string Service { get; set; } = "MainService";
        public string? UserId { get; set; }
        public string? Action { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Metadata { get; set; } = new { };
    }
}
