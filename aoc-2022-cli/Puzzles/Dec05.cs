using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc_2022_cli.Puzzles;

//[T]     [D]         [L]            
//[R]     [S] [G]     [P]         [H]
//[G]     [H] [W]     [R] [L]     [P]
//[W]     [G] [F] [H] [S] [M]     [L]
//[Q]     [V] [B] [J] [H] [N] [R] [N]
//[M] [R] [R] [P] [M] [T] [H] [Q] [C]
//[F] [F] [Z] [H] [S] [Z] [T] [D] [S]
//[P] [H] [P] [Q] [P] [M] [P] [F] [D]
// 1   2   3   4   5   6   7   8   9

// Ans: TPGVQPFDH
// Part 2:  DMRDFRHHH

public class Dec05
{
    public List<Stack<char>> Stacks { get; set; } = new List<Stack<char>>()
        {
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20),
            new Stack<char>(20)
        };

    public void Solve(string? date = null)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: false, debugMode: false);
        dfr.ReadFile(debugMode: false);

        // Console.WriteLine($"First move: {dfr.Lines[10]}");
        // ParseStackRow(dfr.Lines[0]);

        ParseStacks(rows: dfr.Lines);

        // RearrangeStacks(rows: dfr.Lines);
        RearrangeStacksPart2(rows: dfr.Lines);

        // print stack - destructive process
        // PrintStacks();

        PrintStacksTops();
    }

    private void ParseStacks(List<string> rows)
    {
        for (var i = 0; i < 8; i++)
        {
            // Console.WriteLine($"parsing stack: {rows[i]}");
            ParseStackRow(rows[i]);
        }
        for (var i = 0; i < 9; i++)
        {
            Stacks[i] = ReverseStack(Stacks[i]);
            // Console.WriteLine($"Stack: {Stacks[i]}");
        }

    }

    private void RearrangeStacks(List<string> rows)
    {
        for (var i = 10; i < rows.Count; i++)
        {
            // move numberOfCrates from fromStack to toStack
            var instructions = ParseMoveRow(rows[i]);

            var numberOfCrates = instructions[0];
            var fromStack = instructions[1];
            var toStack = instructions[2];

            for (var j = 0; j < numberOfCrates; j++)
            {
                MoveCrate(from: Stacks[fromStack], to: Stacks[toStack]);
            }
        }
    }

    private void RearrangeStacksPart2(List<string> rows)
    {
        for (var i = 10; i < rows.Count; i++)
        {
            // move numberOfCrates from fromStack to toStack
            var instructions = ParseMoveRow(rows[i]);

            var numberOfCrates = instructions[0];
            var fromStack = instructions[1];
            var toStack = instructions[2];

            var tempStack = new Stack<char>(20);

            for (int j = 0; j < numberOfCrates; j++)
            {
                MoveCrate(from: Stacks[fromStack], to: tempStack);
            }

            for (int j = 0; j < numberOfCrates; j++)
            {
                MoveCrate(from: tempStack, to: Stacks[toStack]);
            }
        }
    }

    private void ParseStackRow(string stackRow)
    {
        var parsedRow = stackRow.ToCharArray();

        // add crates to stacks...
        for (int i = 0; i < 9; i++)
        {
            var crate = parsedRow[GetStackIndex(i + 1)];
            if (crate == ' ') continue;
            Stacks[i].Push(crate);
        }
        
    }

    private int GetStackIndex(int stack)
    {
        if (!Enumerable.Range(1, 9).ToArray().Contains(stack))
            throw new ArgumentOutOfRangeException();

        if (stack == 1) return 1;
        return GetStackIndex(stack - 1) + 4;
    }

    private int[] ParseMoveRow(string row)
    {
        // need to parse moves
        // move 3 from 8 to 9
        // move 15 from 4 to 1
        // parse on empty space
        // move numberOfCrates from fromStack to toStack
        string[] parts = row.Split(' ');
        
        var numberOfCrates = Convert.ToInt32(parts[1]);
        // adjust for 0 based indexing
        var fromStack = Convert.ToInt32(parts[3]) - 1;
        var toStack = Convert.ToInt32(parts[5]) - 1;

        return new int[3] { numberOfCrates, fromStack, toStack };
    }

    private void MoveCrate(Stack<char> from, Stack<char> to)
    {
        var crate = from.Pop();
        to.Push(crate);
    }

    private Stack<char> ReverseStack(Stack<char> stack)
    {
        Stack<char> reversed = new Stack<char>(20);
        
        while (stack.Count != 0)
        {
            reversed.Push(stack.Pop());
        }
        return reversed;
    }

    // This method is destructive
    private void PrintStacks()
    {
        for (int i = 0; i < 9; i++)
        {
            Console.WriteLine($"Stacks[{i}].Count: {Stacks[i].Count}");
            PrintStack(index: i, stack: Stacks[i]);
        }
    }

    // This method is destructive
    private void PrintStack(int index, Stack<char> stack)
    {
        for (int j = 0; j < 8; j++)
        {
            if (stack.Count == 0) continue;
            var crate = stack.Pop();
            Console.WriteLine($"Stack[{index}]: {crate}");
        }
    }

    private void PrintStacksTops()
    {
        for (int i = 0; i < 9; i++)
        {
            Console.WriteLine($"Stacks[{i}]: {Stacks[i].Peek()}");
        }
    }
}

