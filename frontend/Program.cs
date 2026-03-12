using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5236")
};

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

while (true)
{
    Console.Write("Enter login: ");
    var login = Console.ReadLine()?.Trim();

    Console.Write("Enter Pin code: ");
    var pin = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pin))
    {
        Console.WriteLine("Login and pin code are required.");
        continue;
    }

    try
    {
        var response = await httpClient.PostAsJsonAsync("/Login", new { Login = login, Pin = pin });

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserResponse>(jsonOptions);
            if (user == null)
            {
                Console.WriteLine("Error: Please try again");
                continue;
            }

            Console.WriteLine();
            if (user.IsAdmin)
            {
                await AdminMenu(user, httpClient, jsonOptions);
            }
            else
            {
                await CustomerMenu(user, httpClient);
            }
        }
        else
        {
            Console.WriteLine("Incorrect login or pin code");
        }
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("Unable to connect to the server");
    }

    Console.WriteLine();
}

static async Task CustomerMenu(UserResponse user, HttpClient httpClient)
{
    while (true)
    {
        Console.WriteLine("1----Withdraw Cash");
        Console.WriteLine("3----Deposit Cash");
        Console.WriteLine("4----Display Balance");
        Console.WriteLine("5----Exit");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter the withdrawal amount: ");
                var withdrawInput = Console.ReadLine()?.Trim();
                if (!decimal.TryParse(withdrawInput, out var withdrawAmount) || withdrawAmount <= 0)
                {
                    Console.WriteLine("Error: Invalid amount");
                    break;
                }
                if (withdrawAmount > user.Balance)
                {
                    Console.WriteLine("Error: Insufficient balance");
                    break;
                }
                try
                {
                    var response = await httpClient.PostAsJsonAsync("/Account/withdraw", new { UserId = user.Id, Amount = withdrawAmount });
                    if (response.IsSuccessStatusCode)
                    {
                        user.Balance -= withdrawAmount;
                        Console.WriteLine("Cash Successfully Withdrawn");
                        Console.WriteLine($"Account #{user.Id}");
                        Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                        Console.WriteLine($"Withdrawn: {withdrawAmount:N0}");
                        Console.WriteLine($"Balance: {user.Balance:N0}");
                    }
                    else
                    {
                        Console.WriteLine("Error: Withdrawal failed");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Error: Check connection");
                }
                break;
            case "3":
                Console.Write("Enter the cash amount to deposit: ");
                var depositInput = Console.ReadLine()?.Trim();
                if (!decimal.TryParse(depositInput, out var depositAmount) || depositAmount <= 0)
                {
                    Console.WriteLine("Error: Invalid amount");
                    break;
                }
                try
                {
                    var response = await httpClient.PostAsJsonAsync("/Account/deposit", new { UserId = user.Id, Amount = depositAmount });
                    if (response.IsSuccessStatusCode)
                    {
                        user.Balance += depositAmount;
                        Console.WriteLine("Cash Deposited Successfully");
                        Console.WriteLine($"Account #{user.Id}");
                        Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                        Console.WriteLine($"Withdrawn: {depositAmount:N0}");
                        Console.WriteLine($"Balance: {user.Balance:N0}");
                    }
                    else
                    {
                        Console.WriteLine("Error: Deposit failed");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Error: Check connection");
                }
                break;
            case "4":
                Console.WriteLine($"Account #{user.Id}");
                Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                Console.WriteLine($"Balance: {user.Balance:N2}");
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
        Console.WriteLine();
    }
}

static async Task AdminMenu(UserResponse user, HttpClient httpClient, JsonSerializerOptions jsonOptions)
{
    while (true)
    {
        Console.WriteLine($"Welcome, {user.Login} (Admin)");
        Console.WriteLine("1----Create New Account");
        Console.WriteLine("2----Delete Existing Account");
        Console.WriteLine("3----Update Account Information");
        Console.WriteLine("4----Search for Account");
        Console.WriteLine("6----Exit");
        Console.Write("Select an option: ");

        var choice = Console.ReadLine()?.Trim();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                Console.Write("Login: ");
                var newLogin = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(newLogin))
                {
                    Console.WriteLine("Error: Invalid Login");
                    break;
                }
                Console.Write("Pin Code: ");
                var newPin = Console.ReadLine()?.Trim();
                if (newPin == null || newPin.Length != 5 || !newPin.All(char.IsDigit))
                {
                    Console.WriteLine("Error: Invalid Pin");
                    break;
                }
                Console.Write("Holders Name: ");
                var newHoldersName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(newHoldersName))
                {
                    Console.WriteLine("Error: Invalid Holder Name");
                    break;
                }
                Console.Write("Starting Balance: ");
                var balanceInput = Console.ReadLine()?.Trim();
                if (!decimal.TryParse(balanceInput, out var newBalance) || newBalance < 0)
                {
                    Console.WriteLine("Error: Invalid balance");
                    break;
                }
                Console.Write("Status (Active/Disabled): ");
                var newStatus = Console.ReadLine()?.Trim();
                if (newStatus != "Active" && newStatus != "Disabled")
                {
                    Console.WriteLine("Error: Invalid Status");
                    break;
                }
                try
                {
                    var createResponse = await httpClient.PostAsJsonAsync("/Account/create", new { Login = newLogin, Pin = newPin, HolderName = newHoldersName, Balance = newBalance, Status = newStatus });
                    if (createResponse.IsSuccessStatusCode)
                    {
                        var result = await createResponse.Content.ReadFromJsonAsync<CreateAccountResponse>(jsonOptions);
                        Console.WriteLine($"Account Successfully Created – the account number assigned is: {result?.Id}");
                    }
                    else
                    {
                        var errorResponse = await createResponse.Content.ReadFromJsonAsync<ErrorResponse>(jsonOptions);
                        Console.WriteLine(errorResponse?.Message ?? "Error: Account creation failed");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Error: Check connection");
                }
                break;
            case "2":
                Console.WriteLine("Delete Existing Account - (not yet implemented)");
                break;
            case "3":
                Console.WriteLine("Update Account Information - (not yet implemented)");
                break;
            case "4":
                Console.WriteLine("Search for Account - (not yet implemented)");
                break;
            case "6":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
        Console.WriteLine();
    }
}

public class UserResponse
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string? HoldersName { get; set; }
    public decimal? Balance { get; set; }
    public bool IsAdmin { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateAccountResponse
{
    public int Id { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}

