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
    public class VigenereModel
    {
        public string Key { get; set; }
        private static int m = 0;
        private static List<char> alphabet;

        static VigenereModel()
        {
            m = SpectrumStoreModel.Alphabet.Count;
            alphabet = new List<char>(SpectrumStoreModel.Alphabet);
        }

        public VigenereModel(string key)
        {
            Key = key;
        }

        public bool VerifyKey()
        {
            foreach (var t in Key)
            {
                if (!alphabet.Contains(t)) return false;
            }
            return true;
        }

        public FileModel Crypting(string path, bool act)
        {
            string txtFile = File.ReadAllText(path).ToUpper().Replace("\n", string.Empty).Replace("\t", string.Empty);

            var txtSB = new StringBuilder(txtFile.Length);
            for (int i = 0; i < txtFile.Length; i++)
            {
                if (alphabet.Contains(txtFile[i]))
                    txtSB.Append(txtFile[i]);
            }
            var txt = txtSB.ToString();
            var output = new StringBuilder(txt.Length);
            var keystr = new StringBuilder(txt.Length);
            for (int i = 0; i < txt.Length/Key.Length; i++)
            {
                keystr.Append(Key);
            }
            keystr.Append(Key.Substring(0, txt.Length%Key.Length));
            
            for (int i = 0; i < txt.Length; i++)
            {
                output.Append(act ? EncryptFormula(keystr[i], txt[i]) : DecryptFormula(keystr[i], txt[i]));
            }
            
            //true = enc
            var prefix = (act) ? "enc" : "dec";
            var encpath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path).Split('_')[0] + "_vigenere_" + prefix + ".txt";
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

        private char EncryptFormula(char key, char msg)
        {
            return alphabet[(alphabet.IndexOf(msg) + alphabet.IndexOf(key)) % m];
        }

        private char DecryptFormula(char key, char msg)
        {
            return alphabet[(alphabet.IndexOf(msg) - alphabet.IndexOf(key) + m) % m];
        }
    }
}