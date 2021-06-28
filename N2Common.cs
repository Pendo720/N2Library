using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N2Library
{
    public static class N2Common
    {
        public static List<double> IntBitsAsDoubles(int target, int count)
        {
            string s = Convert.ToString(target, 2); //Convert to binary in a string
            return s.PadLeft(count, '0')
                    .Select(c => (int.Parse(c.ToString()) == 1?1.0:0.0))
                    .ToList(); 
        }

        public static String StringifyList(List<double> doubles)
        {
            string stoReturn = string.Empty;
            doubles.ForEach(d =>stoReturn += $"{d},");
            stoReturn = stoReturn.Substring(0, stoReturn.Length - 1);
            return stoReturn;
        }

        public static double RandomDouble => new Random().NextDouble();
    }
}
