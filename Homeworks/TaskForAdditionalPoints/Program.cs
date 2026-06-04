using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskForAdditionalPoints
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var processor = new OrderProcessor();

            var logger = new Logger();
            logger.Subscribe(processor);

            var orders = new List<Order>
            {
                new Order("user1@example.com", 5000, "Оплачен"),         
                new Order("student@kpfu.ru", 15000, "Оплачен"),   
                new Order("stupiduser.com", 1000, "Оплачен"),             
                new Order("user2@example.com", 2000, "Ожидает"),         
                new Order("user3@example.com", 3000, "Оплачен"),         
                new Order("user4@example.com", 0, "Оплачен"),        
                new Order("user5@example.com", 5000, "Оплачен"),         
                new Order("user6@example.com", 6000, "Оплачен")         
            };

            await processor.ProcessBatchAsync(orders);

            Console.WriteLine(".");
            Console.ReadKey();
        }
    }
}