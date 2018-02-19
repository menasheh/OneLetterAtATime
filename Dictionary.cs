using System.IO;
using System.Linq;

namespace WordTravel
{
    public static class Dictionary
    {
        private static readonly string[] Words;

        static Dictionary()
        {   //Words = File.ReadAllLines("words.txt")
            //            .Select(word => word.ToUpper())
            //            .OrderBy(word => word.Length);
            Words = (from word in File.ReadAllLines("words.txt") orderby word.Length, word select word.ToUpper()).ToArray();
        }

        public static string[] GetWordsOfLength(int length)
        {
            //return Words.Where(word => word.Length == length);
            return (from word in Words where word.Length == length select word).ToArray();
        }
    }
}
