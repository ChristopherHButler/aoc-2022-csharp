using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aoc_2022_cli.Puzzles;

public class Dec08
{

    public int[,] TreeGrid;

    public int PerimeterTrees = 0;

    public Dec08(string? date)
    {
        InitGridFromInput(date);
        // [m,n]
        // [0,0][0,1][0,2][0,3]
        // [1,0][1,1][1,2][1,3]
        // [2,0][2,1][2,2][2,3]
        // [3,0][3,1][3,2][3,3]

        RunPart1();
        RunPart2();

    }

    private void InitGridFromInput(string? date)
    {
        var dfr = new DataFileReader(filename: "", date: date, runningTests: false, debugMode: false);
        dfr.ReadFile(debugMode: false);

        var lines = dfr.Lines;

        var numRows = lines.Count;
        var numCols = dfr.Lines[0].ToCharArray().Count();

        ComputePerimeterTrees(numRows, numCols);

        TreeGrid = new int[numRows, numCols];

        // for each row
        for (var m = 0; m < lines.Count; m++)
        {
            var row = lines[m].ToCharArray(); // a row has n columns
            // for each column in a row
            for (var n = 0; n < row.Length; n++)
            {
                TreeGrid[m, n] = Convert.ToInt32($"{row[n]}");
            }
        }
    }

    #region Part 1

    // 1546
    private void RunPart1()
    {
        var interiorVisible = ScanAllTrees();

        var numVisible = interiorVisible + PerimeterTrees;

        Console.WriteLine($"The total number of trees visible are: {numVisible}");

        PrintGrid();
    }

    private void ComputePerimeterTrees(int numRows, int numCols)
    {
        PerimeterTrees = (numRows * 2) + (numCols * 2) - 4; // -4 because of duplicates
    }

    private int ScanAllTrees()
    {
        var numVisible = 0;

        for (int m = 1; m < GetGridNumRows(); m++)
        {
            for (int n = 1; n < GetGridNumCols(); n++)
            {
                // skip perimeter trees
                if (m == 0 || n == 0 || m == (GetGridNumRows() - 1) || n == (GetGridNumCols() - 1))
                    continue;

                var isTreeVisible = ScanTreeCrossHair(targetRowIndex: m, targetColumnIndex: n);

                if (isTreeVisible)
                {
                    numVisible++;
                }    
            }
        }
        return numVisible;
    }

    private bool ScanTreeCrossHair(int targetRowIndex, int targetColumnIndex)
    {
        // when m = 0 and n = anything (top row)
        // when n = 0 and m = anything (left side)
        // when m = lines.Count and n = anything (bottom row)
        // when n = dfr.Lines[0].ToCharArray().Count() and m = anything (right side)

        var colScanResults = ScanColumn(targetRowIndex, targetColumnIndex);

        var isVisibleAbove = colScanResults[0];
        var isVisibleBelow = colScanResults[1];

        var rowScanResults = ScanRow(targetRowIndex, targetColumnIndex);

        var isVisibleLeft = rowScanResults[0];
        var isVisibleRight = rowScanResults[1];

        // var isVisible = (isVisibleAbove == true) && (isVisibleBelow == true) && (isVisibleLeft == true) && (isVisibleRight == true);
        var isVisible = (isVisibleAbove == true) || (isVisibleBelow == true) || (isVisibleLeft == true) || (isVisibleRight == true);

        return isVisible;
    }

    private bool[] ScanColumn(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);
        // this is the number of elements in a full column
        // which is actually the number of rows (confusing)
        var columnHeight = GetGridNumRows();

        var isVisibleAbove = true;
        var isVisibleBelow = true;

        for (int currentRowIndex = 0; currentRowIndex < columnHeight; currentRowIndex++)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(currentRowIndex, targetColumnIndex);

