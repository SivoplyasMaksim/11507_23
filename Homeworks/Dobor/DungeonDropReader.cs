using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dobor
{
    public class DungeonDropReader
    {
        public IEnumerable<Item> ReadDrops(string filePath)
        {
            if (!File.Exists(filePath))
            {
                GenerateFile(filePath);
            }

            using var reader = new StreamReader(filePath);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(';');
                if (parts.Length == 3 && int.TryParse(parts[1], out int power))
                {
                    yield return new Item(parts[0], power, parts[2]);
                }
            }
        }

        private void GenerateFile(string filePath)
        {
            using var writer = new StreamWriter(filePath);
            var rng = new Random(42);
            string[] names = { "Меч", "Щит", "Посох", "Лук", "Кинжал", "Шлем", "Сапоги", "Кольцо", "Амулет", "Плащ" };

            for (int i = 0; i < 300; i++)
            {
                string name = $"{names[rng.Next(names.Length)]}_{i}";
                int power = rng.Next(10, 200);
                string rarity = (i == 0 || i == 15) ? "Legendary" : (i % 5 == 0 ? "Epic" : "Common");
                writer.WriteLine($"{name};{power};{rarity}");
            }
        }
    }
}
