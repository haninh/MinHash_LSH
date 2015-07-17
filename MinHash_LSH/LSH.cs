using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinHash_LSH
{
    public class LSH
    {
        Dictionary<int, HashSet<int>> m_lshBuckets = new Dictionary<int, HashSet<int>>();

        uint[,] minHashes;
        int numBands;
        int numHashFunctions;
        int rowsPerBand;
        int numSets;
        List<SortedList<uint, List<uint>>> lshBuckets = new List<SortedList<uint, List<uint>>>();

        public LSH(uint[,] minHashes, int rowsPerBand)
        {
            this.minHashes = minHashes;
            numHashFunctions = minHashes.GetUpperBound(1) + 1;
            numSets = minHashes.GetUpperBound(0) + 1;
            this.rowsPerBand = rowsPerBand;
            this.numBands = numHashFunctions / rowsPerBand;
        }

        public void Calc()
        {
            int thisHash = 0;

            for (int b = 0; b < numBands; b++)
            {
                var thisSL = new SortedList<uint, List<uint>>();
                for (uint s = 0; s < numSets; s++)
                {
                    uint hashValue = 0;
                    for (int th = thisHash; th < thisHash + rowsPerBand; th++)
                    {
                        hashValue = unchecked(hashValue * 1174247 + minHashes[s, th]);
                    }
                    if (!thisSL.ContainsKey(hashValue))
                    {
                        thisSL.Add(hashValue, new List<uint>());
                    }
                    thisSL[hashValue].Add(s);
                }
                thisHash += rowsPerBand;
                var copy = new SortedList<uint, List<uint>>();
                foreach (uint ic in thisSL.Keys)
                {
                    if (thisSL[ic].Count() > 1)
                    {
                        copy.Add(ic, thisSL[ic]);
                    }
                }
                lshBuckets.Add(copy);
            }
        }

        public List<uint> GetNearest(uint n)
        {
            List<uint> nearest = new List<uint>();
            foreach (SortedList<uint, List<uint>> b in lshBuckets)
            {
                foreach (List<uint> li in b.Values)
                {
                    if (li.Contains(n))
                    {
                        nearest.AddRange(li);
                        break;
                    }
                }
            }
            nearest = nearest.Distinct().ToList();
            nearest.Remove(n);  
            return nearest;
        }
    }
}
