using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskForAdditionalPoints
{
    public class OrderProcessor
    {
        public event EventHandler<OrderStageEventArgs> OnOrderStateChanged;

        public void ValidateOrder(Order order)
        {
            var properties = typeof(Order).GetProperties();
            foreach (var prop in properties)
            {
                if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
                {
                    var value = prop.GetValue(order);
                    bool isInvalid = value == null ||
                                     (value is string str && string.IsNullOrWhiteSpace(str)) ||
                                     (value is decimal dec && dec <= 0);

                    if (isInvalid)
                    {
                        throw new ValidationException($"Ошибка валидации: Обязательное поле '{prop.Name}' отсутствует или некорректно в заказе {order.Id}.");
                    }
                }
            }
            RaiseEvent(order.Id, "Валидация данных");
        }

        public void ApplyDynamicDiscounts(Order order)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var discountTypes = assembly.GetTypes()
                .Where(t => typeof(IDiscountRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);


            foreach (var type in discountTypes)
            {
                var rule = (IDiscountRule)Activator.CreateInstance(type);
                rule.ApplyDiscount(order);
            }
            RaiseEvent(order.Id, "Применение скидок");
        }
        public async Task SendToDeliveryAsync(Order order)
        {
            if (order.Status != "Оплачен")
            {
                throw new InvalidOperationException($"Заказ {order.Id} не был отправлен: статус '{order.Status}' (требуется 'Оплачен')");
            }
            // сетевой запрос
            await Task.Delay(500);

            // 1% ошибки внешнего сервиса
            if (Random.Shared.Next(1, 101) == 1)
            {
                throw new Exception($"Сбой внешнего API доставки для заказа {order.Id} (500 Internal Server Error)");
            }

            RaiseEvent(order.Id, "Отправка в доставку");
        }
        private void RaiseEvent(string orderId, string stageName)
        {
            OnOrderStateChanged?.Invoke(this, new OrderStageEventArgs(orderId, stageName));
        }
        public async Task ProcessSingleOrderAsync(Order order)
        {
            try
            {
                Console.WriteLine($">> Обрабатываю заказ {order.Id}");
                ValidateOrder(order);
                ApplyDynamicDiscounts(order);
                await SendToDeliveryAsync(order);
                Console.WriteLine($">> Заказ {order.Id} обработан \n Итоговая сумма: {order.FinalAmount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($">> [Err] Заказ {order.Id} отклонен {ex.Message}");
            }
        }
        public async Task ProcessBatchAsync(List<Order> orders)
        {
            Console.WriteLine($"Обработка пакета из {orders.Count} заказов");
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5 
            };
            await Parallel.ForEachAsync(orders, parallelOptions, async (order, cancellationToken) =>
            {
                await ProcessSingleOrderAsync(order);
            });
            Console.WriteLine("готово");
        }
    }
}