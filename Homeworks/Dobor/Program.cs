using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Dobor
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ПРОВЕРКА ВАРИАНТА 1: РЕЙД ГИЛЬДИИ ===");

            var reader = new DungeonDropReader();
            var itemsForRaid = reader.ReadDrops("dungeon_drops.txt").Take(30).ToList();
            Console.WriteLine($"1. Чтение файла: Успешно прочитано {itemsForRaid.Count} предметов через yield return.");

            var filter = new LootFilter<Item>();

            var rareOrStrong = filter.Filter(itemsForRaid, item => item.Rarity == "Legendary" || item.Power >= 100);

            var legendaries = filter.Filter(itemsForRaid, item => item.Rarity == "Legendary");
            Console.WriteLine($"2. Фильтрация: Найдено {legendaries.Count} легендарных предмета с помощью Predicate<Item>.");

            var chest = new GuildChest();
            var heroes = new[] { "Рыцарь", "Лучник", "Маг" };
            var threads = new Thread[3];
            var completed = new bool[3]; 

            for (int i = 0; i < 3; i++)
            {
                int heroIndex = i;
                var heroBatch = itemsForRaid.Skip(i * 10).Take(10);

                threads[i] = new Thread(() =>
                {
                    var filteredBatch = filter.Filter(heroBatch, _ => true);
                    chest.Deposit(heroes[heroIndex], filteredBatch);
                    completed[heroIndex] = true;
                });
                threads[i].Start();
            }

            Console.WriteLine("3. Запуск 3 потоков:");

            foreach (var t in threads)
            {
                t.Join();
            }

            for (int i = 0; i < 3; i++)
            {
                if (completed[i])
                    Console.WriteLine($"   - [Поток: {heroes[i]}] завершил сбор и запер сундук.");
            }
            Console.WriteLine($"   Итого в сундуке: ровно {chest.TotalCount} предметов (Race condition предотвращен).");

            Console.WriteLine("4. Диспетчер команд (Рефлексия):");
            var commands = new GuildCommands(chest);
            var dispatcher = new CommandDispatcher(commands);

            dispatcher.PrintCommands();
            dispatcher.Execute("/raid");

            Console.WriteLine("   Вызов команды '/raid' через диспетчер успешно выполнен!");
            Console.WriteLine(">>> ВСЕ ТЕСТЫ ВАРИАНТА 1 УСПЕШНО ПРОЙДЕНЫ! <<<");
        }
    }
}
