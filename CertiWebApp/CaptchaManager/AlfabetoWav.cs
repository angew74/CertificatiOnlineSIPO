using System;
using System.Data;
using System.Configuration;

namespace WaveKit
{
    public sealed class AlfabetoWav
    {
        static readonly AlfabetoWav instance = new AlfabetoWav();

        static System.Collections.Hashtable Sounds;
        private string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "z" };

        static AlfabetoWav()
        {
        }

        AlfabetoWav()
        {
            //String commonpath;
            Sounds = new System.Collections.Hashtable(21);
            //commonpath = AppDomain.CurrentDomain.BaseDirectory + "waves\\";
            WaveKit.WaveUtil u = WaveKit.WaveUtil.Instance;
            //for (int i = 0; i < letters.Length; i++) Sounds.Add(letters[i], u.getWav(commonpath + letters[i] + ".wav"));
        }

        public static AlfabetoWav Instance
        {
            get
            {
                return instance;
            }
        }
        public string getLetter(int index) { return letters[index]; }

        public string getRandomLetters(int length)
        {
            String temp = "";
            for (int i = 0; i < length; i++) temp = String.Concat(temp, letters[ImageKit.Util.RandomProvider.Instance.Next(21)]);
            return temp;
        }

        public int getLetterNumber()
        {
            return letters.Length;
        }
        public WaveKit.Wave getLetterSpelling(string letter)
        {
            return (WaveKit.Wave)Sounds[letter];
        }

        public WaveKit.Wave getWordSpelling(string word)
        {
            WaveKit.Wave t = (WaveKit.Wave)Sounds[word.Substring(0, 1)];
            for (int i = 1; i < word.Length; i++)
            {
                WaveKit.Wave t1 = (WaveKit.Wave)Sounds[word.Substring(i, 1)];
                t = WaveKit.WaveUtil.Instance.Merge(t, t1);
            }
            return t;
        }

        public WaveKit.Wave getWordSpellingWithNoise(string word)
        {
            WaveKit.Wave w = getWordSpelling(word);
            WaveKit.WaveUtil.Instance.addNoise(w);
            return w;
        }
    }
}
   
    
    