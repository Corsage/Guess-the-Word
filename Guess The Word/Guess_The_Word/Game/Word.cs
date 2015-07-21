using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Guess_The_Word.Core;

namespace Guess_The_Word.Game
{
    public class Word
    {
        private static string m_word;
        private static string m_newword;

        public static int m_lines { get; set; }
        private int m_randomline;

        private Random m_rand = new Random();

        Constants m_constant = new Constants();

        public Word()
        {
            GetWord();
            RandomizeWord();
        }

        private void GetWord()
        {
            do
            {
                m_randomline = m_rand.Next(0, m_lines);
                m_word = File.ReadLines(m_constant.s_file + @"\Dictionary.txt").Skip(m_randomline).Take(1).First();
            }
            while (m_word.Length == 1);
        }

        private void RandomizeWord()
        {
            do
            {
                m_newword = new string(m_word.ToCharArray().OrderBy(s => (m_rand.Next(2) % 2) == 0).ToArray());
            }
            while (m_newword == m_word);
        }

        public void SetLine(int iLines)
        {
            m_lines = iLines;
        }

        public string OriginalWord()
        {
            return m_word.ToUpper();
        }

        public string RandomizedWord()
        {
            return m_newword;
        }
    }
}
