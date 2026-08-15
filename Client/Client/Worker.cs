
using System.Runtime.Versioning;
using System.Net.Http.Json;
using Client.Modules.PCInfo;
using System.Net.Http;
using Client.Modules.ServerSend.StaticData;
using Client.Modules.ServerSend.DynamicData;

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

            SendCPUInfoToServer sendCPUInfo = new SendCPUInfoToServer();
            await sendCPUInfo.SendStaticDataToServerAsync(client, stoppingToken);

            SendRAMInfoToServer sendRAMInfo = new SendRAMInfoToServer();
            await sendRAMInfo.SendStaticDataToServerAsync(client, stoppingToken);

            SendCurrentCPUInfoToServer sendCurrCPUInfo = new SendCurrentCPUInfoToServer();

            SendCurrentRAMInfoToServer sendCurrRAMInfo = new SendCurrentRAMInfoToServer();
            
            SendProcessesInfoToServer sendProcessInfo = new SendProcessesInfoToServer();

            SendAdaptersInfoToServer sendApaptersInfo = new SendAdaptersInfoToServer();

            SendConnectionInfoToServer sendConnectionsInfo = new SendConnectionInfoToServer();

            while (!stoppingToken.IsCancellationRequested)
            {
                await sendCurrCPUInfo.SendDynamicDataToServerAsync(client, stoppingToken);

                await sendCurrRAMInfo.SendDynamicDataToServerAsync(client, stoppingToken);

                await sendProcessInfo.SendDynamicDataToServerAsync(client, stoppingToken);

                await sendApaptersInfo.SendDynamicDataToServerAsync(client, stoppingToken);

                await sendConnectionsInfo.SendDynamicDataToServerAsync(client, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
