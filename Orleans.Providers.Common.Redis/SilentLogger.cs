using Serilog;

namespace Orleans.Providers.Common.Redis
{
    public class SilentLogger
    {
        public static readonly ILogger Logger = new LoggerConfiguration().CreateLogger();
    }
}
