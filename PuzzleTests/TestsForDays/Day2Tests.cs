using NUnit.Framework.Interfaces;

using aoc_2022_cli.Puzzles;

namespace PuzzleTests;

public class Day2Tests
{
    [Test]
    [TestCase(2500)]
    public void TotalRoundsCount(int roundsCount)
    {
        // Arrange
        var dec02 = new Dec02(date: "02-12-2022", runningTests: true);

        // Act
        dec02.Solve(runningTests: true, debugMode: false);

        // Assert        
        Assert.That(dec02.MePart1.Rounds.Count, Is.EqualTo(roundsCount));
    }
}

