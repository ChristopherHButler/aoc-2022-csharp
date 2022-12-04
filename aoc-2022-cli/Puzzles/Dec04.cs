using System;
namespace aoc_2022_cli.Puzzles;

public class Dec04
{

    public static int[] FindFullyContainedRanges(string? date = null, bool runningTests = false, bool debugMode = false)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: runningTests, debugMode: debugMode);
        dfr.ReadFile(debugMode: debugMode);

        var fullyContainedCount = 0;
        var intersectCount = 0;

        for (var i = 0; i < dfr.Lines.Count; i++)
        {
            // dfr.Lines[0] = 18-20,19-21
            string[] assignmentPairs = dfr.Lines[i].Split(',');

            if (debugMode && i == 0)
            {
                Console.WriteLine($"assignment pairs - pair1: {assignmentPairs[0]} | pair2: {assignmentPairs[1]}");
            }

            var assignment1 = ConvertStringToList(assignmentPairs[0]);
            var assignment2 = ConvertStringToList(assignmentPairs[1]);

            bool isSubset = assignment2.IsSubsetOf(assignment1) || assignment1.IsSubsetOf(assignment2);

            bool intersect = assignment2.Intersect(assignment1).ToList().Count != 0;

            if (isSubset) fullyContainedCount++;
            if (intersect) intersectCount++;
        }

        var results = new int[2] { fullyContainedCount, intersectCount };
        return results;
    }

    private static HashSet<int> ConvertStringToList(string assignment)
    {
        // 18-20 => 18, 20
        string[] bounds = assignment.Split('-');
        if (bounds.Length != 2)
            throw new ArgumentOutOfRangeException();
        var start = Convert.ToInt32(bounds[0]);
        var end = Convert.ToInt32(bounds[1]);
        var count = end - start + 1;
        var set = Enumerable.Range(start, count).ToHashSet();

        return set;
    }
}

