namespace finance.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string DebtSource { get; set; } // Source of the debt
        public DateTime DueDate { get; set; } // When the debt is due
        public int? TagId { get; set; } // Associated tag
        public string Status { get; set; } // Status of the debt
        public string Notes { get; set; }
    }
}
