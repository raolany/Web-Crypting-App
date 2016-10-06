using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class HillModel
    {
        public int[,] Key { get; set; }
        private static int m = 0;
        private static List<char> alphabet;

        public HillModel(int[,] key)
        {
            Key = key;
        }

        static HillModel()
        {
            m = SpectrumStoreModel.Alphabet.Count;
            alphabet = new List<char>(SpectrumStoreModel.Alphabet);
        }

        public FileModel CryptFile(string path, bool act)
        {
            string txt = File.ReadAllText(path).ToUpper().Replace("  ", string.Empty).Trim().Replace("\n", string.Empty).Replace("\t", string.Empty);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < txt.Length; i++)
            {
                if (alphabet.Contains(txt[i]))
                {
                    int[] codes;
                    //true = enc1
                    if(act)
                        codes = EncryptedFormula(i + 1 < txt.Length ? new [] { alphabet.IndexOf(txt[i]), alphabet.IndexOf(txt[i + 1])} : new [] { alphabet.IndexOf(txt[i]), 0 });
                    else
                        codes = DecryptedFormula(i + 1 < txt.Length ? new[] { alphabet.IndexOf(txt[i]), alphabet.IndexOf(txt[i + 1])} : new[] { alphabet.IndexOf(txt[i]), 0 });
                    output.Append(alphabet[(int)codes[0]]);
                    output.Append(alphabet[(int)codes[1]]);
                    i++;
                }
            }

            //true = enc
            var prefix = (act) ? "enc" : "dec";
            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_hill_"+prefix+".txt";
            FileInfo outputFile = new FileInfo(encpath);

            using (StreamWriter sw = outputFile.CreateText())
            {
                sw.Write(output);
                sw.Close();
            }

            return new FileModel()
            {
                File = output.ToString(),
                Name = Path.GetFileName(encpath)
            };
        }

        private int[] EncryptedFormula(int[] vecx)
        {
            return MultVecMatr(vecx, Key);
        }

        private int[] DecryptedFormula(int[] vecx)
        {
            var det = Det(Key);
            int[,] minors = { { Key[1, 1], -Key[1, 0] }, { -Key[0, 1], Key[0, 0] } };

            int[,] key_rev = TransponMtx(minors);
            int[,] fkey_rev = new int[2, 2];

            for (int i = 0; i < vecx.Length; i++)
            {
                for (int j = 0; j < vecx.Length; j++)
                {
                    if (det < 0)
                    {
                        key_rev[i, j] = -key_rev[i, j];
                    }

                    if (key_rev[i, j] >= 0 && key_rev[i, j] % det == 0)
                    {
                        fkey_rev[i, j] = key_rev[i, j] / det;
                    }
                    else
                    {
                        if (key_rev[i, j] < 0)
                            key_rev[i, j] += m;
                        while (key_rev[i, j] % det != 0)
                        {
                            key_rev[i, j] += m;
                        }
                        fkey_rev[i, j] = key_rev[i, j] / Math.Abs(det);
                    }
                }
            }

            return MultVecMatr(vecx, fkey_rev);
        }

        private int[,] TransponMtx(int[,] a)
        {
            var n = a.Length / 2;
            int[,] tMatr = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    tMatr[j, i] = a[i, j];
            return tMatr;
        }

        private int Det(int[,] a)
        {
            return (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0])%m;
        }

        private int[] MultVecMatr(int[] vec, int[,] matr)
        {
            int[] nvec = new int[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                for (int j = 0; j < vec.Length; j++)
                {
                    nvec[i] += vec[j] * matr[j, i];
                }
                nvec[i] = ((nvec[i] % m) + m)%m;
            }
            return nvec;
        }
    }
}