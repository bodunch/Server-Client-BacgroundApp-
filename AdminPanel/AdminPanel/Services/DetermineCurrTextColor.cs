using AdminPanel.Model;
using AdminPanel.Services.CPU;
using AdminPanel.Services.RAM;
using AdminPanel.Services.Procceses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using AdminPanel.Services.Procceses.Files;
using AdminPanel.Services.Ports;

namespace AdminPanel.Services
{
    public static class DetermineCurrTextColor
    {
        public static string CpuDataColor(DnmCpuInfo data)
        {
            string cleanLoad = data.LoadCPU?.Replace("%", "").Trim() ?? "0";
            double load;
            if (!double.TryParse(cleanLoad, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out load))
            {
                load = 0; 
            }

            string cpuAnomalyRed = AnalizingCpuActivity.RedAnomaly(load);
            string cpuAnomalyYellow = AnalizingCpuActivity.YellowAnomaly(load);

            return data switch
            {
                { ErrorCode: not "No errors" } => "red",

                { Status: "Error" } => "red",
                { Status: "Pred Fail" } => "red",
                { Status: "Stressed" } => "red",
                { Status: "NonRecover" } => "red",
                { Status: "No Contact" } => "red",
                { Status: "Lost Comm" } => "red",
                _ when cpuAnomalyRed == "red" => "red",

                { Status: "Degraded" } => "yellow",
                { Status: "Unknown" } => "yellow",
                { Status: "Service" } => "yellow",
                _ when load > 75.0 => "yellow",
                _ when cpuAnomalyYellow == "yellow" => "yellow",

                _ => "black"
            };
        }

        public static string RamDataColor(DnmRamInfo data)
        {
            double free = double.Parse(data.FreeMem);
            double total = double.Parse(data.TotalMem);

            string cpuAnomalyYellow = AnalizingRamActivity.YellowAnomaly(total, free);

            return data switch
            {
                _ when cpuAnomalyYellow == "yellow" => "yellow",
                _ => "black"
            };
        }

        public static string ProcDataColor(ProcessProperty data)
        {
            string hashColor = ThreatManager.CheckOrRegisterHash(data.SHA256);

            return data switch
            {
                _ when hashColor == "red" => "red",

                _ => "black"
            };
        }

        public static string PortsDataColor(PortProperty data)
        {
            string color = AnalizingPortActivity.EvaluatePorts(data.Adress, data.Port);

            return data switch
            {
                _ when color == "red" => "red",

                _ when color == "yellow" => "yellow",

                _ => "black"
            };
        }
    }
}
