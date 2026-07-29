using System.Text.Json;

namespace PersonalBudgetConsole
{
    // BudgetManager class to handle all budget operations
    public class BudgetManager
    {
        private List<Transaction> transactions;
        private decimal monthlyBudgetLimit;
        private string dataFilePath;
        private string userName;

        // Constructor
        public BudgetManager(string filePath = "budget_data.json")
        {
            transactions = new List<Transaction>();
            monthlyBudgetLimit = 0;
            dataFilePath = filePath;
            userName = "User";
            LoadData();
        }

        // Property to get transactions (read-only)
        public List<Transaction> Transactions => transactions;

        // Property for monthly budget limit
        public decimal MonthlyBudgetLimit
        {
            get => monthlyBudgetLimit;
            set => monthlyBudgetLimit = value;
        }

        // Property for user name
        public string UserName
        {
            get => userName;
            set => userName = value;
        }

        // Add a new transaction
        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
            SaveData();
        }

        // Get transactions within a date range
        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Transaction> result = new List<Transaction>();
            for (int i = 0; i < transactions.Count; i++)
            {
                if (transactions[i].Date >= startDate && transactions[i].Date <= endDate)
                {
                    result.Add(transactions[i]);
                }
            }
            return result;
        }

        // Get transactions for a specific month
        public List<Transaction> GetTransactionsForMonth(int year, int month)
        {
            List<Transaction> result = new List<Transaction>();
            for (int i = 0; i < transactions.Count; i++)
            {
                if (transactions[i].Date.Year == year && transactions[i].Date.Month == month)
                {
                    result.Add(transactions[i]);
                }
            }
            return result;
        }

        // Get category summary for a specific month
        public Dictionary<string, decimal> GetCategorySummary(int year, int month)
        {
            List<Transaction> monthTransactions = GetTransactionsForMonth(year, month);
            Dictionary<string, decimal> summary = new Dictionary<string, decimal>();

            for (int i = 0; i < monthTransactions.Count; i++)
            {
                Transaction transaction = monthTransactions[i];
                if (summary.ContainsKey(transaction.Category))
                {
                    if (transaction.Type == TransactionType.Income)
                        summary[transaction.Category] += transaction.Amount;
                    else
                        summary[transaction.Category] -= transaction.Amount;
                }
                else
                {
                    summary[transaction.Category] = transaction.Type == TransactionType.Income 
                        ? transaction.Amount 
                        : -transaction.Amount;
                }
            }

            return summary;
        }

        // Get total expenses for a month
        public decimal GetTotalExpensesForMonth(int year, int month)
        {
            List<Transaction> monthTransactions = GetTransactionsForMonth(year, month);
            decimal total = 0;
            for (int i = 0; i < monthTransactions.Count; i++)
            {
                if (monthTransactions[i].Type == TransactionType.Expense)
                {
                    total += monthTransactions[i].Amount;
                }
            }
            return total;
        }

        // Get total income for a month
        public decimal GetTotalIncomeForMonth(int year, int month)
        {
            List<Transaction> monthTransactions = GetTransactionsForMonth(year, month);
            decimal total = 0;
            for (int i = 0; i < monthTransactions.Count; i++)
            {
                if (monthTransactions[i].Type == TransactionType.Income)
                {
                    total += monthTransactions[i].Amount;
                }
            }
            return total;
        }

        // Check if budget limit is exceeded
        public bool IsBudgetExceeded(int year, int month)
        {
            if (monthlyBudgetLimit <= 0) return false;
            return GetTotalExpensesForMonth(year, month) > monthlyBudgetLimit;
        }

        // Get budget status for a month
        public string GetBudgetStatus(int year, int month)
        {
            if (monthlyBudgetLimit <= 0)
                return "No budget limit set";

            decimal totalExpenses = GetTotalExpensesForMonth(year, month);
            decimal remaining = monthlyBudgetLimit - totalExpenses;

            if (remaining < 0)
                return "BUDGET EXCEEDED! Over by $" + Math.Abs(remaining).ToString("F2");
            else
                return "Budget OK. Remaining: $" + remaining.ToString("F2");
        }

        // Get daily average spending for all data
        public decimal GetDailyAverageSpending()
        {
            if (transactions.Count == 0)
                return 0;

            List<Transaction> expenseTransactions = new List<Transaction>();
            for (int i = 0; i < transactions.Count; i++)
            {
                if (transactions[i].Type == TransactionType.Expense)
                {
                    expenseTransactions.Add(transactions[i]);
                }
            }
            
            if (expenseTransactions.Count == 0)
                return 0;

            // Calculate total days between first and last transaction
            DateTime firstTransaction = expenseTransactions[0].Date;
            DateTime lastTransaction = expenseTransactions[0].Date;
            
            for (int i = 1; i < expenseTransactions.Count; i++)
            {
                if (expenseTransactions[i].Date < firstTransaction)
                    firstTransaction = expenseTransactions[i].Date;
                if (expenseTransactions[i].Date > lastTransaction)
                    lastTransaction = expenseTransactions[i].Date;
            }
            
            int totalDays = (lastTransaction - firstTransaction).Days + 1;

            if (totalDays == 0)
                return 0;

            decimal totalExpenses = 0;
            for (int i = 0; i < expenseTransactions.Count; i++)
            {
                totalExpenses += expenseTransactions[i].Amount;
            }
            return totalExpenses / totalDays;
        }

        // Save data to JSON file
        public void SaveData()
        {
            try
            {
                var data = new
                {
                    Transactions = transactions,
                    MonthlyBudgetLimit = monthlyBudgetLimit,
                    UserName = userName
                };

                string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(dataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

        // Load data from JSON file
        public void LoadData()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    string jsonString = File.ReadAllText(dataFilePath);
                    var data = JsonSerializer.Deserialize<JsonElement>(jsonString);

                    if (data.TryGetProperty("Transactions", out var transactionsElement))
                    {
                        transactions = JsonSerializer.Deserialize<List<Transaction>>(transactionsElement.GetRawText()) ?? new List<Transaction>();
                    }

                    if (data.TryGetProperty("MonthlyBudgetLimit", out var budgetElement))
                    {
                        monthlyBudgetLimit = budgetElement.GetDecimal();
                    }

                    if (data.TryGetProperty("UserName", out var userNameElement))
                    {
                        userName = userNameElement.GetString() ?? "User";
                    }
                }
                else
                {
                    Console.WriteLine("No existing data file found. Starting with empty budget.");
                    transactions = new List<Transaction>();
                    monthlyBudgetLimit = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
                Console.WriteLine("Starting with empty budget.");
                transactions = new List<Transaction>();
                monthlyBudgetLimit = 0;
            }
        }

        // Clear all transactions (for testing purposes)
        public void ClearAllTransactions()
        {
            transactions.Clear();
            SaveData();
        }
    }
}