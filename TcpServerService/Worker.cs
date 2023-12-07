namespace TcpServerService
{
    public class Worker(ILogger<Worker> logger, TcpServer tcpServer) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly TcpServer _tcpServer = tcpServer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
