﻿namespace ReservatieServiceGebruikerRESTService
{
    public static class LoggerExtension
    {
        public static IApplicationBuilder UseLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }
}
