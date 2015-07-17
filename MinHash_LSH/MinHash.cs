using System;
using System.Collections.Generic;
using System.Linq;

public class MinHash
{
    static int k = 2;
    static double t = 0.7;
    // Cấu trúc kích thước không gian mẫu và số hàm Hash
    public MinHash(int universeSize, int numHashFunctions)
    {
        this.numHashFunctions = numHashFunctions;
        // Số bit lưu trữ không gian mẫu
        int u = BitsForUniverse(universeSize);
        GenerateHashFunctions(u);
    }

    private int numHashFunctions;

    public int NumHashFunctions
    {
        get { return numHashFunctions; }
    }

    public delegate uint Hash(int toHash);
    private Hash[] hashFunctions;
   
    public Hash[] HashFunctions
    {
        get { return hashFunctions; }
    }

    // tao khong gian mau cac ham Hash ngau nhien        
    private void GenerateHashFunctions(int u)
    {
        hashFunctions = new Hash[numHashFunctions];        
        // will get the same hash functions each time since the same random number seed is used
        Random r = new Random(10);
        for (int i = 0; i < numHashFunctions; i++)
        {
            uint a = 0;
            // parameter a is an odd positive
            while (a % 1 == 1 || a <= 0)
                a = (uint)r.Next();
            uint b = 0;
            int maxb = 1 << u;
            // parameter b must be greater than zero and less than universe size
            while (b <= 0)
                b = (uint)r.Next(maxb);
            hashFunctions[i] = x => QHash(x, a, b, u);
            //hashFunctions1[i] = x => QHash1(x, a, b, u);
        }
    }

    // tra ve so cac bit can thiet de luu truc khong gian mau
    public int BitsForUniverse(int universeSize)
    {
        return (int)Math.Truncate(Math.Log((double)universeSize, 2.0)) + 1;
    }

    // cac ham Hash voi 2 chi so a vaf b va kich thuoc cua khong gian mau trong bits
    private static uint QHash(int x, uint a, uint b, int u)
    {
        return (a * (uint)x + b) >> (32 - u);
    }
    private static int QHash1(int x, int a, int b, int u)
    {
        return (a * (int)x + b) >> (32 - u);
    }
    // tao cac tocken  va lay ID
    public List<int> getTokenID(string s)
    {
        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' };
        string[] arr = s.Split(delimiterChars);
        List<int> tockens = new List<int>();
        //int[] tockens = new int[arr.Length-k+1];
        for (int i = 0; i < arr.Length - k + 1; i++)
        {
            string shingle = "";
            for (int j = 0; j < k; j++)
            {
                shingle += arr[i + j];
            }
            int temp = shingle.GetHashCode();
            if (temp >= 0)
                tockens.Add(temp % Int32.MaxValue);
            else
                tockens.Add(temp % Int32.MaxValue + Int32.MaxValue);
        }
        return tockens;
    }
    //public int[] GetMinHash1(List<int> wordIds)
    //{
    //    int[] minHashes = new int[numHashFunctions];
    //    for (int h = 0; h < numHashFunctions; h++)
    //    {
    //        minHashes[h] = int.MaxValue;
    //    }
    //    foreach (int id in wordIds)
    //    {
    //        for (int h = 0; h < numHashFunctions; h++)
    //        {
    //            int hash = hashFunctions1[h](id);
    //            minHashes[h] = Math.Min(minHashes[h], hash);
    //        }
    //    }
    //    return minHashes;
    //}

    // tra ve 1 mang  min hasher cho 1 tap cho truoc cua cac Id cua tu`

    public uint[] GetMinHash(List<int> wordIds)
    {
        uint[] minHashes = new uint[numHashFunctions];
        for (int h = 0; h < numHashFunctions; h++)
        {
            minHashes[h] = int.MaxValue;
        }
        foreach (int id in wordIds)
        {
            for (int h = 0; h < numHashFunctions; h++)
            {
                uint hash = hashFunctions[h](id);
                minHashes[h] = Math.Min(minHashes[h], hash);
            }
        }
        return minHashes;
    }
    /// dau vao la 1 tap, dau ra la 1 ma tran Minhash
    public uint[,] getMinHashMatrix(string[] s)
    {
        uint[,] result = new uint[s.Length, numHashFunctions];
        for (int i = 0; i < s.Length; i++)
        {
            var tockens = getTokenID(s[i]);
            for (int h = 0; h < numHashFunctions; h++)
            {
                result[i, h] = int.MaxValue;
            }
            foreach (int id in tockens)
            {
                for (int h = 0; h < numHashFunctions; h++)
                {
                    uint hash = hashFunctions[h](id);
                    result[i, h] = Math.Min(result[i, h], hash);
                }
            }

        }
        return result;
    }
    // dau vao la 1 string dau ra la chu ki minhash
    public uint[] getMinHashSignatures(string s)
    {
        uint[] minHashes = new uint[numHashFunctions];
        var tokens = getTokenID(s);
        for (int h = 0; h < numHashFunctions; h++)
        {
            minHashes[h] = int.MaxValue;
        }
        foreach (int id in tokens)
        {
            for (int h = 0; h < numHashFunctions; h++)
            {
                uint hash = hashFunctions[h](id);
                minHashes[h] = Math.Min(minHashes[h], hash);
            }
        }
        return minHashes;
    }
    //
    public List<int> FindSimilarityDocument(string s, string[] u)
    {
        List<int> result = new List<int>();
        var sigS = getMinHashSignatures(s);
        for (int i = 0; i < u.Length; i++)
        {
            var sigU = getMinHashSignatures(u[i]);
            double sml = Similarity1(sigS, sigU);
            if (sml > t)
                result.Add(i);
        }
        return result;
    }

    public static double Calc(uint[] hs1, uint[] hs2)
    {
        return ((double)hs1.Intersect(hs2).Count() / (double)hs1.Union(hs2).Count());
    }
    public static double Calc1(int[] hs1, int[] hs2)
    {
        return ((double)hs1.Intersect(hs2).Count() / (double)hs1.Union(hs2).Count());
    }
    // tinh do giong nhau cua 2 mang Minhash Value. gan bang ty so giong nhau Jaccard
    public double Similarity1(uint[] l1, uint[] l2)
    {
        return Calc(l1, l2);
    }
    public double Similarity2(int[] l1, int[] l2)
    {
        return Calc1(l1, l2);
    }
    //public double Similarity(List<uint> l1, List<uint> l2)
    //{
    //    Jaccard jac = new Jaccard();
    //    return Jaccard.Calc(l1, l2);
    //}
}