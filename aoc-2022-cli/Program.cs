
using aoc_2022_cli.Puzzles;

namespace aoc_2022_cli;

class Program
{
    static void Main(string[] args)
    {
        RunDay09();

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

    private static void RunDay04()
    {
        var results = Dec04.FindFullyContainedRanges(date: "04-12-2022", debugMode: true);

        Console.WriteLine($"The count of fully contained sets is: {results[0]}");
        Console.WriteLine($"The count of intersecting sets is: {results[1]}");
    }

    private static void RunDay05()
    {
        var dec05 = new Dec05();
        dec05.Solve(date: "05-12-2022");
    }

    public static void RunDay06()
    {
        var dec06 = new Dec06();
        dec06.Solve(date: "06-12-2022");
    }

    public static void RunDay07()
    {
        var dec07 = new Dec07(date: "07-12-2022");
        dec07.RunPart1();
        dec07.RunPart2();
    }

    private static void RunDay08()
    {
        var dec08 = new Dec08(date: "08-12-2022");
    }

    private static void RunDay09()
    {
        var dec09 = new Dec09(date: "09-12-2022");
    }
}

