using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Procceses.Files
{
    public class ThreatManager
    {
        public static string _clearFile = "clear.txt";
        public static readonly string _malwareFile = "malware.txt";
        public static readonly string _notCleanedFile = "not_cleaned.txt";

        public ThreatManager()
        {
            EnsureFilesExist();
        }

        private void EnsureFilesExist()
        {
            if (!File.Exists(_clearFile)) File.Create(_clearFile).Close();
            if (!File.Exists(_malwareFile)) File.Create(_malwareFile).Close();
            if (!File.Exists(_notCleanedFile)) File.Create(_notCleanedFile).Close();
        }

        public static string CheckOrRegisterHash(string hash)
        {
            if (File.ReadLines(_clearFile).Contains(hash))
                return "black";

            if (File.ReadLines(_malwareFile).Contains(hash))
                return "red";

            if (File.ReadLines(_notCleanedFile).Contains(hash))
                return "black";

            File.AppendAllText(_notCleanedFile, hash + Environment.NewLine);
                return "black";
        }
    }
}
