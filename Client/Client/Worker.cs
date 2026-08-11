
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
            using var client = _httpClientFactory.CreateClient();

            SendSystemInfoToServer sendSysInfo = new SendSystemInfoToServer();
            await sendSysInfo.SendStaticDataToServerAsync(client, stoppingToken);

            SendComputerInfoToServer sendCompInfo = new SendComputerInfoToServer();
            await sendCompInfo.SendStaticDataToServerAsync(client, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
