using model;

namespace frontend.Model;

/// Bank client

public class BankClient
{
    private readonly IAccountService _accountService;

    public BankClient(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    public User? Login(string login, string pin)
    {
        return _accountService.ValidateLogin(login, pin);
    }

    public (bool success, string? error) Withdraw(int userId, decimal amount)
    {
        return _accountService.Withdraw(userId, amount);
    }

    public (bool success, string? error) Deposit(int userId, decimal amount)
    {
        return _accountService.Deposit(userId, amount);
    }

    public (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
    {
        return _accountService.CreateAccount(login, pin, holdersName, balance, status);
    }

    public User? GetUser(int id)
    {
        return _accountService.GetAccountById(id);
    }

    public (bool success, string? error) DeleteUser(int id)
    {
        return _accountService.DeleteAccount(id);
    }

    public (bool success, string? error) UpdateAccount(int id, string login, string pin, string holdersName, string status)
    {
        return _accountService.UpdateAccount(id, login, pin, holdersName, status);
    }
}
