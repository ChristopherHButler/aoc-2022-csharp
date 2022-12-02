using aoc_2022_cli.Puzzles;

namespace aoc_2022_cli;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Press Enter to run today's puzzle or the (two digit) date of the day you want to solve: ");

        var userInputAsText = Console.ReadLine();
        var runTodaysPuzzle = String.IsNullOrEmpty(userInputAsText);

        if (runTodaysPuzzle)
        {
            // run today's...do this later
        }

        // only one puzzle to run right now so wil fix later
        var dec01 = new Dec01();
        dec01.SolvePartOne();
        dec01.SolvePartTwo();

        // Wait for user
        Console.Read();
    }
}

