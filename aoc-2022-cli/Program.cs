
using aoc_2022_cli.Puzzles;

namespace aoc_2022_cli;

class Program
{
    static void Main(string[] args)
    {
        RunDay03();

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
        }
    }

    private static void RunDay01()
    {
        var dec01 = new Dec01(date: "01-12-2022");
        dec01.SolvePartOne();
        dec01.SolvePartTwo();
    }

    private static void RunDay02()
    {
        var dec02 = new Dec02(date: "02-12-2022");
        dec02.DisplayResults();
    }

    private static void RunDay03()
    {
        var dec03 = new Dec03(date: "03-12-2022");

        dec03.DefragRuckSacks(debugMode: true);
        dec03.DisplayPart1Solution();
        dec03.DisplayPart2Solution();
    }
}

