using AdminPanel.Model;
using AdminPanel.Services.Procceses.Files;
using AdminPanel.Services.Procceses.ThreatManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;


namespace AdminPanel.Services.Procceses.ThreatManager
{
    public class BackgroundCheckerHash
    {
        private static readonly HttpClient client = new HttpClient();

        private static readonly string apiKey = LoadApiKey();

        public static async Task HashCheckers(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                string hashToProcess = null;

                if(File.Exists(AdminPanel.Services.Procceses.Files.ThreatManager._notCleanedFile))
                {
                    var lines = File.ReadAllLines(AdminPanel.Services.Procceses.Files.ThreatManager._notCleanedFile);

                    if (lines.Length > 0)
                    {
                        hashToProcess = lines[0];

                        File.WriteAllLines(AdminPanel.Services.Procceses.Files.ThreatManager._notCleanedFile, lines.Skip(1));
                    }
                }
                
                if(!string.IsNullOrEmpty(hashToProcess))
                {
                    bool isMalware = false;

                    isMalware = await CheckHashMalwareBazaarAsync(hashToProcess);

                    if (isMalware)
                    {
                        File.AppendAllText(AdminPanel.Services.Procceses.Files.ThreatManager._malwareFile, hashToProcess + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(AdminPanel.Services.Procceses.Files.ThreatManager._clearFile, hashToProcess + Environment.NewLine);
                    }
                }

                await Task.Delay(2000, cancellationToken);
            }
        }

        public static async Task<bool> CheckHashMalwareBazaarAsync(string hash)
        {
            string url = "https://mb-api.abuse.ch/api/v1/";

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Auth-Key", apiKey);

            var formData = new Dictionary<string, string>
            {
                { "query", "get_info" },
                { "hash", hash }
            };
            request.Content = new FormUrlEncodedContent(formData);

            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                string queryStatus = root.GetProperty("query_status").GetString();

                if (queryStatus == "ok")
                {
                    return true;
                }
                else if (queryStatus == "hash_not_found")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string LoadApiKey()
        {
            try
            {
                string filePath = "appsettings.json";
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    using var doc = JsonDocument.Parse(json);
                    return doc.RootElement.GetProperty("ApiKey").GetString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
    }
}
