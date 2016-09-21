using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class AffineModel
    {
        private int a = 0;
        private int b = 0;
        private static int m = 0;
        private static List<char> alphabet;

        public AffineModel(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        static AffineModel()
        {
            m = SpectrumStoreModel.Alphabet.Count;
            alphabet = new List<char>(SpectrumStoreModel.Alphabet);
        }

        public FileModel EncryptFile(string path)
        {
            string txt = File.ReadAllText(path).ToUpper().Replace("  ", string.Empty).Trim().Replace("\n", string.Empty).Replace("\t", string.Empty);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < txt.Length; i++)
            {
                if (alphabet.Contains(txt[i]))
                {
                    //var code = EncryptedFormula(Encoding.ASCII.GetBytes(txt[i].ToString())[0]);
                    //output.Append(Encoding.ASCII.GetString(new byte[] {code}));
                    var code = EncryptedFormula(alphabet.IndexOf(txt[i]));
                    output.Append(alphabet[code]);
                }
            }

            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "_affine_enc.txt";
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

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < txt.Length; i++)
            {
                //var code = DecryptedFormula(Encoding.ASCII.GetBytes(txt[i].ToString())[0]);
                //output.Append(Encoding.ASCII.GetString(new byte[] { code }));
                var code = DecryptedFormula(alphabet.IndexOf(txt[i]));
                output.Append(alphabet[code]);
            }

            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_affine_dec.txt";
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

        private byte EncryptedFormula(int x)
        {
            return (byte) ((a*x + b) % m);
        }

        private byte DecryptedFormula(int x)
        {
            int a_1 = inverse(a, m);
            return (byte) ((a_1*(x - b))%m);
        }

        private void extended_euclid(int a, int b, out int x, out int y, out int d)
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
                q = a/b;
                r = a - q * b;

                x = x2 - q*x1;
                y = y2 - q * y1;

                a = b;
                b = r;

                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 =y;
            }

            d = a;
            x = x2;
            y = y2;
        }


        private int inverse(int a, int n)
        {
            int d, x, y;

            extended_euclid(a, n, out x, out y, out d);

            if (d == 1) return x;
            return 0;
        }
    }
}