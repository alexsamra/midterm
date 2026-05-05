using model;

namespace frontend.Model;

public class BankClient
{
    private readonly UserService _userService;

    public BankClient(UserService userService)
    {
        _userService = userService;
    }

    public User? Login(string login, string pin)
    {
        return _userService.ValidateLogin(login, pin);
    }

    public bool Withdraw(int userId, decimal amount)
    {
        return _userService.Withdraw(userId, amount);
    }

    public bool Deposit(int userId, decimal amount)
    {
        return _userService.Deposit(userId, amount);
    }

    public (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
    {
        return _userService.CreateUser(login, pin, holdersName, balance, status);
    }

    public User? GetUser(int id)
    {
        return _userService.GetUserById(id);
    }

    public bool DeleteUser(int id)
    {
        return _userService.DeleteUser(id);
    }

    public (bool success, string? error) UpdateAccount(int id, string login, string pin, string holdersName, string status)
    {
        return _userService.UpdateUser(id, login, pin, holdersName, status);
    }
}
