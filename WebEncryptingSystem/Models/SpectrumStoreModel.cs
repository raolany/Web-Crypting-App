using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class SpectrumStoreModel
    {
        public Dictionary<char, int> Store { get; set; }
        public static List<char> Alphabet { get; set; }
        public List<char> AlphabetTmp { get; set; }
        public List<int> Spectrum { get; set; }
        public int CharsCount { get; set; }
        public int LettersCount { get; set; }

        public SpectrumStoreModel()
        {
            Store = new Dictionary<char, int>();
            Spectrum = new List<int>();
            CharsCount = 0;
            LettersCount = 0;

            foreach (var ch in Alphabet)
            {
                Store.Add(ch, 0);
            }

            AlphabetTmp = new List<char>(Alphabet);
        }

        static SpectrumStoreModel()
        {
            Alphabet = new List<char>();

            for (char i = 'A'; i <= 'Z'; i++)
            {
                Alphabet.Add(i);
            }
            for (char i = '0'; i <= '9'; i++)
            {
                Alphabet.Add(i);
            }
            Alphabet.Add('.');
            Alphabet.Add(' ');
            Alphabet.Add(',');
            Alphabet.Add('-');
            Alphabet.Add('!');
        }
    }
}