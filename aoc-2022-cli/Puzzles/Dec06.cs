using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aoc_2022_cli.Puzzles;

public class Dec06
{
    private class Entry
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public Entry(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public Dictionary<int, string> Chunks { get; set; } = new Dictionary<int, string>();

    public void Solve(string? date = null, bool? debugMode = false)
    {
        var part1ChunkSize = 4;
        var part2ChunkSize = 14;

        var chunkSize = part2ChunkSize;

        var dfr = new DataFileReader(filename: "", date: date, runningTests: false, debugMode: false);
        dfr.ReadFile(debugMode: false);

        ChunkStringToDictionary(stream: dfr.Lines[0], chunkSize: chunkSize);

        if (debugMode == true) PrintEntries();

        var index = FindStartOfPacketMarker(chunkSize: chunkSize);

        Console.WriteLine($"The start-of-packet index is: {index}");
    }

    private void ChunkStringToDictionary(string stream, int chunkSize)
    {
        var chars = stream.ToCharArray();

        for(var i = 0; i < chars.Length - (chunkSize-1); i++)
        {
            var chunk = stream.Substring(i, chunkSize); // $"{chars[i]}{chars[i + 1]}{chars[i + 2]}{chars[i + 3]}";
            var entry = new Entry(i, chunk);

            // create a dictornary entry
            Chunks.Add(entry.Key, entry.Value);
        }
    }

    private int FindStartOfPacketMarker(int chunkSize)
    {
        for (int i = 0; i < Chunks.Count; i++)
        {
            var target = IsSequenceUnique(Chunks[i]);
            if (target == true) return i + chunkSize;
        }
        return -1;
    }

    // a sequence is a 4 char string.
    private bool IsSequenceUnique(string sequence)
    {
        for (var i = 0; i < sequence.Length; i++)
        {
            var current = sequence[i];
            for (var j = 0; j < sequence.Length; j++)
            {
                if ((i != j) && current == sequence[j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PrintEntries()
    {
        Console.WriteLine("All Entries: ");
        Console.WriteLine("-----------------------");
        for (int i = 0; i < Chunks.Count; i++)
        {
            Console.WriteLine($"[{i}]: {Chunks[i]}");
        }
    }
}

