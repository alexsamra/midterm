using System.Net.Http.Json;
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
                await AdminMenu(user);
            }
            else
            {
                await CustomerMenu(user);
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

static Task CustomerMenu(UserResponse user)
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
                Console.WriteLine("Withdraw Cash - (not yet implemented)");
                break;
            case "3":
                Console.WriteLine("Deposit Cash - (not yet implemented)");
                break;
            case "4":
                Console.WriteLine($"Account #{user.Id}");
                Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                Console.WriteLine($"Balance: {user.Balance:N2}");
                break;
            case "5":
                return Task.CompletedTask;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
        Console.WriteLine();
    }
}

static Task AdminMenu(UserResponse user)
{
    while (true)
    {
        Console.WriteLine($"Welcome, {user.login} (Admin)");
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
                Console.WriteLine("Create New Account - (not yet implemented)");
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
                return Task.CompletedTask;
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
    public decimal? Balance { get; set; }
    public bool IsAdmin { get; set; }
}