            // if a tree is higher than the target, it's not visible
            if (currentTreeHeight >= targetTreeHeight)
            {
                if (currentRowIndex < targetRowIndex)
                {
                    isVisibleAbove = false;
                }
                else if (currentRowIndex > targetRowIndex)
                {
                    isVisibleBelow = false;
                }
            }
        }

        return new bool[] { isVisibleAbove, isVisibleBelow };
    }

    private bool[] ScanRow(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);
        // this is the number of elements in a full row
        // which is actually the number of columns (confusing)
        var rowLength = GetGridNumCols();

        var isVisibleLeft = true;
        var isVisibleRight = true;

        for (int currentColumnIndex = 0; currentColumnIndex < rowLength; currentColumnIndex++)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(targetRowIndex, currentColumnIndex);

            // if a tree is higher than the target, it's not visible
            if (currentTreeHeight >= targetTreeHeight)
            {
                if (currentColumnIndex < targetColumnIndex)
                {
                    isVisibleLeft = false;
                }
                else if (currentColumnIndex > targetColumnIndex)
                {
                    isVisibleRight = false;
                }
            }
        }
        return new bool[] { isVisibleLeft, isVisibleRight };
    }

    #endregion

    #region Part 2

    // 519064

    private void RunPart2()
    {
        var maxScenicScore = ScanAllTreesPart2();

        Console.WriteLine("");
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("");
        Console.WriteLine($"Final Max Score: {maxScenicScore}");
    }

    private int ScanAllTreesPart2()
    {
        var maxScore = 1;

        for (int m = 0; m < GetGridNumRows(); m++)
        {
            for (int n = 0; n < GetGridNumCols(); n++)
            {
                // skip perimeter trees - scenic score is zero
                //if (m == 0 || n == 0 || m == (GetGridNumRows() - 1) || n == (GetGridNumCols() - 1))
                //    continue;

                var currentScore = GetTreeScenicScore(targetRowIndex: m, targetColumnIndex: n);

                if (currentScore > maxScore)
                {
                    maxScore = currentScore;
                    
                    Console.WriteLine($"Tree: [ {m}, {n} ] - New Max Score: {maxScore}");
                }
            }
        }

        return maxScore;
    }

    private int GetTreeScenicScore(int targetRowIndex, int targetColumnIndex)
    {
        var topViewingDistance = GetTopViewingDistance(targetRowIndex, targetColumnIndex);
        var bottomViewingDistance = GetBottomViewingDistance(targetRowIndex, targetColumnIndex);
        var leftViewingDistance = GetLeftViewingDistance(targetRowIndex, targetColumnIndex);
        var rightViewingDistance = GetRightViewingDistance(targetRowIndex, targetColumnIndex);

        var scenicScore = topViewingDistance * bottomViewingDistance * leftViewingDistance * rightViewingDistance;

        return scenicScore;
    }

    private int GetTopViewingDistance(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);
        int topScore = 0;

        // we need to work from the target out (moving up... which decreases the index)
        // - 1 because you don't want to compare the tree to itself
        for (int currentRowIndex = targetRowIndex - 1; currentRowIndex >= 0; currentRowIndex--)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(currentRowIndex, targetColumnIndex);
            // this tree will block your view and you need to stop
            if (currentTreeHeight >= targetTreeHeight)
            {
                return ++topScore;
            }
            else if (currentTreeHeight < targetTreeHeight)
            {
                // increase the score and keep moving out (up)
                topScore++;
            }
        }

        return topScore;
    }

    private int GetBottomViewingDistance(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);
        var columnHeight = GetGridNumRows();
        int bottomScore = 0;

        // we need to work from the target out (moving down... which increases the index)
        // + 1 because you don't want to compare the tree to itself
        for (int currentRowIndex = targetRowIndex + 1; currentRowIndex < columnHeight; currentRowIndex++)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(currentRowIndex, targetColumnIndex);
            // this tree will block your view and you need to stop
            if (currentTreeHeight >= targetTreeHeight)
            {
                return ++bottomScore;
            }
            else if (currentTreeHeight < targetTreeHeight)
            {
                // increase the score and keep moving out (up)
                bottomScore++;
            }
        }

        return bottomScore;
    }

    private int GetLeftViewingDistance(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);
        int leftScore = 0;

        // we need to work from the target out (moving left... which decreases the index)
        // - 1 because you don't want to compare the tree to itself
        for (int currentColumnIndex = targetColumnIndex - 1; currentColumnIndex >= 0; currentColumnIndex--)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(targetRowIndex, currentColumnIndex);

            // this tree will block your view and you need to stop
            if (currentTreeHeight >= targetTreeHeight)
            {
                return ++leftScore;
            }
            else if (currentTreeHeight < targetTreeHeight)
            {
                leftScore++;
            }
        }

        return leftScore;
    }

    private int GetRightViewingDistance(int targetRowIndex, int targetColumnIndex)
    {
        int targetTreeHeight = GetTreeHeight(targetRowIndex, targetColumnIndex);

        // this is the distance from the targert to the edge (right)
        var rightPartRowLength = GetGridNumCols() - targetColumnIndex;
        int rightScore = 0;

        // we need to work from the target out (moving right... which increases the index)
        // + 1 because you don't want to compare the tree to itself
        for (int currentColumnIndex = targetColumnIndex + 1; currentColumnIndex < rightPartRowLength; currentColumnIndex++)
        {
            // get the value in the grid
            var currentTreeHeight = GetTreeHeight(targetRowIndex, currentColumnIndex);

            // this tree will block your view and you need to stop
            if (currentTreeHeight >= targetTreeHeight)
            {
                return ++rightScore;
            }
            else if (currentTreeHeight < targetTreeHeight)
            {
                rightScore++;
            }
        }
        return rightScore;
    }

    #endregion

    #region Helpers

    private void PrintGrid()
    {
        Console.WriteLine("Tree Grid:");
        Console.WriteLine("");
        Console.WriteLine("");

        for (int m = 0; m < GetGridNumRows(); m++)
        {
            for (int n = 0; n < GetGridNumCols(); n++)
            {
                Console.Write(TreeGrid[m, n]);
            }
            Console.WriteLine("");
        }
    }

    private int GetTreeHeight(int rowIndex, int columnIndex)
    {
        return TreeGrid[rowIndex, columnIndex];
    }

    private int GetGridNumRows()
    {
        return TreeGrid.GetLength(0);
    }

    private int GetGridNumCols()
    {
        return TreeGrid.GetLength(1);
    }

    #endregion
}

