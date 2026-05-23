using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//(7 баллов) Напишите PropertyLogger. Метод GetLog(object obj) должен через рефлексию вернуть список строк (НЕ ВЫВЕСТИ В КОНСОЛЬ) "[ИмяСвойства]: [Значение]" для всех свойств, у которых значение не null.

//(7 баллов) Напишите ConcurrentSystem: один поток-продюсер генерирует объекты User и записывает их в очередь, а второй поток-консьюмер одновременно с ним считывает их из очереди и записывает в List.

//(1 балл) Модифицируйте код: консьюмер должен не просто записывать объект, а каждый раз вызывать PropertyLogger.GetLog() для извлечения состояния объекта перед записью в список (сохранять значения не надо).

class Program
{
    static void Main(string[] args)
    {
        //пример
        var testUser = new User
        {
            Id = 52,
            Name = "bob",
            Email = null,
            Age = 30,
            CreatedAt = new DateTime(2026, 1, 15)
        };

        var logs = PropertyLogger.GetLog(testUser);
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }

        var system = new ConcurrentSystem();

        system.Start(producerCount: 1, consumerCount: 2, itemsToProduce: 20);

        Console.WriteLine($"\nlog entries: {system.GetProcessedLogs().Count}");

        var allLogs = system.GetProcessedLogs();
        for (int i = 0; i < Math.Min(10, allLogs.Count); i++)
        {
            Console.WriteLine(allLogs[i]);
        }
    }
}