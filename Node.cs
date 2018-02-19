using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WordTravel
{
    public class Node
    {
        private static string[] _dict;
        private readonly Node _parentNode;

        public readonly string Word;
        public IEnumerable<string> Paths { get; set; }

        public Stack<string> TraceAncestry()
        {
            Stack<string> ancestry = new Stack<string>();
            ancestry.Push(Word);

            Node n = this;
            while (n._parentNode != null)
            {
                n = n._parentNode;
                ancestry.Push(n.Word);
            }

            return ancestry;
        }

        public Node(string word)
        {
            Word = word;
            _dict = Dictionary.GetWordsOfLength(Word.Length);
            Paths = Traveler.LegalMoves(word, ref _dict);
        }

        public Node(string word, Node parent)
        {
            Word = word;
            _parentNode = parent;
            Paths = Traveler.LegalMoves(word, ref _dict).Except(TraceAncestry());
        }

        public ArrayList SpawnChildren()
        {
            var list = new ArrayList();
            foreach (var s in Paths)
            {
                list.Add(new Node(s, this));
            }

            return list;
        }

        public override string ToString()
        {
            return Word;
        }

        public override bool Equals(object obj)
        {
            return Word.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }
        // TODO - NODE could in theory have its own methods for searching within the tree.
        // Each node could have paths, bool expanded (better than .length, some have no expansions),
        // and expanded arraylist of nodes. Then individual nodes could intelligently search through
        // the tree, expanding as necessary, with a different algorithm which might be faster than
        // current method. And/or multithreading
    }
}