using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinHash_LSH
{
    class RabinKarp
    {
        // Search and Multiple pattern search
        public RabinKarp()
        {
        }

        public bool RabinKarpSearch(string text, string pattern)
        {
            bool result = false;
            ulong siga = 0;
            ulong sigb = 0;
            ulong Q = 100078;// lấy tùy ý
            ulong D = 256;
            // tao cac gia tri bam ban dau
            for (int i = 0; i < pattern.Length; i++)
            {
                siga = (siga * D + (ulong)text[i]) % Q;
                sigb = (sigb * D + (ulong)pattern[i]) % Q;
            }

            if (siga == sigb)
            {               
                result = true;               
            }

            ulong pow = 1;

            for (int k = 1; k <= pattern.Length - 1; k++)
                pow = (pow * D) % Q;

            for (int j = 1; j <= text.Length - pattern.Length; j++)
            {
                siga = (siga + Q - pow * (ulong)text[j - 1] % Q) % Q;
                siga = (siga * D + (ulong)text[j + pattern.Length - 1]) % Q;
                if (siga == sigb)
                {
                    if (text.Substring(j, pattern.Length) == pattern)
                    {                        
                        result = true;
                        break;
                    }
                }                
            }
            return result;
        }

    }
}
