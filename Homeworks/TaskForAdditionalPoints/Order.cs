using System.ComponentModel.DataAnnotations;

namespace TaskForAdditionalPoints
{
    // ==========================================
    // МОДЕЛИ ДАННЫХ
    // ==========================================
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString()[..8];

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } // "Оплачен", "Ожидает"
        public decimal FinalAmount { get; set; }

        public Order(string email, decimal amount, string status)
        {
            UserEmail = email;
            TotalAmount = amount;
            Status = status;
            FinalAmount = amount;
        }
    }
}