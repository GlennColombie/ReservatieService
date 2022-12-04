namespace ReservatieServiceBeheerderRESTService
{
    public class LoggerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public LoggerMiddleware(ILoggerFactory logger, RequestDelegate next)
        {
            _logger = logger.AddFile("Logs/Beheerder").CreateLogger<LoggerMiddleware>();
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation("Request {method} => {statuscode}",
                    context.Request?.Method,
                    context.Response?.StatusCode);
            }
        }
    }
}
