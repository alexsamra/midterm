using Dal;
using Model;

namespace tests;

internal sealed class FakeUserRepository : IUserRepository
{
    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? ValidateLoginResult { get; set; }
    public bool WithdrawResult { get; set; }
    public bool DepositResult { get; set; }
    public bool LoginExistsResult { get; set; }
    public int CreateUserResult { get; set; }
    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? GetUserByIdResult { get; set; }
    public bool DeleteUserResult { get; set; }
    public bool UpdateUserResult { get; set; }

    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? ValidateLogin(string login, string pin)
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

    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? GetUserById(int userId)
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
    public (bool Success, string? Error) WithdrawResult { get; set; }
    public (bool Success, string? Error) DepositResult { get; set; }
    public (bool Success, int? AccountId, string? Error) CreateAccountResult { get; set; }
    public User? GetAccountByIdResult { get; set; }
    public (bool Success, string? Error) DeleteAccountResult { get; set; }
    public (bool Success, string? Error) UpdateAccountResult { get; set; }

    public User? ValidateLogin(string login, string pin) => ValidateLoginResult;

    public (bool Success, string? Error) Withdraw(int userId, decimal amount) => WithdrawResult;

    public (bool Success, string? Error) Deposit(int userId, decimal amount) => DepositResult;

    public (bool Success, int? AccountId, string? Error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
        => CreateAccountResult;

    public User? GetAccountById(int userId) => GetAccountByIdResult;

    public (bool Success, string? Error) DeleteAccount(int userId) => DeleteAccountResult;

    public (bool Success, string? Error) UpdateAccount(int userId, string login, string pin, string holdersName, string status)
        => UpdateAccountResult;
}
