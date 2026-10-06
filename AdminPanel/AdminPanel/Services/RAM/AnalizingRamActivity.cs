using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.RAM
{
    public class AnalizingRamActivity
    {
        private static List<double> nums = new List<double>();

        private static bool isListFull = false;

        public static string YellowAnomaly(double total, double free)
        {
            double proc = (free * 100) / total;
            AddNumToList(proc);

            int AnomalyMass = 0;

            if (nums.Count == 10)
            {
                for(int i = 0; i < nums.Count; i++)
                {
                    if (nums[i] <= 10)
                    {
                        AnomalyMass++;
                    }
                }

                if (AnomalyMass >= 9)
                {
                    return "yellow";
                }
                
            }

            return "none";
        }

        private static void AddNumToList(double number)
        {
            if (isListFull)
            {
                nums.RemoveAt(0);
                nums.Add(number);
            }
            else
            {
                nums.Add(number);
                if (nums.Count == 10)
                {
                    isListFull = true;
                }
            }
        }

        public static void ResetHistory()
        {
            nums.Clear();
            isListFull = false;
        }
    }
}
