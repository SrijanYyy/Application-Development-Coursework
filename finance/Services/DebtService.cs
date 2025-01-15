using System.Text.Json;
using finance.Models;

namespace finance.Services
{
    public class DebtService
    {
        private const string FilePath = "data/debts.json";

        public async Task<List<Debt>> GetDebtsAsync()
        {
            if (!File.Exists(FilePath))
                return new List<Debt>();

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Debt>>(json) ?? new();
        }

        public async Task SaveDebtsAsync(List<Debt> debts)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentException("FilePath cannot be null or empty", nameof(FilePath));
            }

            var directoryPath = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task AddDebtAsync(Debt debt)
        {
            var debts = await GetDebtsAsync();
            debt.Id = debts.Any() ? debts.Max(d => d.Id) + 1 : 1;
            debts.Add(debt);
            await SaveDebtsAsync(debts);
        }

        public async Task UpdateDebtAsync(Debt updatedDebt)
        {
            var debts = await GetDebtsAsync();
            var existingDebt = debts.FirstOrDefault(d => d.Id == updatedDebt.Id);

            if (existingDebt != null)
            {
                existingDebt.Title = updatedDebt.Title;
                existingDebt.Amount = updatedDebt.Amount;
                existingDebt.DebtSource = updatedDebt.DebtSource;
                existingDebt.DueDate = updatedDebt.DueDate;
                existingDebt.TagId = updatedDebt.TagId;
                existingDebt.Notes = updatedDebt.Notes;

                await SaveDebtsAsync(debts);
            }
        }

        public async Task DeleteDebtAsync(int id)
        {
            var debts = await GetDebtsAsync();
            debts.RemoveAll(d => d.Id == id);
            await SaveDebtsAsync(debts);
        }
    }
}
