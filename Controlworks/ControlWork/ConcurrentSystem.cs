using System.Collections.Concurrent;

public class ConcurrentSystem
{
    private readonly BlockingCollection<User> _queue = new BlockingCollection<User>(boundedCapacity: 100);
    private readonly List<string> _processedLogs = new List<string>();
    private readonly object _lock = new object();

    private CancellationTokenSource _cts;

    public void Start(int producerCount, int consumerCount, int itemsToProduce)
    {
        _cts = new CancellationTokenSource();
        var tasks = new List<Task>();

        // Producer 
        for (int i = 0; i < producerCount; i++)
        {
            int producerId = i;
            tasks.Add(Task.Run(() => Producer(producerId, itemsToProduce / producerCount, _cts.Token)));
        }

        //consumer 
        for (int i = 0; i < consumerCount; i++)
        {
            int consumerId = i;
            tasks.Add(Task.Run(() => Consumer(consumerId, _cts.Token)));
        }

        //waiting
        Task.WhenAll(tasks.FindAll(t => t.Id <= producerCount)).ContinueWith(_ => _queue.CompleteAdding());

        Task.WaitAll(tasks.ToArray());
    }

    private void Producer(int id, int itemCount, CancellationToken token)
    {
        for (int i = 0; i < itemCount && !token.IsCancellationRequested; i++)
        {
            var user = new User
            {
                Id = id * 1000 + i,
                Name = $"User_{id}_{i}",
                Email = i % 3 == 0 ? null : $"user{id}_{i}@example.com",
                Age = i % 5 == 0 ? null : 20 + (i % 50), 
                CreatedAt = DateTime.UtcNow
            };

            _queue.Add(user, token);
            Console.WriteLine($"[Producer {id}] Created: {user}");
            Thread.Sleep(10); 
        }
    }

    private void Consumer(int id, CancellationToken token)
    {
        foreach (var user in _queue.GetConsumingEnumerable(token))
        {
            var logEntries = PropertyLogger.GetLog(user);

            lock (_lock)
            {
                _processedLogs.AddRange(logEntries);
            }

            Console.WriteLine($"[Consumer {id}] Processed User#{user.Id}, properties logged: {logEntries.Count}");
        }
    }

    public List<string> GetProcessedLogs()
    {
        lock (_lock)
        {
            return new List<string>(_processedLogs);
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        _queue?.CompleteAdding();
    }
}
