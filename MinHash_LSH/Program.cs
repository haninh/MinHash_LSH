using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinHash_LSH
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MinHash - SLH
            //MinHash1 mh = new MinHash1(1000, 100);            
            //string[] s = new string[9];
            //s[0] = "Most hashing techniques are designed to create hash values that differ significantly for similar items, such as documents. For example, two documents that are very similar will often generate hash values that are very different.";
            //s[1] = "Most hashing techniques are designed to create hash values that differ significantly for similar items, such as documents. For example, two documents that are very similar will often generate hash values that are very different.MinHash is different – the technique is designed to ensure that two similar items generate hashes that are themselves similar. In fact, the similarity of the hashes has a direct relationship to the similarity of the documents they were generated from. This relationship approximates to the";
            //s[2] = "MinHashes alone can be used to estimate the similarity of two documents without reference to the content of the documents. They are therefore “document fingerprints” or “document signatures”. The size of document fingerprint is determined by the number of hashes used. A typical number of hashes is around 100, so the total size of the finger print is around 400 bytes regardless of the original size of the document.";
            //s[3] = "MinHashes alone can be used to estimate the similarity of two documents without reference to the content of the documents. They are therefore “document fingerprints” or “document signatures”. The size of document fingerprint is determined by the number of hashes used. A typical number of hashes is around 100, so the total size of the finger print is around 400 bytes regardless of the original size of the document.";
            //s[4] = "Most hashing techniques are designed to create hash values that differ significantly for similar items, such as documents. For example, will often generate hash values that are very different.";
            //s[5] = "in the end rolling in the deep";
            //s[6] = "in the end rolling in the deep";
            //s[7] = "in the end rolling in the deep";
            //s[8] = "Most hashing techniques are designed to create hash values that differ significantly for similar items, such as documents. For example, two documents that are very similar will often generate hash values that are very";
            //List<double> rl = new List<double>();
            //List<double> rl1 = new List<double>();
            //List<int> abc = mh.FindSimilarityDocument(s[5],s);            
            //uint[,] matrix1 = mh.getMinHashMatrix(s);
            //LSH lsh1 = new LSH(matrix1, 10);
            //lsh1.Calc();
            //ArrayList arrList = new ArrayList();

            //for (uint n = 0; n < matrix1.GetUpperBound(0); n++)
            //{
            //    List<uint> nearest1 = lsh1.GetNearest(n);
            //    arrList.Add(nearest1);
            //}
            //string a = null;
            #endregion

            #region Test Rabin-Karp
            string s1 = "sieu nhan sip do day ha ha";
            string s2 = "sieu nhan sip do";
            RabinKarp rk = new RabinKarp();
            bool KQ = rk.RabinKarpSearch(s1, s2);
            if (KQ == true)
                Console.WriteLine("Text contains this pattern!!!");
            else
                Console.WriteLine("Text don't contain this pattern!!!");
            #endregion
        }
    }
}
