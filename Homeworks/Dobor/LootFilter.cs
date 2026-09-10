using System;
using System.Collections.Generic;
using System.Text;

namespace Dobor
{
    public class LootFilter<T>
    {
        public List<T> Filter(IEnumerable<T> items, Predicate<T> predicate, Action<T>? onMatch = null)
        {
            var result = new List<T>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    result.Add(item);
                    onMatch?.Invoke(item);
                }
            }
            return result;
        }
    }
}
