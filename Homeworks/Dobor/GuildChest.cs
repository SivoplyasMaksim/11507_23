using System;
using System.Collections.Generic;
using System.Text;

namespace Dobor
{
    public class GuildChest
    {
        private readonly List<Item> _items = new();
        private readonly object _lock = new();

        public void Deposit(string heroName, IEnumerable<Item> items)
        {
            lock (_lock)
            {
                _items.AddRange(items);
            }
        }

        public int TotalCount
        {
            get
            {
                lock (_lock)
                {
                    return _items.Count;
                }
            }
        }
    }
}
