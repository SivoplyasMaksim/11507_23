namespace TaskForAdditionalPoints
{
    public class Logger
    {
        public void Subscribe(OrderProcessor processor)
        {
            processor.OnOrderStateChanged += HandleOrderStageCompleted;
        }

        private void HandleOrderStageCompleted(object sender, OrderStageEventArgs e)
        {
            Console.WriteLine($"[LOG] Успешно: Заказ {e.OrderId}  Этап: {e.StageName}  Время: {DateTime.Now:HH:mm:ss}");
        }
    }
}