using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpencerStuart.RecursiveAdding
{
    public class RecursiveAddingUtils
    {
        private static readonly byte[] EmptyByteArray = new byte[0];
        //Split input arrays on at most that number of parts. Each part - recursive call.
        //Changing of this param would change memory consumpsion
        private static readonly int MaxPartsCount = 10;

        public static byte[] AddRecursive(byte[] firstSummand, byte[] secondSummand)
        {
            int length = firstSummand.Length;

            
            switch (length)
            {
                case 0:
                    return EmptyByteArray;
                case 1:
                    return new [] { (byte)(firstSummand[0] + secondSummand[0]) };
            }


            int partsCount = length < MaxPartsCount ? length : MaxPartsCount;
            

            int remain = length % partsCount;

            byte[] result = new byte[length];


            int partLegth = length / partsCount;

            if (remain > 0)
            {
                partLegth++;
            }
            byte[] firstSummandPart = new byte[partLegth];
            byte[] secondSummandPart = new byte[partLegth];
            int from = 0;
            for (int i = 0; i < partsCount; i++)
            {                
                Array.Copy(firstSummand, from, firstSummandPart, 0, partLegth);
                Array.Copy(secondSummand, from, secondSummandPart, 0, partLegth);
                byte[] partResult = AddRecursive(firstSummandPart, secondSummandPart);

                Array.Copy(partResult, 0, result, from, partLegth);

                from += partLegth;
                remain--;
                if (remain == 0)
                {
                    partLegth--;
                    firstSummandPart = new byte[partLegth];
                    secondSummandPart = new byte[partLegth];
                }
            }
           
            return result;
        }
    }
}
