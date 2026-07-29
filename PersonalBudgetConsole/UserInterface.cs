using System.Globalization;

namespace PersonalBudgetConsole
{
    // UserInterface class to handle all console interactions
    public class UserInterface
    {
        private BudgetManager budgetManager;
        private NoteManager noteManager;

        // Constructor
        public UserInterface(BudgetManager manager)
        {
            budgetManager = manager;
            noteManager = new NoteManager();
        }

        // Display main menu
        public void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Personal Budget Tracker - Welcome " + budgetManager.UserName + "! ===");
            Console.WriteLine("1. Add Income");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. View Transactions");
            Console.WriteLine("4. View Category Summary");
            Console.WriteLine("5. Set Monthly Budget Limit");
            Console.WriteLine("6. View Budget Status");
            Console.WriteLine("7. Add Note");
            Console.WriteLine("8. View All Notes");
            Console.WriteLine("9. Set User Name");
            Console.WriteLine("10. View Daily Average Spending");
            Console.WriteLine("11. Exit");
            Console.WriteLine("================================");
        }

        // Get user choice from menu
        public int GetUserChoice()
        {
            Console.Write("Enter your choice (1-11): ");
            string input = Console.ReadLine() ?? "";
            
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 11)
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 11.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return -1;
            }
        }

        // Add income transaction
        public void AddIncome()
        {
            Console.Clear();
            Console.WriteLine("=== Add Income ===");
            
            try
            {
                var transaction = GetTransactionInput(TransactionType.Income);
                budgetManager.AddTransaction(transaction);
                Console.WriteLine("Income added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding income: " + ex.Message);
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Add expense transaction
        public void AddExpense()
        {
            Console.Clear();
            Console.WriteLine("=== Add Expense ===");
            
            try
            {
                var transaction = GetTransactionInput(TransactionType.Expense);
                budgetManager.AddTransaction(transaction);
                Console.WriteLine("Expense added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding expense: " + ex.Message);
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Get transaction input from user
        private Transaction GetTransactionInput(TransactionType type)
        {
            DateTime date = GetDateInput();
            string description = GetDescriptionInput();
            decimal amount = GetAmountInput();
            string category = GetCategoryInput();

            return new Transaction(date, description, amount, category, type);
        }

        // Get date input with validation
        private DateTime GetDateInput()
        {
            while (true)
            {
                Console.Write("Enter date (dd/mm/yyyy): ");
                string input = Console.ReadLine() ?? "";
                
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("Invalid date format! Please use dd/mm/yyyy format.");
                }
            }
        }

        // Get description input
        private string GetDescriptionInput()
        {
            while (true)
            {
                Console.Write("Enter description: ");
                string input = Console.ReadLine() ?? "";
                
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                else
                {
                    Console.WriteLine("Description cannot be empty!");
                }
            }
        }

        // Get amount input with validation
        private decimal GetAmountInput()
        {
            while (true)
            {
                Console.Write("Enter amount: $");
                string input = Console.ReadLine() ?? "";
                
                if (decimal.TryParse(input, out decimal amount) && amount > 0)
                {
                    return amount;
                }
                else
                {
                    Console.WriteLine("Invalid amount! Please enter a positive number.");
                }
            }
        }

        // Get category input
        private string GetCategoryInput()
        {
            Console.WriteLine("Common categories:");
            Console.WriteLine("1. Food");
            Console.WriteLine("2. Transport");
            Console.WriteLine("3. Rent");
            Console.WriteLine("4. Utilities");
            Console.WriteLine("5. Entertainment");
            Console.WriteLine("6. Healthcare");
            Console.WriteLine("7. Salary");
            Console.WriteLine("8. Other");
            
            while (true)
            {
                Console.Write("Enter category (1-8 or type custom): ");
                string input = Console.ReadLine() ?? "";
                
                if (!string.IsNullOrWhiteSpace(input))
                {
                    // Check if it's a number choice
                    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 8)
                    {
                        return choice switch
                        {
                            1 => "Food",
                            2 => "Transport",
                            3 => "Rent",
                            4 => "Utilities",
                            5 => "Entertainment",
                            6 => "Healthcare",
                            7 => "Salary",
                            8 => "Other",
                            _ => "Other"
                        };
                    }
                    else
                    {
                        return input.Trim();
                    }
                }
                else
                {
                    Console.WriteLine("Category cannot be empty!");
                }
            }
        }

        // View transactions
        public void ViewTransactions()
        {
            Console.Clear();
            Console.WriteLine("=== View Transactions ===");
            Console.WriteLine("1. View all transactions");
            Console.WriteLine("2. View transactions by date range");
            Console.WriteLine("3. View transactions for specific month");
            Console.Write("Enter your choice (1-3): ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    DisplayAllTransactions();
                    break;
                case "2":
                    DisplayTransactionsByDateRange();
                    break;
                case "3":
                    DisplayTransactionsForMonth();
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display all transactions with simple dash separators
        private void DisplayAllTransactions()
        {
            List<Transaction> transactions = budgetManager.Transactions;
            
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }
            
            Console.WriteLine("\nTotal Transactions: " + transactions.Count);
            Console.WriteLine("------------------------------------------------------------");
            
            for (int i = 0; i < transactions.Count; i++)
            {
                Console.WriteLine(transactions[i].ToString());
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        // Display transactions by date range with simple dash separators
        private void DisplayTransactionsByDateRange()
        {
            Console.Write("Enter start date (dd/mm/yyyy): ");
            DateTime startDate = GetDateInput();
            
            Console.Write("Enter end date (dd/mm/yyyy): ");
            DateTime endDate = GetDateInput();
            
            List<Transaction> transactions = budgetManager.GetTransactionsByDateRange(startDate, endDate);
            
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found in the specified date range.");
                return;
            }
            
            Console.WriteLine("\nTransactions from " + startDate.ToString("dd/MM/yyyy") + " to " + endDate.ToString("dd/MM/yyyy"));
            Console.WriteLine("------------------------------------------------------------");
            
            for (int i = 0; i < transactions.Count; i++)
            {
                Console.WriteLine(transactions[i].ToString());
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        // Display transactions for specific month with simple dash separators
        private void DisplayTransactionsForMonth()
        {
            Console.Write("Enter year: ");
            int year = GetIntegerInput();
            
            Console.Write("Enter month (1-12): ");
            int month = GetIntegerInput(1, 12);
            
            List<Transaction> transactions = budgetManager.GetTransactionsForMonth(year, month);
            
            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found for " + month + "/" + year + ".");
                return;
            }
            
            Console.WriteLine("\nTransactions for " + month + "/" + year);
            Console.WriteLine("------------------------------------------------------------");
            
            for (int i = 0; i < transactions.Count; i++)
            {
                Console.WriteLine(transactions[i].ToString());
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        // View category summary with simple dash separators
        public void ViewCategorySummary()
        {
            Console.Clear();
            Console.WriteLine("=== Category Summary ===");
            
            Console.Write("Enter year: ");
            int year = GetIntegerInput();
            
            Console.Write("Enter month (1-12): ");
            int month = GetIntegerInput(1, 12);
            
            Dictionary<string, decimal> summary = budgetManager.GetCategorySummary(year, month);
            
            if (summary.Count == 0)
            {
                Console.WriteLine("No transactions found for " + month + "/" + year + ".");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nCategory Summary for " + month + "/" + year);
            Console.WriteLine("------------------------------------------------------------");
            
            foreach (KeyValuePair<string, decimal> item in summary)
            {
                string amountStr = item.Value >= 0 ? "$" + item.Value.ToString("F2") : "-$" + Math.Abs(item.Value).ToString("F2");
                Console.WriteLine("Category: " + item.Key + " ----- Amount: " + amountStr);
                Console.WriteLine("------------------------------------------------------------");
            }
            
            //decimal totalIncome = budgetManager.GetTotalIncomeForMonth(year, month);
            //decimal totalExpenses = budgetManager.GetTotalExpensesForMonth(year, month);
            //decimal netAmount = totalIncome - totalExpenses;
            
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine("Total Income ----- $" + totalIncome.ToString("F2"));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine("Total Expenses ----- $" + totalExpenses.ToString("F2"));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine("Net Amount ----- $" + netAmount.ToString("F2"));
            //Console.WriteLine("------------------------------------------------------------");
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Set monthly budget limit
        public void SetMonthlyBudgetLimit()
        {
            Console.Clear();
            Console.WriteLine("=== Set Monthly Budget Limit ===");
            
            Console.Write("Enter monthly budget limit: $");
            decimal limit = GetAmountInput();
            
            budgetManager.MonthlyBudgetLimit = limit;
            Console.WriteLine("Monthly budget limit set to $" + limit.ToString("F2"));
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View budget status with simple dash separators
        public void ViewBudgetStatus()
        {
            Console.Clear();
            Console.WriteLine("=== Budget Status ===");
            
            Console.Write("Enter year: ");
            int year = GetIntegerInput();
            
            Console.Write("Enter month (1-12): ");
            int month = GetIntegerInput(1, 12);
            
            decimal totalExpenses = budgetManager.GetTotalExpensesForMonth(year, month);
            decimal totalIncome = budgetManager.GetTotalIncomeForMonth(year, month);
            string budgetStatus = budgetManager.GetBudgetStatus(year, month);
            
            Console.WriteLine("\nBudget Status for " + month + "/" + year);
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Total Expenses ----- $" + totalExpenses.ToString("F2"));
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Budget Status ----- " + budgetStatus);
            Console.WriteLine("------------------------------------------------------------");
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Get integer input with validation
        private int GetIntegerInput(int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number between " + min + " and " + max + ".");
                }
            }
        }

        // Add note
        public void AddNote()
        {
            Console.Clear();
            Console.WriteLine("=== Add Note ===");
            
            try
            {
                DateTime date = GetDateInput();
                Console.Write("Enter note content: ");
                string content = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine("Note content cannot be empty!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                
                var note = new Note(date, content.Trim());
                noteManager.AddNote(note);
                Console.WriteLine("Note added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding note: " + ex.Message);
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Set user name
        public void SetUserName()
        {
            Console.Clear();
            Console.WriteLine("=== Set User Name ===");
            
            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty!");
            }
            else
            {
                budgetManager.UserName = name.Trim();
                Console.WriteLine("User name set to: " + budgetManager.UserName);
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View all notes
        public void ViewAllNotes()
        {
            Console.Clear();
            Console.WriteLine("=== All Notes ===");
            
            List<Note> allNotes = noteManager.GetAllNotes();
            
            if (allNotes.Count == 0)
            {
                Console.WriteLine("No notes found.");
            }
            else
            {
                Console.WriteLine("Total Notes: " + allNotes.Count);
                Console.WriteLine("------------------------------------------------------------");
                
                for (int i = 0; i < allNotes.Count; i++)
                {
                    Console.WriteLine(allNotes[i].ToString());
                    Console.WriteLine("------------------------------------------------------------");
                }
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View daily average spending
        public void ViewDailyAverageSpending()
        {
            Console.Clear();
            Console.WriteLine("=== Daily Average Spending ===");
            
            decimal dailyAverage = budgetManager.GetDailyAverageSpending();
            
            if (dailyAverage == 0)
            {
                Console.WriteLine("No expense data available.");
            }
            else
            {
                Console.WriteLine("Your daily average spending is: $" + dailyAverage.ToString("F2"));
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}