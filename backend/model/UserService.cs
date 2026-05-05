using dal;

namespace model;

/// Account service
public class UserService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User? ValidateLogin(string login, string pin)
    {
        var result = _userRepository.ValidateLogin(login, pin);
        if (result == null) return null;

        var r = result.Value;
        return new User
        {
            Id = r.id,
            Login = r.login,
            Pin = r.pin,
            HoldersName = r.holdersName,
            Balance = r.balance,
            IsAdmin = r.isAdmin,
            Status = r.status
        };
    }

    public (bool success, string? error) Withdraw(int userId, decimal amount)
    {
        var (isValid, error) = AccountValidator.ValidateAmount(amount);
        if (!isValid)
            return (false, error);

        var success = _userRepository.Withdraw(userId, amount);
        if (!success)
            return (false, "Withdrawal failed. Check balance and account.");

        return (true, null);
    }

    public (bool success, string? error) Deposit(int userId, decimal amount)
    {
        var (isValid, error) = AccountValidator.ValidateAmount(amount);
        if (!isValid)
            return (false, error);

        var success = _userRepository.Deposit(userId, amount);
        if (!success)
            return (false, "Deposit failed.");

        return (true, null);
    }

    public (bool success, int? accountId, string? error) CreateAccount(string login, string pin, string holdersName, decimal balance, string status)
    {
        var (validLogin, loginError) = AccountValidator.ValidateLogin(login);
        if (!validLogin)
            return (false, null, loginError);

        var (validPin, pinError) = AccountValidator.ValidatePin(pin);
        if (!validPin)
            return (false, null, pinError);

        var (validHolder, holderError) = AccountValidator.ValidateHolderName(holdersName);
        if (!validHolder)
            return (false, null, holderError);

        var (validBalance, balanceError) = AccountValidator.ValidateBalance(balance);
        if (!validBalance)
            return (false, null, balanceError);

        var (validStatus, statusError) = AccountValidator.ValidateStatus(status);
        if (!validStatus)
            return (false, null, statusError);

        if (_userRepository.LoginExists(login))
            return (false, null, "Login already exists.");

        var id = _userRepository.CreateUser(login, pin, holdersName, balance, status);
        return id > 0 ? (true, id, null) : (false, null, "Failed to create account.");
    }

    public User? GetAccountById(int userId)
    {
        var result = _userRepository.GetUserById(userId);
        if (result == null) return null;

        var r = result.Value;
        return new User
        {
            Id = r.id,
            Login = r.login,
            Pin = r.pin,
            HoldersName = r.holdersName,
            Balance = r.balance,
            IsAdmin = r.isAdmin,
            Status = r.status
        };
    }

    public (bool success, string? error) DeleteAccount(int userId)
    {
        var success = _userRepository.DeleteUser(userId);
        if (!success)
            return (false, "Account not found.");

        return (true, null);
    }

    public (bool success, string? error) UpdateAccount(int userId, string login, string pin, string holdersName, string status)
    {
        var (validLogin, loginError) = AccountValidator.ValidateLogin(login);
        if (!validLogin)
            return (false, loginError);

        var (validPin, pinError) = AccountValidator.ValidatePin(pin);
        if (!validPin)
            return (false, pinError);

        var (validHolder, holderError) = AccountValidator.ValidateHolderName(holdersName);
        if (!validHolder)
            return (false, holderError);

        var (validStatus, statusError) = AccountValidator.ValidateStatus(status);
        if (!validStatus)
            return (false, statusError);

        var existing = _userRepository.GetUserById(userId);
        if (existing == null)
            return (false, "Account not found.");

        if (existing.Value.login != login && _userRepository.LoginExists(login))
            return (false, "Login already in use.");

        var updated = _userRepository.UpdateUser(userId, login, pin, holdersName, status);
        return updated ? (true, null) : (false, "Update failed.");
    }
}
