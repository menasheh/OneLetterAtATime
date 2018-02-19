using System;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;

namespace WordTravel
{
    [Log(AttributeExclude = true)]
    class Program
    {
        private static readonly string START_WORD;
        private static readonly string END_WORD;

        static Program()
        {
            string[,] options = {{"BIG", "LOG"}, {"FLED", "TRUE"}}; //, {"CHASE", "BAKED"}};
            // todo from dictionary pick random, detect impossibility? auto generate longest streams?

            var rand = new Random();
            var option = rand.Next(options.GetLength(0));
            var order = rand.Next(2);
            START_WORD = options[option, order];
            END_WORD = options[option, 1 - order];
        }

        static void Main(string[] args)
        {
            var backend = new ConsoleLoggingBackend();
            backend.Options.Theme = ConsoleThemes.Dark;
            LoggingServices.DefaultBackend = backend;

            Console.WriteLine("Loading... This may take up to a minute...");

            Traveler game = new Traveler(Dictionary.GetWordsOfLength(START_WORD.Length), START_WORD, END_WORD);
            Console.Clear();

            Console.WriteLine("Welcome to Word Traveler!");
            Console.WriteLine("Enter a word with one letter changed or 'I QUIT' to give up.");
            Console.WriteLine();
            Console.WriteLine("Start word: " + START_WORD);
            Console.WriteLine("Goal word:  " + END_WORD);
            Console.WriteLine();
            Console.WriteLine(START_WORD);

            string active = START_WORD;

            while (!game.Over)
            {
                string input = Console.ReadLine().ToUpper();
                if (input.Equals("I QUIT"))
                {
                    Console.WriteLine();
                    foreach (String s in game.Path)
                    {
                        Console.WriteLine(s);
                    }

                    Console.WriteLine();
                    break;
                }

                if (game.Step(input))
                {
                    active = input;
                }
                else
                {
                    Console.WriteLine("Invalid entry.");
                    Console.WriteLine(active);
                }
            }

            if (game.Over)
            {
                Console.WriteLine("Congrats!");
                Console.WriteLine("Win in " + game.UserMoves + " moves.");
                Console.WriteLine();
            }

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}