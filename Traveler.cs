using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WordTravel
{
    public class Traveler
    {
        private string[] _dict;
        private string _src;
        private string _dest;

        private IEnumerable _path;

        private ArrayList _userPath;
        private bool _gameOver;

        public Traveler(string[] words, string source, string destination)
        {

            _dict = words;
            _src = source;
            _dest = destination;


            if (_src.Length != _dest.Length) throw new Exception("Source and Destination strings must be of equal length!");

                _path = Travel(); // todo This can fail with bad _src and _dest - fix (?)

            _userPath = new ArrayList {_src};
            _gameOver = false;
        }

        public bool Step(string word)
        {
            if (word.Length != _src.Length) return false;
            if (DifferingCharacters(word, (string) _userPath[_userPath.Count - 1]) != 1) return false;
            if (!_dict.Contains(word)) return false;

            _userPath.Add(word);
            if (word.Equals(_dest)) _gameOver = true;

            return true;
        }

        public bool Over => _gameOver;
        public IEnumerable Path => _path;
        public int UserMoves => _userPath.Count;

        public IEnumerable<string> LegalMoves(string start)
        {
            return LegalMoves(start, ref _dict);
        }

        public static IEnumerable<string> LegalMoves(string start, ref string[] dict)
        {
            return (from word in dict
                where start.Where((t, i) => start.Substring(i, 1) != word.Substring(i, 1)).Count().Equals(1)
                select word);
        }

        private IEnumerable Travel()
        {
            Node rootNode = new Node(_src);
            ArrayList generation;
            ArrayList nextGeneration = rootNode.SpawnChildren();

            long time;
            for (int i = 0;; i++)
            {
                time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                generation = nextGeneration;
                nextGeneration = new ArrayList();

                foreach (Node parent in generation)
                {

                    if (parent.Word.Equals(_dest)) return parent.TraceAncestry();

                    var children = parent.SpawnChildren();

                    //nextGeneration.AddRange(children);
                     foreach (Node child in children) //todo multithreading might make this more efficient
                     {
                         if(!nextGeneration.Contains(child.Word))
                             nextGeneration.Add(child);
                     }
                }

                time = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - time;
                Console.WriteLine("Gen" + i + ": " + time + "ms");
            }
        }

        private static int DifferingCharacters(string word, string otherword)
        {
            return word.Where((t, i) => word.Substring(i, 1) != otherword.Substring(i, 1)).Count();
        }
    }
}
