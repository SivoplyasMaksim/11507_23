namespace TaskForAdditionalPoints
{
    public class LargeOrderDiscount : IDiscountRule
    {
        public void ApplyDiscount(Order order)
        {
            if (order.TotalAmount > 10000)
            {
                order.FinalAmount *= 0.9m; 
                Console.WriteLine($"Применена скидка за объем для заказа {order.Id}");
            }
        }
    }
}