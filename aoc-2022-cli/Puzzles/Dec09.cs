using System;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aoc_2022_cli.Puzzles;

public class Dec09
{
    private class Move
    {
        public char Direction { get; set; }
        public int Steps { get; set; }

        public Move(char direction, int steps)
        {
            Direction = direction;
            Steps = steps;
        }
    }

    private class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private string Date { get; set; }
    private List<string> Moves = new List<string>();

    // part 1

    private List<Coordinate> HeadCoordinates { get; set; } = new List<Coordinate>();
    private List<Coordinate> TailCoordinates { get; set; } = new List<Coordinate>();

    private Coordinate CurrentHeadPosition = new Coordinate(x: 0, y: 0);
    private Coordinate CurrentTailPosition = new Coordinate(x: 0, y: 0);

    // part 2
    public int KnotCount { get; set; } = 10;

    private record Point(int X, int Y);
    private readonly List<Point> Knots = new();
    private readonly HashSet<Point> TailPositions = new();

    public int TailCount => TailPositions.Count;
    private Point Head => Knots[0];
    private Point Tail => Knots[^1];    
    
    public int HeadCount = 0;

    public Dec09(string date = "09-12-2022")
    {
        Date = date;

        ParseRopeMoves(date);


        // RunPart1();
        RunPart2();
    }

    #region setup

