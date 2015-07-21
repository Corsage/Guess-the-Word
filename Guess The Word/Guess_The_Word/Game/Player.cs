using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guess_The_Word.Game
{
    public class Player
    {
        private int iScore;
        private int iWordsSolved;

        public double m_timer = 30.0f;

        private string s_word;

        public Player()
        {
            this.iScore = 0;
            this.iWordsSolved = 0;

            this.s_word = string.Empty;
        }

        public void AddPoints(int iScore)
        {
            this.iScore += iScore;
        }


        public void UpdateWord(string sWord)
        {
            this.s_word = sWord;
        }

        public void SolvedWord()
        {
            this.iWordsSolved += 1;
        }

        public int PlayerScore()
        {
            return iScore;
        }

        public string WordAttempt()
        {
            return s_word;
        }

        public int PlayerWords()
        {
            return iWordsSolved;
        }
    }
}
