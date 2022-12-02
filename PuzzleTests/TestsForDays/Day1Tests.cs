using NUnit.Framework.Interfaces;

using aoc_2022_cli.Puzzles;

namespace PuzzleTests;

public class Puzzle1Tests
{
    [Test]
    [TestCase(235)]
    public void TotalElfCount_ReturnsCorrectNumberOfElves(int elfCount)
    {
        // Arrange / Act
        var dec01 = new Dec01(runningTests: true);

        Assert.That(dec01.Elves.Count, Is.EqualTo(elfCount));

    }

    [Test]
    public void Elf_0_CalculatedCalories_Equals_ManualCalorieCalculation()
    {
        var dec01 = new Dec01(runningTests: true);
        var firstElf = dec01.Elves[0];
        // from provided data
        var manualSum = 7896 + 4992 + 1382 + 2920 + 7533 + 2709 + 6020 + 5321 + 2698 + 6806 + 8008;

        Assert.That(manualSum, Is.EqualTo(firstElf.TotalCalories));
    }

    [Test]
    [TestCase(68787)]
    public void TopElf_HasXCalories(int cals)
    {
        var dec01 = new Dec01(runningTests: true);
        var max = dec01.GetMaxCalElf();

        Assert.That(max.TotalCalories, Is.EqualTo(cals));
    }

    [Test]
    [TestCase(11)]
    public void TopElf_HasXItems(int items)
    {
        var dec01 = new Dec01(runningTests: true);
        var max = dec01.GetMaxCalElf();

        Assert.That(max.Items.Count, Is.EqualTo(items));
    }

    [Test]
    [TestCase(198041)]
    public void Top3Elves_HasXCalories(int cals)
    {
        var dec01 = new Dec01(runningTests: true);
        var orderedElves = dec01.OrderElves();

        var totalCals = 0;

        for (int i = 0; i < 3; i++)
        {
            totalCals += orderedElves[i].TotalCalories;
        }

        Assert.That(totalCals, Is.EqualTo(cals));
    }

    [Test]
    [TestCase(33)]
    public void Top3Elves_HasXItems(int items)
    {
        var dec01 = new Dec01(runningTests: true);
        var orderedElves = dec01.OrderElves();

        var totalItems = 0;

        for (int i = 0; i < 3; i++)
        {
            totalItems += orderedElves[i].Items.Count;
        }
        
        Assert.That(totalItems, Is.EqualTo(items));
    }

}
