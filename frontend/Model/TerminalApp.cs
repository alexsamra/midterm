using model;

namespace frontend.Model;

/// Terminal application UI

public static class TerminalApp
{
    public static void Run(BankClient client)
    {
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
                var user = client.Login(login, pin);
                if (user == null)
                {
                    Console.WriteLine("Incorrect login or pin code");
                    continue;
                }

                Console.WriteLine();
                if (user.IsAdmin)
                {
                    AdminMenu(user, client);
                }
                else
                {
                    CustomerMenu(user, client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    private static void CustomerMenu(User user, BankClient client)
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
                    if (!decimal.TryParse(withdrawInput, out var withdrawAmount))
                    {
                        Console.WriteLine("Error: Invalid amount");
                        break;
                    }
                    if (user.Balance.HasValue && withdrawAmount > user.Balance.Value)
                    {
                        Console.WriteLine("Error: Insufficient balance");
                        break;
                    }
                    try
                    {
                        var (success, error) = client.Withdraw(user.Id, withdrawAmount);
                        if (success)
                        {
                            if (user.Balance.HasValue)
                                user.Balance -= withdrawAmount;
                            Console.WriteLine("Cash Successfully Withdrawn");
                            Console.WriteLine($"Account #{user.Id}");
                            Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                            Console.WriteLine($"Withdrawn: {withdrawAmount:N0}");
                            Console.WriteLine($"Balance: {user.Balance:N0}");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {error}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                case "3":
                    Console.Write("Enter the cash amount to deposit: ");
                    var depositInput = Console.ReadLine()?.Trim();
                    if (!decimal.TryParse(depositInput, out var depositAmount))
                    {
                        Console.WriteLine("Error: Invalid amount");
                        break;
                    }
                    try
                    {
                        var (success, error) = client.Deposit(user.Id, depositAmount);
                        if (success)
                        {
                            if (user.Balance.HasValue)
                                user.Balance += depositAmount;
                            Console.WriteLine("Cash Deposited Successfully");
                            Console.WriteLine($"Account #{user.Id}");
                            Console.WriteLine($"Date: {DateTime.Now:MM/dd/yyyy}");
                            Console.WriteLine($"Deposited: {depositAmount:N0}");
                            Console.WriteLine($"Balance: {user.Balance:N0}");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {error}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
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

    private static void AdminMenu(User user, BankClient client)
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
                    HandleCreateAccount(client);
                    break;
                case "2":
                    HandleDeleteAccount(client);
                    break;
                case "3":
                    HandleUpdateAccount(client);
                    break;
                case "4":
                    HandleSearchAccount(client);
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

    private static void HandleCreateAccount(BankClient client)
    {
        Console.Write("Login: ");
        var newLogin = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(newLogin))
        {
            Console.WriteLine("Error: Invalid Login");
            return;
        }

        Console.Write("Pin Code: ");
        var newPin = Console.ReadLine()?.Trim();
        if (newPin == null || newPin.Length != AccountConstants.PinLength || !newPin.All(char.IsDigit))
        {
            Console.WriteLine($"Error: Pin must be {AccountConstants.PinLength} digits");
            return;
        }

        Console.Write("Holders Name: ");
        var newHoldersName = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(newHoldersName))
        {
            Console.WriteLine("Error: Invalid Holder Name");
            return;
        }

        Console.Write("Starting Balance: ");
        var balanceInput = Console.ReadLine()?.Trim();
        if (!decimal.TryParse(balanceInput, out var newBalance) || newBalance < 0)
        {
            Console.WriteLine("Error: Invalid balance");
            return;
        }

        Console.Write("Status (Active/Disabled): ");
        var newStatus = Console.ReadLine()?.Trim();
        if (!AccountConstants.ValidStatuses.Contains(newStatus ?? ""))
        {
            Console.WriteLine($"Error: Status must be one of: {string.Join(", ", AccountConstants.ValidStatuses)}");
            return;
        }

        try
        {
            var (success, createdAccountId, error) = client.CreateAccount(newLogin, newPin, newHoldersName, newBalance, newStatus!);
            if (success)
            {
                Console.WriteLine($"Account Successfully Created – the account number assigned is: {createdAccountId}");
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void HandleDeleteAccount(BankClient client)
    {
        Console.Write("Enter the account number to delete: ");
        var deleteIdInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(deleteIdInput, out var deleteId))
        {
            Console.WriteLine("Error: Invalid account number");
            return;
        }

        try
        {
            var getUserResponse = client.GetUser(deleteId);
            if (getUserResponse == null)
            {
                Console.WriteLine("Error: Account not found");
                return;
            }

            Console.Write($"You wish to delete the account held by {getUserResponse.HoldersName}. If this information is correct, please re-enter\nthe account number: ");
            var confirmInput = Console.ReadLine()?.Trim();
            if (!int.TryParse(confirmInput, out var confirmId) || confirmId != deleteId)
            {
                Console.WriteLine("Error: Different account number");
                return;
            }

            var (success, error) = client.DeleteUser(deleteId);
            if (success)
            {
                Console.WriteLine("Account Deleted Successfully");
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void HandleUpdateAccount(BankClient client)
    {
        Console.WriteLine("Enter the Account Number: ");
        var accountIdInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(accountIdInput, out var accountId))
        {
            Console.WriteLine("Error: Invalid account number");
            return;
        }

        try
        {
            var getUserResponse = client.GetUser(accountId);
            if (getUserResponse == null)
            {
                Console.WriteLine("Error: Account not found");
                return;
            }

            Console.WriteLine($"Account #{accountId}");
            Console.Write("Holder: ");
            var newHolder = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(newHolder))
            {
                Console.WriteLine("Error: Invalid Holder Name");
                return;
            }

            Console.WriteLine($"Balance: {getUserResponse.Balance}");
            Console.Write("Status (Active/Disabled): ");
            var updateStatus = Console.ReadLine()?.Trim();
            if (!AccountConstants.ValidStatuses.Contains(updateStatus ?? ""))
            {
                Console.WriteLine($"Error: Status must be one of: {string.Join(", ", AccountConstants.ValidStatuses)}");
                return;
            }

            Console.Write("Login: ");
            var updateLogin = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(updateLogin))
            {
                Console.WriteLine("Error: Invalid Login");
                return;
            }

            Console.Write("Pin Code: ");
            var updatePin = Console.ReadLine()?.Trim();
            if (updatePin == null || updatePin.Length != AccountConstants.PinLength || !updatePin.All(char.IsDigit))
            {
                Console.WriteLine($"Error: Pin must be {AccountConstants.PinLength} digits");
                return;
            }

            var (success, error) = client.UpdateAccount(accountId, updateLogin, updatePin, newHolder, updateStatus!);
            if (success)
            {
                Console.WriteLine("Account Updated Successfully");
            }
            else
            {
                Console.WriteLine($"Error: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void HandleSearchAccount(BankClient client)
    {
        Console.Write("Enter the account number: ");
        var searchAccountInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(searchAccountInput, out var searchAccountId))
        {
            Console.WriteLine("Error: Invalid account number");
            return;
        }

        try
        {
            var getUserResponse = client.GetUser(searchAccountId);
            if (getUserResponse == null)
            {
                Console.WriteLine("Error: Account not found");
                return;
            }

            Console.WriteLine($"Account #{searchAccountId}");
            Console.WriteLine($"Holder: {getUserResponse.HoldersName}");
            Console.WriteLine($"Balance: {getUserResponse.Balance}");
            Console.WriteLine($"Status: {getUserResponse.Status}");
            Console.WriteLine($"Login: {getUserResponse.Login}");
            Console.WriteLine($"Pin: {getUserResponse.Pin}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