    private void ParseRopeMoves(string? date)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: false, debugMode: false);
        dfr.ReadFile(debugMode: false);

        Moves.Clear();

        Moves.AddRange(dfr.Lines);
    }

    #endregion

    #region Part 1

    private void RunPart1()
    {
        // 5907
        ParseRopeMoves(Date);

        GenerateCoordinates(moves: Moves);
    }

    private void GenerateCoordinates(List<string> moves)
    {
        // start at 0,0...
        HeadCoordinates.Clear();
        TailCoordinates.Clear();

        CurrentHeadPosition = new Coordinate(x: 0, y: 0);
        CurrentTailPosition = new Coordinate(x: 0, y: 0);

        HeadCoordinates.Add(CurrentHeadPosition);
        TailCoordinates.Add(CurrentTailPosition);

        // process all moves
        for (var i = 0; i < moves.Count; i++)
        {
            TrackHeadSteps(currentMove: ConvertMove(moves[i]));
        }

        Console.WriteLine($"Head made {HeadCoordinates.Count} moves");
        Console.WriteLine($"Generated {TailCoordinates.Count} Coordniates");
    }

    private void TrackHeadSteps(Move currentMove)
    {
        var steps = currentMove.Steps;

        // for each head step in the current move
        for (int i = 0; i < steps; i++)
        {
            // create head coordinate for next step
            var nextHeadStep = ComputeCellStep(currentCell: CurrentHeadPosition, direction: currentMove.Direction);
            // the head WILL move, so add it to the list now.
            HeadCoordinates.Add(nextHeadStep);

            // check if the tail needs to move
            var moveTail = TailMustMove(nextHeadStep: nextHeadStep, currentTailPosition: CurrentTailPosition);

            // if the tail needs to move
            if (moveTail)
            {
                // create a coordinate for the next tail move
                var nextTailStep = ComputeTailMove(nextHeadStep);

                // check if it exists in list
                if (!TailCoordinates.Any(c => c.X == nextTailStep.X && c.Y == nextTailStep.Y))
                {
                    // if not add it
                    TailCoordinates.Add(nextTailStep);
                }
                // update the current tail position
                CurrentTailPosition.X = nextTailStep.X;
                CurrentTailPosition.Y = nextTailStep.Y;
            }

            // update the head step
            CurrentHeadPosition.X = nextHeadStep.X;
            CurrentHeadPosition.Y = nextHeadStep.Y;
        }
    }

    private Coordinate ComputeTailMove(Coordinate nextHeadStep)
    {
        int newX = 0;
        int newY = 0;

        // [x-2, y-2] [x-2, y-1] [x-2, y] [x-2, y+1] [x-2, y+2]
        // [x-1, y-2] [x-1, y-1] [x-1, y] [x-1, y+1] [x-1, y+2]
        // [x,   y-2] [x,   y-1] [0,   0] [x,   y+1] [x,   y+2]
        // [x+1, y-2] [x+1, y-1] [x+1, y] [x+1, y+1] [x+1, y+2]
        // [x+2, y-2] [x+2, y-1] [x+2, y] [x+2, y+1] [x+2, y+2]

        // assume tail is at 0,0
        // need to find the difference between x & y to see the position of the next head step
        // a move is only required if the difference is greater than 1 so theoretically we will never come in here
        // unless the difference is greater than 1 or exactly 2 because we check every step.
        // that means we only need to check steps + 2 away

        // if the next head step is two above
        if ((CurrentTailPosition.X == (nextHeadStep.X + 2)) && (CurrentTailPosition.Y == nextHeadStep.Y))
        {
            // move the tail up one vertically [x-1, y]
            newX = CurrentTailPosition.X - 1;
            newY = CurrentTailPosition.Y;
        }
        // if the next head step is two below
        else if ((CurrentTailPosition.X == (nextHeadStep.X - 2)) && (CurrentTailPosition.Y == nextHeadStep.Y))
        {
            // move the tail down one vertically [x+1, y]
            newX = CurrentTailPosition.X + 1;
            newY = CurrentTailPosition.Y;
        }
        // if the next head step is two left
        else if ((CurrentTailPosition.X == nextHeadStep.X) && (CurrentTailPosition.Y == ((nextHeadStep.Y + 2))))
        {
            // move the tail one left [x,   y-1]
            newX = CurrentTailPosition.X;
            newY = CurrentTailPosition.Y - 1;
        }
        // if the next head step is two right
        else if ((CurrentTailPosition.X == nextHeadStep.X) && (CurrentTailPosition.Y == ((nextHeadStep.Y - 2))))
        {
            // move the tail one right [x,   y+1]
            newX = CurrentTailPosition.X;
            newY = CurrentTailPosition.Y + 1;
        }
        // if the next head step is the upper left corner ([x-2, y-1] || [x-2, y-2] || [x-1, y-2]
        else if (
            ((CurrentTailPosition.X == (nextHeadStep.X + 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 1))) || // [x-2, y-1]
            ((CurrentTailPosition.X == (nextHeadStep.X + 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 2))) || // [x-2, y-2]
            ((CurrentTailPosition.X == (nextHeadStep.X + 1)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 2))))   // [x-1, y-2]
        {
            // move the tail one up and to the left [x-1, y-1]
            newX = CurrentTailPosition.X - 1;
            newY = CurrentTailPosition.Y - 1;
        }
        // if the next head step is the upper right corner ([x-2, y+1] || [x-2, y+2] || [x-1, y+2]
        else if (
            ((CurrentTailPosition.X == (nextHeadStep.X + 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 1))) || // [x-2, y+1]
            ((CurrentTailPosition.X == (nextHeadStep.X + 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 2))) || // [x-2, y+2]
            ((CurrentTailPosition.X == (nextHeadStep.X + 1)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 2))))   // [x-1, y+2]
        {
            // move the tail one up and to the right [x-1, y+1]
            newX = CurrentTailPosition.X - 1;
            newY = CurrentTailPosition.Y + 1;
        }
        // if the next head step is the bottom left corner ([x+1, y-2] || [x+2, y-2] || [x+2, y-1]
        else if (
            ((CurrentTailPosition.X == (nextHeadStep.X - 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 1))) || // [x+2, y-1]
            ((CurrentTailPosition.X == (nextHeadStep.X - 1)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 2))) || // [x+1, y-2]
            ((CurrentTailPosition.X == (nextHeadStep.X - 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y + 2))))    // [x+2, y-2]
        {
            // move the tail one down and to the left [x+1, y-1]
            newX = CurrentTailPosition.X + 1;
            newY = CurrentTailPosition.Y - 1;
        }
        // if the next head step is the bottom right corner ([x-2, y+1] || [x-2, y+2] || [x-1, y+2]
        else if (
            ((CurrentTailPosition.X == (nextHeadStep.X - 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 1))) || // [x+2, y+1]
            ((CurrentTailPosition.X == (nextHeadStep.X - 2)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 2))) || // [x+2, y+2]
            ((CurrentTailPosition.X == (nextHeadStep.X - 1)) && (CurrentTailPosition.Y == (nextHeadStep.Y - 2))))   // [x+1, y+2]
        {
            // move the tail one down and to the right [x+1, y+1]
            newX = CurrentTailPosition.X + 1;
            newY = CurrentTailPosition.Y + 1;
        }

        return new Coordinate(x: newX, y: newY);
    }

    private Coordinate ComputeCellStep(Coordinate currentCell, char direction)
    {
        int newX = 0;
        int newY = 0;

        // [x-2, y-2] [x-2, y-1] [x-2, y] [x-2, y+1] [x-2, y+2]
        // [x-1, y-2] [x-1, y-1] [x-1, y] [x-1, y+1] [x-1, y+2]
        // [x,   y-2] [x,   y-1] [0,   0] [x,   y+1] [x,   y+2]
        // [x+1, y-2] [x+1, y-1] [x+1, y] [x+1, y+1] [x+1, y+2]
        // [x+2, y-2] [x+2, y-1] [x+2, y] [x+2, y+1] [x+2, y+2]

        switch (direction)
        {
            case 'U':
                newX = currentCell.X - 1;
                newY = currentCell.Y;
                break;
            case 'D':
                newX = currentCell.X + 1;
                newY = currentCell.Y;
                break;
            case 'R':
                newX = currentCell.X;
                newY = currentCell.Y + 1;
                break;
            case 'L':
                newX = currentCell.X;
                newY = currentCell.Y - 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new Coordinate(x: newX, y: newY);
    }

    private bool TailMustMove(Coordinate nextHeadStep, Coordinate currentTailPosition)
    {
        // calculate distance between the next head step and the current tail position
        var nextStepDistance = ComputeDistance(head: nextHeadStep, tail: currentTailPosition);

        if (Math.Abs(nextStepDistance.X) > 1 || Math.Abs(nextStepDistance.Y) > 1)
        {
            return true;
        }
        return false;
    }

    private Coordinate ComputeDistance(Coordinate head, Coordinate tail)
    {
        var dx = head.X - tail.X;
        var dy = head.Y - tail.Y;
        return new Coordinate(x: dx, y: dy);
    }

    #endregion

    #region Part 2

    private void RunPart2()
    {
        // 2303
        ParseRopeMoves(Date);

        InitialiseKnots();

        ProcessAllMovesPart2();
    }

    private void InitialiseKnots()
    {
        for (int i = 0; i < KnotCount; i++)
        {
            Knots.Add(new Point(0, 0));
        }
    }

    private void ProcessAllMovesPart2()
    {
        for (var i = 0; i < Moves.Count; i++)
        {
            var move = ConvertMove(Moves[i]);
            MoveHead(currentMove: move);
        }

        // Console.WriteLine($"Head count: {HeadCount}");
        Console.WriteLine($"The total positions is: {TailCount}");
    }

    private void MoveHead(Move currentMove)
    {
        var steps = currentMove.Steps;

        for (int i = 0; i < steps; i++)
        {
            Knots[0] = ComputeHeadMove(direction: currentMove.Direction);
            // HeadCount++;

            for (int j = 0; j < Knots.Count - 1; j++)
            {
                MoveKnot(j, j + 1);
            }
            TailPositions.Add(Tail);            
        }
    }

    private Point ComputeHeadMove(char direction)
    {
        return direction switch
        {
            'U' => new(Head.X, Head.Y + 1),
            'D' => new(Head.X, Head.Y - 1),
            'L' => new(Head.X - 1, Head.Y),
            'R' => new(Head.X + 1, Head.Y),
            _ => throw new InvalidOperationException()
        } ;
    }

    private void MoveKnot(int currentHead, int currentTail)
    {
        var head = Knots[currentHead];
        var tail = Knots[currentTail];

        var dx = head.X - tail.X;
        var dy = head.Y - tail.Y;

        if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
        {
            tail = (Math.Abs(dx), Math.Abs(dy)) switch
            {
                (2, 2) => new(head.X - Math.Sign(dx), head.Y - Math.Sign(dy)),
                (2, _) => new(head.X - Math.Sign(dx), head.Y),
                (_, 2) => new(head.X, head.Y - Math.Sign(dy)),
                _ => throw new InvalidOperationException()
            };
        }

        Knots[currentTail] = tail;
    }

    #endregion

    #region common

    private Move ConvertMove(string move)
    {
        var parts = move.Split(" ");
        return new Move(direction: ((char)parts[0][0]), steps: Convert.ToInt32(parts[1]));
    }

    #endregion
}

