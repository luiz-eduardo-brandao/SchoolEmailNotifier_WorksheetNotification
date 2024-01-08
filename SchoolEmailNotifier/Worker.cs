using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolEmailNotifier.Application;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolEmailNotifier
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CoreApplication _coreApplication;

        public Worker(ILogger<Worker> logger, CoreApplication coreApplication)
        {
            _logger = logger;
            _coreApplication = coreApplication;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _coreApplication.Execute();

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            //    _coreApplication.Execute();

            //    await Task.Delay(3000, stoppingToken);
            //}
        }
    }
}
