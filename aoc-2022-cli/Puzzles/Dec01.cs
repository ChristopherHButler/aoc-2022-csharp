using System;
using aoc_2022_cli.Entities;

namespace aoc_2022_cli.Puzzles;

public class Dec01
{
    public List<Elf> Elves { get; set; } = new List<Elf>();

    public Dec01(string? date = null, bool runningTests = false)
    {
        CreateElves(date, runningTests);
        LogElfData();
    }

    // Is the ugliness of this code buried deeply enough uncle bob?
    private void CreateElves(string? date, bool runningTests)
    {
        // create a data file reader and read the file.
        var dfr = new DataFileReader(date: date, runningTests: runningTests);
        dfr.ReadFile();

        // track the "current" elf
        var currentElf = 0;

        // need to reset this for each elf
        List<int> currentElfItems = new List<int>();

        for (int i = 0; i < dfr.Lines.Count; i++)
        {
            var currentItem = dfr.Lines[i];
            if (String.IsNullOrEmpty(currentItem))
            {
                // end of elf items.
                // create new elf and add to list
                var elf = new Elf(name: currentElf.ToString())
                {
                    Items = currentElfItems,
                    TotalCalories = currentElfItems.Sum(item => item)
                };

                Elves.Add(elf);

                // reset and update
                currentElfItems.Clear();
                currentElf++;
            }
            else
            {
                // add current item calories to list
                currentElfItems.Add(Convert.ToInt32(dfr.Lines[i]));
            }
        }
    }

    private void LogElfData()
    {
        Console.WriteLine($"There are {Elves.Count} elves");
        Console.WriteLine("");
    }

    public Elf GetMaxCalElf()
    {
        var MaxCalElf = Elves.MaxBy(e => e.TotalCalories);
        return MaxCalElf == null ? new Elf(name: "nully") : MaxCalElf;
    }

    public List<Elf> OrderElves()
    {
        return Elves.OrderByDescending(e => e.TotalCalories).ToList();
    }

    public void SolvePartOne()
    {
        var pappaElf = GetMaxCalElf();

        Console.WriteLine($"Elf {pappaElf?.Name} has {pappaElf?.Items.Count} items and a total of {pappaElf?.TotalCalories} calories");
        Console.WriteLine("");
    }

    public void SolvePartTwo()
    {
        List<Elf> orderedElves = OrderElves();

        var firstElf = orderedElves[0];
        var secondElf = orderedElves[1];
        var ThirdElf = orderedElves[2];

        var topThreeElvesCalories = firstElf.TotalCalories + secondElf.TotalCalories + ThirdElf.TotalCalories;

        Console.WriteLine($"Calories for the top three elves: {topThreeElvesCalories}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Elf 1 has {firstElf.Items.Count} snacks with: {firstElf.TotalCalories} calories");
        Console.WriteLine($"Elf 2 has {secondElf.Items.Count} snacks with: {secondElf.TotalCalories} calories");
        Console.WriteLine($"Elf 3 has {ThirdElf.Items.Count} snacks with: {ThirdElf.TotalCalories} calories");
    }
}

