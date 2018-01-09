using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bot.ConsoleClient
{
    public class WordGenerator
    {
        private readonly Dictionary<string, LanguageDictionary> _dics;
        private readonly Random _rand;


        public WordGenerator()
        {
            _rand = new Random();
            _dics = new Dictionary<string, LanguageDictionary>();

            Load(Language.FR, @"ressources\dictionaries\FR.txt");
            Load(Language.ES, @"ressources\dictionaries\ES.txt");
            Load(Language.IT, @"ressources\dictionaries\IT.txt");
            Load(Language.SE, @"ressources\dictionaries\SE.txt");
            Load(Language.EN, @"ressources\dictionaries\ENG.txt");

        }

        public void Load(Language lang, string file)
        {
            var dicName = lang.ToString();
            try
            {
                _dics.Add(dicName, new LanguageDictionary());

                _dics[dicName]._words = File.ReadAllLines(file, Encoding.UTF8)
                    .Select(x => x.Split(" ").First().Split("/").First().Split("\t").First())
                    .Where(x => Char.IsLower(x.First()))
                    .Where(x => x.Length > 2)
                    .Where(x => !x.Contains("-"))
                    .ToList();

                _dics[dicName]._firstTwo = new List<string>();
                _dics[dicName]._letters = new Dictionary<string, List<char>>();

                foreach (var word in _dics[dicName]._words)
                {
                    try
                    {
                        _dics[dicName]._firstTwo.Add(word.Substring(0, 2));

                        for (int i = 2; i < word.Length; i++)
                        {
                            var letter = word[i];
                            var sentence = word.Substring(i - 2, 2);
                            if (!_dics[dicName]._letters.ContainsKey(sentence)) _dics[dicName]._letters.Add(sentence, new List<char>());
                            _dics[dicName]._letters[sentence].Add(letter);
                        }

                        var lastSentence = word.Substring(word.Length - 2, 2);
                        if (!_dics[dicName]._letters.ContainsKey(lastSentence)) _dics[dicName]._letters.Add(lastSentence, new List<char>());
                        _dics[dicName]._letters[lastSentence].Add(' ');
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"erreur avec le mot {word} : {e}");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"erreur avec le dictionnaire {dicName} : {e}");
            }

        }

        public string GetWord(Language lang)
        {
            var dicName = lang.ToString();
            while (true)
            {
                var word = _dics[dicName]._firstTwo[_rand.Next(_dics[dicName]._firstTwo.Count)];

                var currentSentence = word;
                while (true)
                {
                    var letters = _dics[dicName]._letters[currentSentence];
                    var nextLetter = letters[_rand.Next(letters.Count)];
                    if (nextLetter != ' ')
                        word += nextLetter;
                    else
                        break;
                    currentSentence = word.Substring(word.Length - 2, 2);
                }

                if (_dics[dicName]._words.Contains(word)) continue; // ne peut pas générer un mot existant dans la langue
                return word;
            }
        }

        public string GetWord(Language lang, int length)
        {
            if (length < 3) throw new ArgumentOutOfRangeException(nameof(length));
            while (true)
            {
                var word = GetWord(lang);
                if (word.Length == length) return word;
            }
        }
    }

    public class LanguageDictionary
    {
        public List<string> _words;
        public List<string> _firstTwo = new List<string>();
        public Dictionary<string, List<char>> _letters;
    }

    public enum Language
    {
        FR,
        ES,
        IT,
        SE,
        EN
    }
}
