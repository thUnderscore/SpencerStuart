using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpencerStuart.Stairs
{
    public class Runner
    {
        //If you gonna change next constants, you must change GetStridesCount
        //for now it expects StepsPerStride > 1 and  StepsPerStride <= StepsPerFlight
        public static int MinStepsPerStride = 2;
        public static int MaxStepsPerStride = 5;
        public static int MinStepsPerFlight = 5;
        public static int MaxStepsPerFlight = 30;
        public static int MinFlights = 1;
        public static int MaxFlights = 50;

        private static byte _turnStridesCount = 2;

        private static bool CheckRange(int min, int max, int val)
        {
            return val >= min && val <= max;
        }
        public static bool CheckStepsPerStride(int val)
        {
            return CheckRange(MinStepsPerStride, MaxStepsPerStride, val);
        }

        public static bool CheckStepsPerFlight(int val)
        {
            return CheckRange(MinStepsPerFlight, MaxStepsPerFlight, val);
        }

        public static bool CheckFlights(int val)
        {
            return CheckRange(MinFlights, MaxFlights, val);
        }

        public static int GetStridesCount(int[] flights, int stepsPerStride)
        {            

            if ((flights == null) ||
                !CheckFlights(flights.Length) ||
                !CheckStepsPerStride(stepsPerStride))
            {
                return -1;                
                
            }

            int result = 0;

            foreach (int stepsCount in flights)
            {
                if (!CheckStepsPerFlight(stepsCount))
                {
                    return -1;
                }
                result += ((stepsCount - 1) / stepsPerStride) + 1;
            }
            result += (flights.Length - 1) * _turnStridesCount;

            return result;
        }
    }
}
