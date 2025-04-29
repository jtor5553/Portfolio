using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace FinalProject{
public abstract class Person{ //Person Class
    public string FirstName{get; set;}
    public string LastName{get; set;}
    public abstract string GetInfo();
}
    public enum AccountType{Savings, Checking}
    public enum LoanType{Home, Auto, Personal, None}
    public enum JobTitle{Manager, LoanOfficer}

public class Employee : Person{
    public string Username{get; set;}  
    public string Password{get; set;}
    public JobTitle Title{get; set;}  
    public string AccountNumber{get; set;}
    public string PIN{get; set;}
    public AccountType AccountType{get; set;}
    public double Balance{get; set;}
    public LoanType LoanType{get; set;}
    public double LoanBalance{get; set;}

    public override string GetInfo(){
        return $"Name: {FirstName} {LastName} - Title: {Title}";
    }
}

public class Customer : Person{
    public string AccountNumber{get; set;}
    public string PIN{get; set;}
    public AccountType AccountType{get; set;}
    public double Balance{get; set;}
    public LoanType LoanType{get; set;}
    public double LoanBalance{get; set;}

    public override string GetInfo(){
        return $"\n---------- ACCOUNT #: {AccountNumber} --------------\n" +
               $"Name: {FirstName} {LastName}\n" +
               $"Account Balance: ${Balance:F2}\n" +
               $"Loan Balance: ${LoanBalance:F2}";
    }
}

class Program{
    private static List<Customer> customers = new List<Customer>();
    private static List<Employee> employees = new List<Employee>();
    private static string customerFilePath = "customer_data.csv";
    private static string adminFilePath = "employee_data.csv";

static void Main(){//Loads data and main menu
    LoadData();
    MainMenu();
}

static void MainMenu(){//Writes main menu and options
    while (true){
        Console.WriteLine("\n---------\nMAIN MENU\n---------");
        Console.WriteLine("1. Account Login");
        Console.WriteLine("2. Create Account");
        Console.WriteLine("3. Administrator Login");
        Console.WriteLine("4. Quit");
        Console.Write("Select Option: ");

        string choice = Console.ReadLine();//Reads input and decides what to do off input
            switch (choice){
                case "1": AccountLogin(); break;
                case "2": CreateAccount(); break;
                case "3": EmployeeLogin(); break;
                case "4": SaveData(); return;
                default: Console.WriteLine("Invalid choice. Try again."); break;
        }
    }
}

static void LoadData(){//checks for customer data
    if(File.Exists(customerFilePath)){
        foreach (var line in File.ReadAllLines(customerFilePath)){
            var parts = line.Split(',');
            customers.Add(new Customer{ // 
                AccountNumber = parts[0],
                PIN = parts[1],
                FirstName = parts[2],
                LastName = parts[3],
                Balance = double.Parse(parts[4]),
                AccountType = Enum.TryParse(parts[5], out AccountType accountType) ? accountType : AccountType.Savings,
                LoanType = Enum.TryParse(parts[6], out LoanType loanType) ? loanType : LoanType.None,
                LoanBalance = double.Parse(parts[7]),
            });
        }
    }

    if (File.Exists(adminFilePath)) { // Load employee data
    foreach (var line in File.ReadAllLines(adminFilePath)){
        var parts = line.Split(',');
        
       JobTitle title;
        if (!Enum.TryParse(parts[4], ignoreCase: true, out title)) {
            Console.WriteLine($"Invalid title value '{parts[4]}' for employee {parts[2]} {parts[3]}. Defaulting to Manager.");
            title = JobTitle.Manager; // Default to Manager
        }


        employees.Add(new Employee { 
            Username = parts[0],
            Password = parts[1],
            FirstName = parts[2],
            LastName = parts[3],
            Title = title,
        });
    }
}
}


static void SaveData(){//saves data
     var customerLines = customers.Select(c =>
        $"{c.AccountNumber},{c.PIN},{c.FirstName},{c.LastName},{c.Balance},{c.AccountType},{c.LoanType},{c.LoanBalance}");
    File.WriteAllLines(customerFilePath, customerLines);

    var employeeLines = employees.Select(e =>
    $"{e.Username},{e.Password},{e.FirstName},{e.LastName},{e.Title},{e.LoanType},{e.LoanBalance}");
    File.WriteAllLines(adminFilePath, employeeLines);
}

static void AccountLogin(){//Account log in tab
    Console.Write("\n--------------\nAccount Login\n--------------\n");
    Console.Write("Enter Account Number: ");
    string accountNumber = Console.ReadLine();
    Console.Write("Enter PIN: ");
    string pin = Console.ReadLine();

    var customer = customers.FirstOrDefault(c => c.AccountNumber == accountNumber && c.PIN == pin);

        if (customer == null){
            Console.WriteLine("\n--------------------\nInvalid account # or PIN.\n--------------------");
            return;
        }

    AccountServicesMenu(customer);
    }

static void CreateAccount(){//Create account tab
    Console.Write("\n---------------\nCreate Account\n---------------\n");
    Console.Write("Enter First Name: ");
    string firstName = Console.ReadLine();
    Console.Write("Enter Last Name: ");
    string lastName = Console.ReadLine();

    string pin;
    do{
        Console.Write("Enter a 4-digit PIN: ");
        pin = Console.ReadLine();
    } 
    while(pin.Length != 4 || !pin.All(char.IsDigit));
        Console.WriteLine("Select Account Type: 1. Savings  2. Checking");
        AccountType accountType = Console.ReadLine() == "1" ? AccountType.Savings : AccountType.Checking;

        string accountNumber = "183977" + new Random().Next(1000000000).ToString("D10");
        customers.Add(new Customer{
            AccountNumber = accountNumber,
            PIN = pin,
            FirstName = firstName,
            LastName = lastName,
            Balance = 100,
            AccountType = accountType,
            LoanType = LoanType.None,
            LoanBalance = 0
    });

        Console.WriteLine($"\n-----------------------------------------------\nAccount created successfully! Your account number is {accountNumber}\n-----------------------------------------------");
}

static void EmployeeLogin(){//Employee log in tab
    Console.Write("\n------------\nEmployee Login\n------------\n");
    Console.Write("Enter Username: ");
    string username = Console.ReadLine();
    Console.Write("Enter Password: ");
    string password = Console.ReadLine();

    var employee = employees.FirstOrDefault(e => e.Username == username && e.Password == password);

    if (employee == null){
        Console.WriteLine("\n------------------------\nInvalid login credentials.\n------------------------");
        return;
    }

    EmployeeMenu(employee);
}

static void AccountServicesMenu(Customer customer){//Accounnt services menu
    while(true){
        Console.WriteLine("\n-----------------\nAccount Services\n-----------------\n");
        Console.WriteLine("1. Deposit");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Transfer");
        Console.WriteLine("4. Loan Payment");
        Console.WriteLine("5. Balance Inquiry");
        Console.WriteLine("6. Back to Main Menu");
        Console.Write("Select Option: ");

        string choice = Console.ReadLine();
        switch(choice){
            case "1": Deposit(customer); break;
            case "2": Withdraw(customer); break;
            case "3": Transfer(customer); break;
            case "4": LoanPayment(customer); break;
            case "5": Console.WriteLine(customer.GetInfo()); break;
            case "6": return;
            default: Console.WriteLine("Invalid choice. Try again."); break;
        }
    }
}


static void Deposit(Customer customer){//Deposit tab
    Console.Write("Enter deposit amount: ");
        if(double.TryParse(Console.ReadLine(), out double amount) && amount > 0){
            customer.Balance += amount;
            Console.WriteLine($"Deposit successful! New balance: ${customer.Balance:F2}");
        }
        else{
            Console.WriteLine("Invalid amount. Try again.");
        }
    }

static void Withdraw(Customer customer){//Withdrawl tab
    Console.Write("Enter withdrawal amount: ");
        if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0 && amount <= customer.Balance){
            customer.Balance -= amount;
            Console.WriteLine($"Withdrawal successful! New balance: ${customer.Balance:F2}");
        }
        else{
            Console.WriteLine("Invalid amount or insufficient funds.");
        }
    }

static void Transfer(Customer sender){//Transfer tab
    Console.Write("Enter recipient account number: ");
    string recipientAccount = Console.ReadLine();
    var recipient = customers.FirstOrDefault(c => c.AccountNumber == recipientAccount);

        if(recipient == null){
            Console.WriteLine("\n---------------------------\nRecipient account not found.\n---------------------------");
            return;
    }

        Console.Write("Enter transfer amount: ");
            if(double.TryParse(Console.ReadLine(), out double amount) && amount > 0 && amount <= sender.Balance){
                sender.Balance -= amount;
                recipient.Balance += amount;
                Console.WriteLine($"Transfer successful! New balance: ${sender.Balance:F2}");
            }
                else{
                    Console.WriteLine("Invalid amount or insufficient funds.");
                }
        }

static void LoanPayment(Customer customer){//Loan tab
    if(customer.LoanBalance <= 0){
        Console.WriteLine("\n--------------------------\nNo outstanding loan balance.\n--------------------------");
        return;
}

Console.Write("Enter loan payment amount: ");
    if(double.TryParse(Console.ReadLine(), out double amount) && amount > 0 && amount <= customer.Balance && amount <= customer.LoanBalance){
        customer.Balance -= amount;
        customer.LoanBalance -= amount;
        Console.WriteLine($"Loan payment successful! New loan balance: ${customer.LoanBalance:F2}");
    }
    else{
        Console.WriteLine("Invalid amount or insufficient funds.");
    }
}

static void EmployeeMenu(Employee employee){//Employee tab
    while(true) {
        Console.WriteLine("\nAdmin Menu:");
        Console.WriteLine("1. Average Savings Account Balance");
        Console.WriteLine("2. Total Savings Account Balance");
        Console.WriteLine("3. Average Checking Account Balance");
        Console.WriteLine("4. Total Checking Account Balance");
        Console.WriteLine("5. Number of Accounts by Type");
        Console.WriteLine("6. Total Outstanding Loan Balances");
        Console.WriteLine("7. View All Employee Information");
        Console.WriteLine("8. Back to Main Menu");
        Console.Write("Select Option: ");
        
        string choice = Console.ReadLine();
        switch(choice) {
            case "1": ShowAverageBalance(AccountType.Savings); break;
            case "2": ShowTotalBalance(AccountType.Savings); break;
            case "3": ShowAverageBalance(AccountType.Checking); break;
            case "4": ShowTotalBalance(AccountType.Checking); break;
            case "5": ShowAccountCountByType(); break;
            case "6": ShowLoanBalances(); break;
            case "7": ShowEmployeeInformation(); break;
            case "8": return;
            default: Console.WriteLine("Invalid choice. Try again."); break;
        }
    }
}

static void ShowAverageBalance(AccountType type){//Avergae balance for employee to see
    var accounts = customers.Where(c => c.AccountType == type).ToList();
        if(accounts.Count == 0){
            Console.WriteLine($"No {type} accounts found.");
            return;
        }

        double average = accounts.Average(c => c.Balance);
        Console.WriteLine($"Average {type} Account Balance: ${average:F2}");
}
static void ShowTotalBalance(AccountType type){//Shows total balance
    var accounts = customers.Where(c => c.AccountType == type).ToList();
        if(accounts.Count == 0){
            Console.WriteLine($"No {type} accounts found.");
            return;
        }

        double total = accounts.Sum(c => c.Balance);
        Console.WriteLine($"Total {type} Account Balance: ${total:F2}");
}

static void ShowAccountCountByType(){// Shows account by type
    var savingsCount = customers.Count(c => c.AccountType == AccountType.Savings);
    var checkingCount = customers.Count(c => c.AccountType == AccountType.Checking);

    Console.WriteLine($"Savings Accounts: {savingsCount}");
    Console.WriteLine($"Checking Accounts: {checkingCount}");
}
static void ShowLoanBalances(){//Shows loans
    var loans = customers.Where(c => c.LoanType != LoanType.None);
    foreach(LoanType loanType in Enum.GetValues(typeof(LoanType))){
        if(loanType == LoanType.None) continue;
            double totalLoan = loans.Where(c => c.LoanType == loanType).Sum(c => c.LoanBalance);
            Console.WriteLine($"Total {loanType} Loan Balance: ${totalLoan:F2}");
        }
    }
static void ShowEmployeeInformation(){//Shows employee info
    if(employees.Count == 0){
        Console.WriteLine("No employees found.");
        return;
    }

        foreach (var employee in employees){
            Console.WriteLine(employee.GetInfo());
        }
    }
}
}
