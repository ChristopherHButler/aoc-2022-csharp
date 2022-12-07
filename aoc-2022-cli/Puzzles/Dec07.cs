using System;
using System.IO;
using System.Linq;

namespace aoc_2022_cli.Puzzles;

public class Dec07
{
    private class File
    {
        public string FileName { get; set; } = "";
        public int Size { get; set; } = 0;
        public Directory? Parent { get; set; }
    }

    private class Directory
    {
        public string Name { get; set; } = "";
        public long TotalSize { get; set; } = 0;
        public int Size { get; set; } = 0;
        public Directory? Parent { get; set; }
        public List<File> Files { get; set; } = new List<File>();
        public List<Directory> Dirs { get; set; } = new List<Directory>();

        public Directory(string name)
        {
            Name = name;
        }

        public long GetTotalSize()
        {
            long totalSize = 0;
            foreach (File f in Files)
            {
                totalSize += f.Size;
            }
            foreach (Directory d in Dirs)
            {
                totalSize += d.GetTotalSize();
            }
            this.TotalSize = totalSize;
            return totalSize;
        }
    }

    private Directory Root { get; set; } = new Directory(name: "root")
    {
        Parent = null,
    };
    private Directory CurrentDirectory { get; set; }
    private List<Directory> UnderHtPart1 { get; set; } = new List<Directory>();
    private List<Directory> AllDirs { get; set; } = new List<Directory>();

    private long TotalDiskSpace =    70000000;
    private long RequiredFreeSpace = 30000000;

    public Dec07(string? date = null)
    {
        CurrentDirectory = Root;
        ConstructFileSystemFromLog(date);

        Console.WriteLine($"root: {Root}");
    }

    public void RunPart1()
    {
        // Part 1
        CalculateTotalFolderSizeRec(curr: Root);
        GetPart1Dirs(Root);
        var SumOfDirsWithSizeUnder100000 = CalcPart1Size();
        // the sum of the total sizes of the directories with a total size of at most 100000 is: 1477771
        Console.WriteLine($"the sum of the total sizes of the directories with a total size of at most 100000 is: {SumOfDirsWithSizeUnder100000}");
    }

    public void RunPart2()
    {
        // Target Dir: 
        // Name: wjznmcw
        // Parent: hggcq
        // File Count: 4
        // Size: 295354
        // Folder Count: 4
        // Total Size: 3579501

        // 1. Get All Directories
        AllDirs = GetAllDirs(Root);

        // 2. Find the total used space.
        var TotalUsedSpace = CalcTotalUsedSpace(Root);

        // 3. Compute space required
        var remainingSpaceRequired = ComputeSpaceRequiredToFree(TotalUsedSpace);

        SetTotalSize();

        // 4. order the directories
        AllDirs = OrderAllDirs(AllDirs);

        // Find the target
        for (int i = 0; i < AllDirs.Count; i++)
        {
            if (AllDirs[i].TotalSize > remainingSpaceRequired)
            {
                Console.WriteLine("Previous Dir: ");
                PrintDirectoryInfo(AllDirs[i - 1], details: true);

                Console.WriteLine("Target Dir: ");
                PrintDirectoryInfo(AllDirs[i], details: true);

                Console.WriteLine("Next Dir: ");
                PrintDirectoryInfo(AllDirs[i + 1], details: true);
                break;
            }
        }
    }

