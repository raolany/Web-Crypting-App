using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class RSAModel
    {
        private int e = 0;
        private int d = 0;
        private int n = 0;
        private static int m = 0;
        private static List<char> alphabet;

        public RSAModel(int e = 0, int d = 0, int n = 0)
        {
            this.e = e;
            this.d = d;
            this.n = n;
        }

        public RSAModel(int [] key)
        {
            this.e = key[0];
            this.d = key[1];
            this.n = key[2];
        }

        static RSAModel()
        {
            m = SpectrumStoreModel.Alphabet.Count;
            alphabet = new List<char>(SpectrumStoreModel.Alphabet);
        }

        public static int [] SessionKeys(int p, int q)
        {
            var n = p * q;
            int fin = (p - 1) * (q - 1);

            int[] numsFerma = new[] { 3, 5, 17, 257, 65537 };

            int e = 0;
            for (int i = numsFerma.Length - 1; i > 0; i--)
            {
                e = numsFerma[i];
                if (e < fin) break;
            }

            var d = inverse(e, fin);

            return new int[] {e, d, n};
        }

        public FileModel EncryptFile(string path)
        {
            string txt = File.ReadAllText(path).ToUpper().Replace("\n", string.Empty).Replace("\t", string.Empty);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < txt.Length; i++)
            {
                if (alphabet.Contains(txt[i]))
                {
                    var code = EncryptedFormula(alphabet.IndexOf(txt[i]));
                    output.Append(code+" ");
                }
            }

            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_rsa_enc.txt";
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

        public FileModel DecryptFile(string path)
        {
            string txt = File.ReadAllText(path);
            var nums = txt.Split(' ');

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < nums.Length; i++)
            {
                var code = DecryptedFormula(int.Parse(nums[i]));
                output.Append(alphabet[code]);
            }

            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_rsa_enc.txt";
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

        private int EncryptedFormula(int x)
        {
            return (int)BigInteger.ModPow(x, e, n);
        }

        private int DecryptedFormula(int x)
        {
            return  (int)BigInteger.ModPow(x, d, n);
        }

        private static void extended_euclid(int a, int b, out int x, out int y, out int d)
        {
            int q, r, x1, x2, y1, y2;

            if (b == 0)
            {
                d = a;
                x = 1;
                y = 0;
                return;
            }

            x2 = 1;
            x1 = 0;
            y2 = 0;
            y1 = 1;

            while (b > 0)
            {
                q = a / b;
                r = a - q * b;

                x = x2 - q * x1;
                y = y2 - q * y1;

                a = b;
                b = r;

                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }

            d = a;
            x = x2;
            y = x1;
        }

        private static int inverse(int a, int n)
        {
            int d, x, y;

            extended_euclid(a, n, out x, out y, out d);

            if (d == 1)
            {
                if (x < 0)
                {
                    return x + y;
                }
                return x;
            }
            return 0;
        }
    }
}