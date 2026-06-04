namespace TaskForAdditionalPoints
{
    public interface IDiscountRule
    {
        void ApplyDiscount(Order order);
    }
}