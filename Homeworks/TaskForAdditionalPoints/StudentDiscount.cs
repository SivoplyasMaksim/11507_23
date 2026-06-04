namespace TaskForAdditionalPoints
{
    public class StudentDiscount : IDiscountRule
    {
        public void ApplyDiscount(Order order)
        {
            if (order.UserEmail.EndsWith("kpfu.ru"))
            {
                order.FinalAmount *= 0.95m; 
                Console.WriteLine($"Применена скидка студента КФУ для заказа {order.Id}");
            }
        }
    }
}