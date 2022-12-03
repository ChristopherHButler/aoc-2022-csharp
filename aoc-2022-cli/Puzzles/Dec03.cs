using System;
using aoc_2022_cli.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aoc_2022_cli.Puzzles;

public class Dec03
{
    public int DuplicateSum { get; set; }
    public int IntersectionSum { get; set; }

    public Dec03(string? date = null, bool runningTests = false, bool debugMode = false)
    {
     

    }

    public void DefragRuckSacks(string? date = null, bool runningTests = false, bool debugMode = false)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: runningTests, debugMode: debugMode);
        dfr.ReadFile(debugMode: debugMode);

        if (debugMode)
        {
            Console.WriteLine($"Total Number of Packs: {dfr.Lines.Count}");
        }

        var invalid = false;

        for (var i = 0; i < dfr.Lines.Count; i++)
        {
            // vJrwpWtwJgWrhcsFMMfFFhFp
            var rucksackItems = dfr.Lines[i];

            if (debugMode)
                Console.WriteLine($"rucksackItems: {rucksackItems}");

            if (!validateItems(rucksackItems.Length))
            {
                invalid = true;
                Console.WriteLine($"Pack {i} is not valid: {rucksackItems}");
                throw new ArgumentOutOfRangeException();
            }

            var compartmentSize = rucksackItems.Length / 2;
            var compartment1Items = rucksackItems.Substring(0, compartmentSize);
            var compartment2Items = rucksackItems.Substring(compartmentSize, compartmentSize);

            Part1FindSharedItems(compartment1Items, compartment2Items);

            if (i % 3 == 0 && i + 2 <= dfr.Lines.Count)
            {                
                Part2FindSharedItems(group1: dfr.Lines[i], group2: dfr.Lines[i+1], group3: dfr.Lines[i+2]);
            }                     
        }
        if (!invalid)
        {
            Console.WriteLine("All packs are valid");
        }
    }

    public void Part1FindSharedItems(string compartment1, string compartment2)
    {
        var compartment1Items = ConvertStringToCharList(compartment1);
        var compartment2Items = ConvertStringToCharList(compartment2);

        // doesn't really make sense to loop again aside from good coding practices...
        // need to compare compartment 1 and 2 for duplicates
        // we KNOW the elf only mixed up exactly 1 item.
        var duplicates = compartment1Items.Intersect(compartment2Items).ToList();

        if (duplicates.Count > 0)
        {
            // Console.WriteLine($"duplicate: {duplicates[0]}");
            DuplicateSum += GetItemPriority(duplicates[0]);
        }
    }

    public void Part2FindSharedItems(string group1, string group2, string group3)
    {
        var group1Items = ConvertStringToCharList(group1);
        var group2Items = ConvertStringToCharList(group2);
        var group3Items = ConvertStringToCharList(group3);

        var groups1And2Dups = group1Items.Intersect(group2Items).ToList();
        var fullIntersection = groups1And2Dups.Intersect(group3Items).ToList();

        if (fullIntersection.Count > 0)
        {
            IntersectionSum += GetItemPriority(fullIntersection[0]);
        }
    }

    public void DisplayPart1Solution()
    {
        Console.WriteLine($"Part 1: The sum is {DuplicateSum}");
    }
    public void DisplayPart2Solution()
    {
        Console.WriteLine($"Part 2: The sum is {IntersectionSum}");
    }

    private bool validateItems(int itemsLength)
    {
        return itemsLength % 2 == 0;
    }

    private char[] ConvertStringToCharList(string compartmentItems)
    {
        var list = compartmentItems.ToCharArray();
        return list;
    }

    public int GetItemPriority(char itemType)
    {
        int index = (int)itemType % 32;
        if (Char.IsUpper(itemType)) return index + 26;
        return index;
    }
}

