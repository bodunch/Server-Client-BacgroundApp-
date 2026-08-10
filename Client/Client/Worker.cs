
using System.Runtime.Versioning;
using System.Net.Http.Json;
using Client.Modules.PCInfo;
using System.Net.Http;
using Client.Modules.ServerSend.StaticData;

namespace Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [SupportedOSPlatform("windows")]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SendSystemInfoToServer sendSysInfo = new SendSystemInfoToServer();
            await sendSysInfo.SendStaticDataToServerAsync(_httpClientFactory, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
