using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.CPU
{
    public class AnalizingCpuActivity
    {
        private static List<double> nums = new List<double>();
        private static bool isListFull = false;
        
        public static string RedAnomaly(double num)
        {
            AddNumToList(num);

            int AnomalyMass = 0;

            AnomalyMass = 0;

            if(nums.Count == 10)
            {
                for(int i = 0; i < nums.Count - 1; i++)
                {
                    if (Math.Abs(nums[i] - nums[i + 1]) >= 60)
                    {
                        AnomalyMass++;
                    }
                }

                if(AnomalyMass >= 8)
                {
                    return "red";
                }
            }

            return "none";
        }

        public static string YellowAnomaly(double num)
        {
            int AnomalyMass = 0;

            for(int i = 0; i < nums.Count; i++)
            {
                if (nums[i] >= 85)
                {
                    AnomalyMass++;
                }
            }

            if( AnomalyMass >= 9)
            {
                return "yellow";
            }
            return "none";
        }

        private static void AddNumToList (double number)
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
