using ffooe.db.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ffooe.service.worker
{
    public class PushGroupCreateWorker : BackgroundService
    { 
        protected readonly ILogger<PushGroupCreateWorker> _logger;
        protected readonly FFOOEContext _context;
        protected readonly IConfiguration _configuration;
        public PushGroupCreateWorker(ILogger<PushGroupCreateWorker> logger, FFOOEContext context, IConfiguration configuration) 
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("PushGroupCreateWorker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(6000, stoppingToken);
            }
        });
    }
}
