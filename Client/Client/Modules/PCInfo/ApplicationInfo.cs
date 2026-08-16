using Client.Modules.PCInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Client.Modules.PCInfo
{
    public class ApplicationInfo
    {
        public ApplicationInfoModel GetApplicationInfo()
        {
            var model = new ApplicationInfoModel()
            {
                Application = new List<ApplicationProperty>()
            };

            foreach (var app in Process.GetProcesses())
            {
                if (app.MainWindowHandle != IntPtr.Zero && !string.IsNullOrWhiteSpace(app.MainWindowTitle))
                {
                    string appName = app.ProcessName ?? "Unknown";
                    string windowTitle = app.MainWindowTitle ?? "Unknown";
                    string id = app.Id.ToString() ?? "Unknown";
                    string ram = Math.Round(app.WorkingSet64 / 1048576.0, 2).ToString() ?? "Unknown";

                    var appPropery = new ApplicationProperty()
                    {
                        AppName = appName,
                        WindowTitle = windowTitle,
                        Id = id,
                        RAM = ram
                    };

                    model.Application.Add(appPropery);
                }
            }

            return model;
        }
    }
}
