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
        static string wordPath = "Words.txt";
        static void Main(string[] args)
        {

            while (true)
            {
                if (args.Length == 1) wordPath = args[0]; //accepts user defined words if specified

                Console.Clear();
                Console.WriteLine("Try your luck at Hangman! Press the any key to start or ESC to close!");
                if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                Game game = new Game();
                game.HangGame(wordPath);
            }
        }
    }

    class Game
    {
        static StringBuilder incorrect;
        static char[] guesses;
        static int tries, correct;
        static string secret;
        /// <summary>
        /// Main game function
        /// </summary>
        /// <param name="file">Path to the file to load words from</param>
        public void HangGame(string file)
        {
            //List<char> guesses = new List<char>();
            tries = 10;
            guesses = new char[30];
            incorrect = new StringBuilder();
            AddToArray(' '); //since some places have spaces in their names, and that's not obvious to the player
            string[] HangWords = ReadWords(file);
            if (HangWords.Length == 0) //make sure we have something to play with
            {
                Console.WriteLine("Couldn't find any csv file with words to load.\n" +
                    "Put a file name named Words.txt in the program folder, or specify your own with a launch argument.");
                return;
            }
            else
            {
                secret = HangWords[GetRandomInt(0, HangWords.Length)].ToLower();
            }
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
                    if (guess.Length == 1) //guessing a single letter
                    {
                        AddToArray(char.Parse(guess));
                        if (!secret.Contains(guess))
                        {
                            tries--;
                            incorrect.Append(guess);
                        }
                    }
                    else if (guess == secret) //guessing the right word(s)
                    {
                        Console.WriteLine("\nWell done, you guessed it!");
                        Console.ReadLine();
                        return;
                    }
                    else
                    { //incorrect word guesses
                        tries--;
                    }
                }
                else
                { //mostly a catch-all for things not considered
                    Console.WriteLine("That's not a valid guess!");
                }


            }
            DrawGallows(tries); //for that last, gritty piece of reminder that you skills were insufficient to save that poor ASCII dude
            Console.WriteLine("Sorry! You lose, we were looking for " + secret);

            Console.ReadLine();
        }

        /// <summary>
        /// Adds a char to the first free slot in the guesses array
        /// </summary>
        /// <param name="ch"></param>
        static void AddToArray(char ch)
        {
            for (int i = 0; i < guesses.Length; i++)
            {
                if (guesses[i] == '\0')
                {
                    guesses[i] = ch;
                    break;
                }
            }
        }

        /// <summary>
        /// Reads the words to load into the program
        /// </summary>
        /// <param name="file">Path to csv (,) delimited text file</param>
        /// <returns>String array of words read from the specified file</returns>
        public static string[] ReadWords(string file)
        {
            try
            {
                using TextFieldParser parser = new TextFieldParser(file);
                parser.Delimiters = new string[] { "," };
                while (true)
                {
                    string[] parts = parser.ReadFields();
                    return parts;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't find a proper csv file to load words from.");
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
            if (guess.Length==1)
            {
                return (guess.Length >= 1)
                && Regex.IsMatch(guess, @"^[a-z,å-ö,A-Z,Å-Ö,' ']+$") //is one or more letter or space
                && !guesses.Contains(char.Parse(guess));
            }
            else if (guess.Length >= 1)
            {
                return Regex.IsMatch(guess, @"^[a-z,å-ö,A-Z,Å-Ö,' ']+$"); //is one or more letter or space
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Gets a random int in the interval min-max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Randomized int</returns>
        public static int GetRandomInt(int min, int max)
        {
            Random randNum = new Random();
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
                if (guesses.Contains(item))
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

        /// <summary>
        /// 2D array of the ASCII depicting a poor, dismembered soul being put back together just to be hanged
        /// </summary>
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
