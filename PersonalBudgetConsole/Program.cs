using PersonalBudgetConsole;

namespace PersonalBudgetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create budget manager and user interface
            BudgetManager budgetManager = new BudgetManager();
            UserInterface userInterface = new UserInterface(budgetManager);

            // Welcome message
            Console.WriteLine("Welcome to Personal Budget Tracker, " + budgetManager.UserName + "!");
            Console.WriteLine("Your financial data will be saved automatically.");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            // Main application loop
            bool running = true;
            while (running)
            {
                try
                {
                    userInterface.DisplayMainMenu();
                    int choice = userInterface.GetUserChoice();

                    switch (choice)
                    {
                        case 1:
                            userInterface.AddIncome();
                            break;
                        case 2:
                            userInterface.AddExpense();
                            break;
                        case 3:
                            userInterface.ViewTransactions();
                            break;
                        case 4:
                            userInterface.ViewCategorySummary();
                            break;
                        case 5:
                            userInterface.SetMonthlyBudgetLimit();
                            break;
                        case 6:
                            userInterface.ViewBudgetStatus();
                            break;
                        case 7:
                            userInterface.AddNote();
                            break;
                        case 8:
                            userInterface.ViewAllNotes();
                            break;
                        case 9:
                            userInterface.SetUserName();
                            break;
                        case 10:
                            userInterface.ViewDailyAverageSpending();
                            break;
                        case 11:
                            Console.WriteLine("Thank you for using Personal Budget Tracker, " + budgetManager.UserName + "!");
                            Console.WriteLine("Your data has been saved automatically.");
                            running = false;
                            break;
                        case -1:
                            // Invalid choice, menu will be displayed again
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
