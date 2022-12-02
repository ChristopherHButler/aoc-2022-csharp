using System;
using aoc_2022_cli.Entities;
using aoc_2022_cli.Helpers;

namespace aoc_2022_cli.Puzzles;

public class Dec02
{
    public Player MePart1 = new Player("Chris Part 1");
    public Player MePart2 = new Player("Chris Part 2");

    public Dec02(bool runningTests = false, bool debugMode = false)
    {
        Run(runningTests, debugMode);
    }

    public void Run(bool runningTests, bool debugMode)
    {
        var dfr = new DataFileReader(filename: "", debugMode: true);
        dfr.ReadFile(debugMode: true);

        for (int i = 0; i < dfr.Lines.Count; i++)
        {
            var round = dfr.Lines[i];
            var moves = round.Split(' ');

            var elfMove = RockPaperScissors.ConvertMove(moves[0]);
            var myMove = RockPaperScissors.ConvertMove(moves[1]);
            var outcome = RockPaperScissors.ConvertResult(moves[1]);

            // sanity check
            if (i == 0 && debugMode)
            {
                Console.WriteLine($"elf move 0: {moves[0]} your move 0: {moves[1]}");
            }

            var myRound = RockPaperScissors.GetMyRoundResults(myMove, elfMove);
            var myRound2 = RockPaperScissors.GetMyRound2Results(elfMove, outcome);

            MePart1.Rounds.Add(myRound);
            MePart2.Rounds.Add(myRound2);

            if (debugMode)
            {
                Console.WriteLine($"round added: {myRound.Result}, points: {myRound.Points}");
                Console.WriteLine($"round added: {myRound2.Result}, points: {myRound2.Points}");
            }
        }
    }

    private void DisplayPart1()
    {
        MePart1.ComputeMyStats();
        MePart1.DisplayMyStats();
    }

    private void DisplayPart2()
    {
        MePart2.ComputeMyStats();
        MePart2.DisplayMyStats();
    }

    public void DisplayResults()
    {
        DisplayPart1();
        Console.WriteLine("");
        Console.WriteLine("----------");
        Console.WriteLine("");
        DisplayPart2();
    }
}

