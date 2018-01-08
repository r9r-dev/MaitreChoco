using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bot.ConsoleClient
{
    public class WordGenerator
    {
        private List<string> _words;
        private List<string> _firstTwo = new List<string>();
        private Dictionary<string, List<char>> _letters;
        private readonly Random _rand;


        public WordGenerator()
        {
            _rand = new Random();
        }


        public void Load(string file)
        {
            _words = File.ReadAllLines(file, Encoding.UTF8)
                .Select(x => x.Split(" ").First().Split("/").First().Split("\t").First())
                .Where(x => Char.IsLower(x.First()))
                .Where(x => x.Length > 2)
                .Where(x => !x.Contains("-"))
                .ToList();

            _firstTwo = new List<string>();
            _letters = new Dictionary<string, List<char>>();

            foreach (var word in _words)
            {
                _firstTwo.Add(word.Substring(0, 2));

                for(int i = 2;i<word.Length;i++)
                {
                    var letter = word[i];
                    var sentence = word.Substring(i - 2, 2);
                    if (!_letters.ContainsKey(sentence)) _letters.Add(sentence, new List<char>());
                    _letters[sentence].Add(letter);
                }

                var lastSentence = word.Substring(word.Length - 2, 2);
                if (!_letters.ContainsKey(lastSentence)) _letters.Add(lastSentence, new List<char>());
                _letters[lastSentence].Add(' ');
            }

        }

        public string GetWord()
        {
            var word = _firstTwo[_rand.Next(_firstTwo.Count)];

            var currentSentence = word;
            while (true)
            {
                var letters = _letters[currentSentence];
                var nextLetter = letters[_rand.Next(letters.Count)];
                if (nextLetter != ' ') word += nextLetter;
                else break;
                currentSentence = word.Substring(word.Length - 2, 2);
            }

            return word;
        }

        public string GetWord(int length)
        {
            if (length < 3) throw new ArgumentOutOfRangeException(nameof(length));
            while (true)
            {
                var word = GetWord();
                if (word.Length == length) return word;
            }
        }
    }
}
