using System;
using System.Linq;

namespace aoc_2022_cli;

// This Data File Reader is meant to be used in a very specific way.
// if no parameter is suppied, it will create the file name it wants to use from the date.
// You can overwrite the filename by supplying it.
public class DataFileReader
{
    public string FullPath { get; set; }
    public List<string> Lines { get; set; } = new List<string>();

    public DataFileReader(string? filename, bool debugMode = false, bool runningTests = false)
    {
        if (!String.IsNullOrEmpty(filename))
        {
            FullPath = filename;
        }
        else
        {
            // get the current directory
            string cwd = Directory.GetCurrentDirectory();
            // This is tightly coupled to directory structure. I'm cool with that.
            string path = Path.GetFullPath(Path.Combine(cwd, @"../../../Data"));
            string testPath = Path.GetFullPath(Path.Combine(cwd, @"../../../../aoc-2022-cli/Data"));
            var now = DateTime.Now.ToString("dd-MM-yyyy");
            // In for a penny in for a pound.
            // This assumes you are creating a data file each day with a specific name format: dd-MM-yyyy-data.txt
            // If that is not the case, this will obviously crash and burn but I'm OK with that for this project.
            var filePath = runningTests ? testPath : path;
            var fullPath = $"{filePath}/{now}-data.txt";

            FullPath = fullPath;

            if (debugMode)
            {
                Console.WriteLine($"DataFileReader - ctor - fullPath: {FullPath}");
            }
        }
    }

    // could save this value during creation but this offers more fine grained logging control
    public void ReadFile(bool debugMode = false)
    {
        try
        {
            if (File.Exists(FullPath))
            {
                var strArr = File.ReadAllLines(FullPath);                
                Lines = new List<string>(strArr);

                if (debugMode)
                {
                    Console.WriteLine($"DataFileReader - ReadFile - strArr.Length: {strArr.Length}");
                    Console.WriteLine($"DataFileReader - ReadFile - Lines.Count: {Lines.Count}");
                }

            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Reading File: {FullPath}");
            Console.WriteLine($"stack trace: {e}");
        }
    }
}

