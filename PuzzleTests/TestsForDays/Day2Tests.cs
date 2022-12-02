using System;
using aoc_2022_cli.Puzzles;

namespace PuzzleTests;

public class Day2Tests
{
    [Test]
    [TestCase(2500)]
    public void TotalRoundsCount(int roundsCount)
    {
        // Arrange
        var dec02 = new Dec02(runningTests: true);

        // Act

        // Assert        

    }


    //[Test]
    //[TestCase(235)]
    //public void TotalElfCount_ReturnsCorrectNumberOfElves(int elfCount)
    //{
    //    // Arrange / Act
    //    var dec01 = new Dec01(runningTests: true);

    //    Assert.That(dec01.Elves.Count, Is.EqualTo(elfCount));

    //}
}

