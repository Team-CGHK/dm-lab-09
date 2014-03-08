using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_E
{
    class FastPow
    {
        public static long Pow(int x, int pow)
        {
            if (pow == 0)
                return 1;
            if (pow % 2 == 1)
                return Pow(x, pow - 1) * x;
            else
            {
                long b = Pow(x, pow / 2);
                return b * b;
            }
        }
    }
}
