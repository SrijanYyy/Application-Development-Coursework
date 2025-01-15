using System.Text.Json;
using finance.Models;

namespace finance.Services
{
    public class TransactionService
    {
        private const string FilePath = "data/transactions.json";

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            if (!File.Exists(FilePath))
                return new List<Transaction>();

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        
        public async Task SaveTransactionsAsync(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            var transactions = await GetTransactionsAsync();
            transaction.Id = transactions.Any() ? transactions.Max(t => t.Id) + 1 : 1;
            transactions.Add(transaction);
            await SaveTransactionsAsync(transactions);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transactions = await GetTransactionsAsync();
            transactions.RemoveAll(t => t.Id == id);
            await SaveTransactionsAsync(transactions);
        }

        public async Task UpdateTransactionAsync(Transaction updatedTransaction)
        {
            var transactions = await GetTransactionsAsync();
            var existingTransaction = transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);

            if (existingTransaction != null)
            {
                transactions.Remove(existingTransaction);
                transactions.Add(updatedTransaction);
                await SaveTransactionsAsync(transactions);
            }
        }
    }
}
