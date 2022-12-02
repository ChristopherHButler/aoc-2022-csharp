
using aoc_2022_cli.Puzzles;

namespace aoc_2022_cli;

class Program
{
    static void Main(string[] args)
    {
        Run();

        // Wait for user
        Console.Read();
    }

    private static void Run()
    {
        Console.WriteLine("Press Enter to run today's puzzle or the (two digit) date of the day you want to solve: ");

        var userInputAsText = Console.ReadLine();
        var runTodaysPuzzle = String.IsNullOrEmpty(userInputAsText);

        if (runTodaysPuzzle)
        {
            // run today's...do this later
            RunDay02();
        }
    }

    private static void RunDay01()
    {
        var dec01 = new Dec01();
        dec01.SolvePartOne();
        dec01.SolvePartTwo();
    }

    private static void RunDay02()
    {
        var dec02 = new Dec02();
        dec02.DisplayResults();
    }
}

