using NUnit.Framework.Interfaces;

using aoc_2022_cli.Puzzles;

namespace PuzzleTests;

public class Day3Tests
{
    [Test]
    [TestCase('a', 1)]
    [TestCase('b', 2)]
    [TestCase('c', 3)]
    [TestCase('j', 10)]
    [TestCase('o', 15)]
    [TestCase('t', 20)]
    [TestCase('y', 25)]
    [TestCase('z', 26)]
    [TestCase('A', 27)]
    [TestCase('B', 28)]
    [TestCase('C', 29)]
    [TestCase('J', 36)]
    [TestCase('O', 41)]
    [TestCase('T', 46)]
    [TestCase('Y', 51)]
    [TestCase('Z', 52)]
    public void GetItemPriority_ReturnsCorrectPriority(char c, int priority)
    {
        // Arrange / Act
        var dec03 = new Dec03(date: "03-12-2022", runningTests: true);

        var computedPriority = dec03.GetItemPriority(c);

        Assert.That(computedPriority, Is.EqualTo(priority));

    }
}