    private void ConstructFileSystemFromLog(string? date)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: false, debugMode: false);
        dfr.ReadFile(debugMode: false);

        var logs = dfr.Lines;

        for (var i = 0; i < logs.Count; i++)
        {
            // Console.WriteLine($"log[{i}]");
            ProcessLog(log: logs[i]);
        }
        Console.WriteLine("File System Construction complete");
        
    }

    private void ProcessLog(string log)
    {
        var parts = log.Split(' ');

        // running command
        if (parts[0] == "$")
        {
            ProcessCommand(parts);
        }
        // item is dir and dir is part of current directory
        else if (parts[0] == "dir")
        {
            ProcessFolder(log: log);
        }
        // item is a file and file is part of current dir
        else if (Convert.ToInt32(parts[0]) != 0)
        {
            ProcessFile(log: log);
        }
    }

    private void ProcessCommand(string[] parts)
    {
        // command is change dir
        if (parts[1] == "cd")
            ProcessCd(parts);
        
        // command is list items in dir
        else if (parts[1] == "ls")
        {
            // here we can set something to say until the next '$'
            // all items are either files or folders in the current dir
            // we don't actually need to do anything here though...
        }
    }

    private void ProcessCd(string[] parts)
    {
        // skip command to switch to root
        if (parts[2] == "/")
        {
            CurrentDirectory = Root;
            return;
        }

        // cd in - literally change the current directory
        if (parts[2] != "..")
        {
            // find this directory in the current directory
            var newCurrentDirectory = CurrentDirectory.Dirs.Find(d => d.Name == parts[2]);

            if (newCurrentDirectory != null)
            {
                CurrentDirectory = newCurrentDirectory;
                return;
            }
            else
                throw new ArgumentNullException();
        }

        // cd out - literally change the current directory
        if (parts[2] == "..")
        {
            if (CurrentDirectory.Parent != null)
            {
                var newCurrentDirectory = CurrentDirectory.Parent;
                CurrentDirectory = newCurrentDirectory;
                return;
            }
            else
                throw new ArgumentNullException();   
        }
    }

    private void ProcessFolder(string log)
    {
        var dirName = log.Split(' ')[1];

        // check if directory already exists in parent
        if (CurrentDirectory.Dirs.Exists(d => d.Name == dirName)) return;

        // create a directory and add to parent dir
        
        var dir = new Directory(name: dirName)
        {
            Parent = CurrentDirectory,
        };
        CurrentDirectory.Dirs.Add(dir);
    }

    private void ProcessFile(string log)
    {
        // create a file here and add to parent directory
        var file = new File()
        {
            FileName = log.Split(' ')[1],
            Size = Convert.ToInt32(log.Split(' ')[0]),
            Parent = CurrentDirectory,
        };

        CurrentDirectory.Files.Add(file);
        // add the file to the direct folder size and the folder total size
        CurrentDirectory.Size += file.Size;
    }

    private void CalculateTotalFolderSizeRec(Directory curr)
    {
        // starting at the root directory...
        // for each sub folder...
        foreach (var child in curr.Dirs)
        {
            // does the sub folder have children? keep going
            if (child.Dirs.Count > 0)
            {
                CalculateTotalFolderSizeRec(child);
                foreach (var gc in child.Dirs)
                {
                    child.TotalSize += gc.TotalSize;
                }
                curr.TotalSize += child.TotalSize;
            }
            // if you have a folder with NO children, only files
            else
            {
                // instead of adding all files here, we did this when creating the file
                child.TotalSize = child.Size;
            }
        }
        curr.TotalSize += curr.Size;
    }

    private void GetPart1Dirs(Directory curr)
    {
        foreach (var folder in curr.Dirs)
        {
            if (folder.Dirs.Count > 0)
            {
                GetPart1Dirs(folder);
                if (folder.TotalSize <= 100000)
                    UnderHtPart1.Add(folder);
            }
            else
            {
                if (folder.TotalSize <= 100000)
                    UnderHtPart1.Add(folder);
            }
        }
    }

    private long CalcPart1Size()
    {
        long size = 0;
        foreach (var dir in UnderHtPart1)
        {
            // PrintDirectoryInfo(dir, details: true);
            size += dir.TotalSize;
        }
        return size;
    }

    // -------------------------------------------

    private List<Directory> GetAllDirs(Directory curr)
    {
        var allDirs = new List<Directory>();
        allDirs.Add(curr);
        foreach (var dir in curr.Dirs)
        {
            allDirs.AddRange(GetAllDirs(dir));
        }
        return allDirs;
    }

    private long CalcTotalUsedSpace(Directory curr)
    {
        long totalSpace = 0;
        foreach (File f in curr.Files)
        {
            totalSpace += f.Size;
        }
        foreach (Directory d in curr.Dirs)
        {
            totalSpace += CalcTotalUsedSpace(d);
        }
        return totalSpace;
    }

    private long ComputeSpaceRequiredToFree(long TotalUsedSpace)
    {
        var currentSpaceAvailable = TotalDiskSpace - TotalUsedSpace;

        long remainingSpaceRequired = 0;
        if (currentSpaceAvailable < RequiredFreeSpace)
        {
            remainingSpaceRequired = RequiredFreeSpace - currentSpaceAvailable;
        }
        return remainingSpaceRequired;
    }

    private void SetTotalSize()
    {
        foreach (var d in AllDirs)
        {
            d.TotalSize = d.GetTotalSize();
        }
    }

    private List<Directory> OrderAllDirs(List<Directory> AllDirs)
    {
        return AllDirs.OrderBy(d => d.TotalSize).ToList();
    }


    private void PrintDirectoryInfo(Directory dir, bool details = false)
    {
        if (details == true)
        {
            Console.WriteLine($"");
            Console.WriteLine($"Name: {dir.Name}");
            Console.WriteLine($"Parent: {dir.Parent?.Name}");
            Console.WriteLine($"File Count: {dir.Files.Count}");
            Console.WriteLine($"Size: {dir.Size}");
            Console.WriteLine($"Folder Count: {dir.Dirs.Count}");
            Console.WriteLine($"Total Size: {dir.TotalSize}");
            Console.WriteLine($"");
        }
        else
        {
            Console.WriteLine($"Dir Name: {dir.Name}, Total Size: {dir.TotalSize}");
        }
        
    }

    private void PrintFileInfo(File file)
    {
        Console.WriteLine($"Name: ${file.FileName}, Size: {file.Size}, Parent: {file.Parent.Name}");
    }
}

