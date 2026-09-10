using System;
using System.Collections.Generic;
using System.Text;

namespace Dobor
{
    public class GuildCommands
    {
        private readonly GuildChest _chest;

        public GuildCommands(GuildChest chest)
        {
            _chest = chest;
        }

        [GameCommand("raid", "Запустить рейд 3 героев")]
        public void Raid()
        {
            Console.WriteLine("   > Рейд успешно завершен! Герои вернулись с лутом.");
        }

        [GameCommand("chest", "Показать содержимое сундука")]
        public void ShowChest()
        {
            Console.WriteLine($"   > В сундуке находится {_chest.TotalCount} предметов.");
        }
    }
}
