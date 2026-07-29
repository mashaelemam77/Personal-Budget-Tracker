using System.Text.Json;

namespace PersonalBudgetConsole
{
    // Enum for transaction types
    public enum TransactionType
    {
        Income,
        Expense
    }

    // Transaction class to represent individual financial records
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public TransactionType Type { get; set; }

        // Constructor
        public Transaction(DateTime date, string description, decimal amount, string category, TransactionType type)
        {
            Date = date;
            Description = description;
            Amount = amount;
            Category = category;
            Type = type;
        }


        // Method to display transaction details with simple format
        public override string ToString()
        {
            string typeStr = Type == TransactionType.Income ? "Income" : "Expense";
            return "Date: " + Date.ToString("dd/MM/yyyy") + " ----- Type: " + typeStr + " ----- Category: " + Category + " ----- Description: " + Description + " ----- Amount: $" + Amount.ToString("F2");
        }

        // Method to get formatted date string
        public string GetFormattedDate()
        {
            return Date.ToString("dd/MM/yyyy");
        }
    }
}