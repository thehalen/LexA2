using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LexA2
{
    partial class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Try your luck at Hangman!");
                Game game = new Game();
                game.HangGame("Words.txt");
            }
        }
    }

    class Game
    {
        static StringBuilder guesses,incorrect;
        static int tries,correct;
        static string secret;
        /// <summary>
        /// Main game function
        /// </summary>
        /// <param name="file">Path to the file to load words from</param>
        public void HangGame(string file)
        {
            //List<char> guesses = new List<char>();
            tries = 10;
            guesses = new StringBuilder();
            incorrect = new StringBuilder();
            
            string[] HangWords = ReadWords(file);
            secret = HangWords[GetRandomInt(0, HangWords.Length)].ToLower();
            string guess;
            while (tries > 0)
            {
                
                DrawGallows(tries);    
                if (correct == secret.Length)
                {
                    Console.WriteLine("\nCongratulations! You win!");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("\nGuesses left: " + tries);
                guess = Console.ReadLine().ToLower();
                if (ValidGuess(guess))
                {
                    if (guess.Length == 1)
                    {
                        guesses.Append(guess);
                        if (!secret.Contains(guess))
                        {
                            tries--;
                            incorrect.Append(guess);
                        }
                    }
                    else if (guess == secret)
                    {
                        Console.WriteLine("\nWell done, you guessed it!");
                        Console.ReadLine();
                        return;
                    }
                    else
                    { //basically incorrect word guesses
                        tries--;
                    }
                }
                else
                {
                    Console.WriteLine("That's not a valid guess!");
                } 
                
                
            }
            DrawGallows(tries);
            Console.WriteLine("Sorry! You lose, we were looking for " + secret);

            Console.ReadLine();
        }

        /// <summary>
        /// Reads the words to load into the program
        /// </summary>
        /// <param name="file">path to csv (,) delimited text file</param>
        /// <returns>String array of words read from the specified file</returns>
        public static string[] ReadWords(string file)
        {
            try
            {
                using TextFieldParser parser = new(file);
                parser.Delimiters = new string[] { "," };
                while (true)
                {
                    string[] parts = parser.ReadFields();
                    return parts;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't find a file to load words from.");
                throw;
            }
        }

        /// <summary>
        /// Validates the guess
        /// </summary>
        /// <param name="guess">The users guess</param>
        /// <returns>True if the guess is valid, otherwise false.</returns>
        private static bool ValidGuess(string guess)
        {
            // Must be alphabetical, and a single character.
            return (guess.Length >= 1) 
                && Regex.IsMatch(guess, @"^[a-z,å-ö,A-Z,Å-Ö,' ']+$") //is one or more letter or space
                && !guesses.ToString().Contains(guess);
        }

        /// <summary>
        /// Gets a random int in the interval min-max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Randomized int</returns>
        public static int GetRandomInt(int min, int max)
        {
            Random randNum = new();
            return randNum.Next(min, max);
        }

        /// <summary>
        /// Draws the graphics and finally checks if the last guessed character completes the puzzle
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static void DrawGallows(int state)
        {
            correct = 0;
            Console.Clear();

            foreach (string row in HangAscii[state]) //the pretty graphics
            {
                Console.WriteLine(row);
            }

            foreach (char item in secret) //check each guess against the secret
            {
                if (guesses.ToString().Contains(item))
                {
                    Console.Write(item + " ");
                    correct++;
                }
                else
                {
                    Console.Write("_ ");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Incorrect guesses: " + incorrect.ToString());
        }

        readonly string[] states = { "drop!",
                                "leg2",
                                "leg1",
                                "arm2",
                                "arm1",
                                "torso",
                                "head",
                                "rope",
                                "top",
                                "Pole",
                                "foundation"};

        static readonly string[][] HangAscii = new string[][] {
                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |         .-`--'.",
                @" | |        / Y. .Y\\",
                @" | |       // |   | \\",
                @" | |      //  | . |  \\",
                @" | |     ')   |   |   (`",
                @" | |          ||'||",
                @" | |          || ||",
                @" | |          || ||",
                @" | |          || ||",
                @" | |         / | | \",
                @" | |         `-' `-'",
                @" |_|_________         ___",
                @" | _________ \       |_  |",
                @" | |        \ \        | |",
                @" | |         \ \       | |",
                @" | |          `'       | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |         .-`--'.",
                @" | |        / Y. .Y\\",
                @" | |       // |   | \\",
                @" | |      //  | . |  \\",
                @" | |     ')   |   |   (`",
                @" | |          ||'||",
                @" | |          || ||",
                @" | |          || ||",
                @" | |          || ||",
                @" | |         / | | \",
                @" | |         `-' `-'",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |         .-`--'.",
                @" | |        / Y. .Y\\",
                @" | |       // |   | \\",
                @" | |      //  | . |  \\",
                @" | |     ')   |   |   (`",
                @" | |          ||'",
                @" | |          ||",
                @" | |          ||",
                @" | |          ||",
                @" | |         / |",
                @" | |         `-'",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |         .-`--'.",
                @" | |        / Y. .Y\\",
                @" | |       // |   | \\",
                @" | |      //  | . |  \\",
                @" | |     ')   |   |   (`",
                @" | |          -----",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |         .-`--'.",
                @" | |        / Y. .Y",
                @" | |       // |   |",
                @" | |      //  | . |",
                @" | |     ')   |   |",
                @" | |          -----",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))______ | ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |          -`--'.",
                @" | |          |. .||",
                @" | |          |   |",
                @" | |          | . |",
                @" | |          |   |",
                @" | |          -----",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},


                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||.- ''.",
                @" | |/         |/ _  \",
                @" | |          ||  `/,|",
                @" | |          (\\`_.'",
                @" | |           `--'.",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"  ____________.._______",
                @" | .__________))_______| ",
                @" | | / /      ||",
                @" | |/ /       ||",
                @" | | /        ||",
                @" | |/         ||",
                @" | |          ||",
                @" | |          /.\",
                @" | |         (( ))",
                " | |           \"",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"   ____________________",
                @" | .___________________| ",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {" ___",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" | |",
                @" |_|_____________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

                new string[]
                {"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @"",
                @" ________________________",
                @" | ____________________  |",
                @" | |                   | |",
                @" | |                   | |",
                @" | |                   | |"},

            };


    }
}


//
