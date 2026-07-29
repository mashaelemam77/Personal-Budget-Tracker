# Personal Budget Tracker

A simple console-based application for managing personal finances, built with C# and .NET 9.0.

## Features

### Core Functionalities
- ✅ Add Income and Expense records
- ✅ View transactions with date filtering
- ✅ Category summaries for monthly spending
- ✅ Monthly budget limit with warnings
- Automatic data persistence (JSON)

### Input Validation
- ✅ Date format validation (dd/mm/yyyy)
- ✅ Amount validation (positive numbers only)
- ✅ Description validation (non-empty)
- ✅ Category selection with predefined options

### Data Management
- ✅ JSON file persistence (`budget_data.json`)
- ✅ Automatic data loading on startup
- ✅ Error handling for missing files

## How to Use

1. **Run the application:**
   ```bash
   dotnet run
   ```

2. **Main Menu Options:**
   - **1. Add Income** - Record salary, bonuses, etc.
   - **2. Add Expense** - Record purchases, bills, etc.
   - **3. View Transactions** - See all transactions or filter by date
   - **4. View Category Summary** - Monthly spending by category
   - **5. Set Monthly Budget Limit** - Set spending limits
   - **6. View Budget Status** - Check if you're over budget
   - **7. Exit** - Save and exit

3. **Adding Transactions:**
   - Enter date in dd/mm/yyyy format
   - Provide a description
   - Enter amount (positive number)
   - Select category from list or enter custom

4. **Data Storage:**
   - All data is automatically saved to `budget_data.json`
   - Data persists between application sessions
   - File is created automatically if it doesn't exist

## Object-Oriented Design

###  Classes and Their Responsibilities:

1. **Transaction Class**
   - Encapsulates transaction data (Date, Description, Amount, Category, Type)
   - Provides string representation and formatting methods

2. **BudgetManager Class**
   - Manages all budget operations (CRUD operations)
   - Handles data persistence (JSON serialization/deserialization)
   - Calculates summaries and budget status
   - Implements business logic for budget tracking

3. **UserInterface Class**
   - Handles all console interactions
   - Validates user input
   - Provides user-friendly menus and displays
   - Separates UI logic from business logic

4. **Program Class**
   - Application entry point
   - Coordinates between BudgetManager and UserInterface
   - Handles application lifecycle

### OOP Principles Applied:

- **Encapsulation**: Each class has private fields and public methods
- **Abstraction**: UserInterface abstracts complex operations behind simple method calls
- **Composition**: Program class composes BudgetManager and UserInterface
- **Single Responsibility**: Each class has one clear purpose

## File Structure

```
PersonalBudgetConsole/
├── Program.cs              # Main application entry point
├── Transaction.cs          # Transaction data model
├── BudgetManager.cs       # Core business logic
├── UserInterface.cs       # Console user interface
├── budget_data.json       # Data persistence file (created automatically)
└── README.md              # This file
```

## Example Usage

1. Start the application
2. Add some income: "Salary" for $3000 in "Salary" category
3. Add some expenses: "Groceries" for $150 in "Food" category
4. Set monthly budget limit to $2000
5. View category summary to see spending breakdown
6. Check budget status to see if you're over budget

The application will automatically save all data and load it when you restart.
