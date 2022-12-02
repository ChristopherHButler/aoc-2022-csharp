using System;

namespace aoc_2022_cli.Entities;

public enum RPSMoves
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public enum Result
{
    Win,
    Loss,
    Draw,
}

public class Round
{
    public RPSMoves Move { get; set; }
    public Result Result { get; set; }
    public int Points { get; set; }
}

public class Player
{
    public string Name { get; set; } = "";
    public List<Round> Rounds { get; set; } = new List<Round>();
    public int Points { get; set; } = 0;
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;
    public int Draws { get; set; } = 0;

    public Player(string name)
    {
        Name = name;
    }

    public void ComputeMyStats()
    {
        foreach (var round in Rounds)
        {
            Points += round.Points;
            if (round.Result == Result.Win)
                Wins++;
            else if (round.Result == Result.Draw)
                Draws++;
            else if (round.Result == Result.Loss)
                Losses++;
        }
    }

    public void DisplayMyStats()
    {
        Console.WriteLine($"My name is {Name}");
        Console.WriteLine($"My Total Points: {Points}");
        Console.WriteLine($"My Wins:         {Wins}");
        Console.WriteLine($"My Draws:        {Draws}");
        Console.WriteLine($"My losses:       {Losses}");
        Console.WriteLine($"Total: {Wins + Draws + Losses}");
    }
}
