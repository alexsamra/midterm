using dal;
using model;

namespace tests;

internal sealed class FakeUserRepository : IUserRepository
{
    public (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? ValidateLoginResult { get; set; }
    public bool WithdrawResult { get; set; }
    public bool DepositResult { get; set; }
    public bool LoginExistsResult { get; set; }
    public int CreateUserResult { get; set; }
    public (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? GetUserByIdResult { get; set; }
    public bool DeleteUserResult { get; set; }
    public bool UpdateUserResult { get; set; }

    public (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? ValidateLogin(string login, string pin)
    {
        return ValidateLoginResult;
    }

    public bool Withdraw(int userId, decimal amount)
    {
        return WithdrawResult;
    }

    public bool Deposit(int userId, decimal amount)
    {
        return DepositResult;
    }

    public bool LoginExists(string login)
    {
        return LoginExistsResult;
    }

    public int CreateUser(string login, string pin, string holdersName, decimal balance, string status = "Active")
    {
        return CreateUserResult;
    }

    public (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? GetUserById(int userId)
    {
        return GetUserByIdResult;
    }

    public bool DeleteUser(int userId)
    {
        return DeleteUserResult;
    }

    public bool UpdateUser(int userId, string login, string pin, string holdersName, string status)
    {
        return UpdateUserResult;
    }
}

internal sealed class FakeAccountService : IAccountService
{
    public User? ValidateLoginResult { get; set; }
    public (bool success, string? error) WithdrawResult { get; set; }
    public (bool success, string? error) DepositResult { get; set; }
    public (bool success, int? accountId, string? error) CreateAccountResult { get; set; }
    public User? GetAccountByIdResult { get; set; }
    public (bool success, string? error) DeleteAccountResult { get; set; }
    public (bool success, string? error) UpdateAccountResult { get; set; }

    public User? ValidateLogin(string login, string pin) => ValidateLoginResult;

    public (bool success, string? error) Withdraw(int userId, decimal amount) => WithdrawResult;

    public (bool success, string? error) Deposit(int userId, decimal amount) => DepositResult;

    public (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
        => CreateAccountResult;

    public User? GetAccountById(int userId) => GetAccountByIdResult;

    public (bool success, string? error) DeleteAccount(int userId) => DeleteAccountResult;

    public (bool success, string? error) UpdateAccount(int userId, string login, string pin, string holdersName, string status)
        => UpdateAccountResult;
}
