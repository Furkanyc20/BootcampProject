using System;

namespace BootcampProject.Core.Middleware
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public ErrorResponse(string message, int statusCode, string details = null) : this()
        {
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }
    }
}