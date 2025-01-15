namespace finance.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // "Debit" or "Credit"
        public int? TagId { get; set; } // Store the selected tag's ID
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
