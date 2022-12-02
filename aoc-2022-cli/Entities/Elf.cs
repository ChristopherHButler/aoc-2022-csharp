using System;
namespace aoc_2022_cli.Entities
{
    public class Elf
    {
        public string Name { get; set; }
        public List<int> Items { get; set; } = new List<int>();
        public int TotalCalories { get; set; } = 0;

        public Elf(string name)
        {
            Name = name;
        }

        public void ConsoleElfInfo()
        {
            Console.WriteLine("");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Item Count: {Items.Count}");
            Console.WriteLine($"Item List:");
            for (var i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"Item: {i}: {Items[i]}");
            }
            Console.WriteLine($"Total Calories: {TotalCalories}");
            Console.WriteLine("---------------------");
            Console.WriteLine("");
        }
    }
}
