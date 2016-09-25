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
    public class BitModel
    {
        public int FirstBit { get; set; }
        public int SecondBit { get; set; }

        public BitModel(int firstBit, int secondBit)
        {
            FirstBit = firstBit;
            SecondBit = secondBit;
        }

        public FileModel Crypting(string path, bool act)
        {
            string txt = File.ReadAllText(path).ToUpper().Replace("  ", string.Empty).Trim().Replace("\n", string.Empty).Replace("\t", string.Empty);

            var bytes = Encoding.Unicode.GetBytes(txt);
            var bitarr = new BitArray(bytes);

            for (int i = 0; i < bitarr.Count; i += 16)
            {
                bool tmp = bitarr[i + FirstBit];
                bitarr[i + FirstBit] = bitarr[i + SecondBit];
                bitarr[i + SecondBit] = tmp;
            }

            var output = Encoding.Unicode.GetString(BitArrayToByteArray(bitarr));
            Debug.WriteLine("OUTPUT is " + output);

            //true = enc
            var prefix = (act) ? "enc" : "dec";
            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_bit_"+prefix+".txt";
            FileInfo outputFile = new FileInfo(encpath);

            using (StreamWriter sw = outputFile.CreateText())
            {
                sw.Write(output);
                sw.Close();
            }

            return new FileModel()
            {
                File = output,
                Name = Path.GetFileName(encpath)
            };
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}