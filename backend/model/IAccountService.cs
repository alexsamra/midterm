namespace model;

public interface IAccountService
{
    User? ValidateLogin(string login, string pin);
    (bool success, string? error) Withdraw(int userId, decimal amount);
    (bool success, string? error) Deposit(int userId, decimal amount);
    (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status);
    User? GetAccountById(int userId);
    (bool success, string? error) DeleteAccount(int userId);
    (bool success, string? error) UpdateAccount(int userId, string login, string pin, string holdersName, string status);
}
