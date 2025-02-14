using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Projekt_zaliczeniowy.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string logDirectory = "Logs"; // Folder na logi
        private readonly string logFilePath;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            logFilePath = Path.Combine(logDirectory, "requests.log");

            //  Sprawdzenie, czy folder istnieje, jeśli nie – utworzenie go
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var logMessage = $"[{DateTime.Now}] {request.Method} {request.Path} {request.QueryString}\n";

            //  Zapis do pliku w folderze `Logs/`
            await File.AppendAllTextAsync(logFilePath, logMessage);

            await _next(context); 
        }
    }
}
