using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEncryptingSystem.Models
{
    public class SpectrumStoreModel
    {
        public Dictionary<char, int> Store { get; set; }
        public List<char> Alphabet { get; set; }
        public List<int> Spectrum { get; set; }
        public int CharsCount { get; set; }
        public int LettersCount { get; set; }

        public SpectrumStoreModel()
        {
            Store = new Dictionary<char, int>();
            Alphabet = new List<char>();
            Spectrum = new List<int>();
            CharsCount = 0;
            LettersCount = 0;

            for (char i = 'A'; i <= 'Z'; i++)
            {
                Alphabet.Add(i);
                Store.Add(i, 0);
            }
            for (char i = '0'; i <= '9'; i++)
            {
                Alphabet.Add(i);
                Store.Add(i, 0);
            }
        }
    }
}