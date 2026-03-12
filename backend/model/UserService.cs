using dal;

namespace model;

public class UserService
{
    private readonly UserDal _userDal;

    public UserService(UserDal userDal)
    {
        _userDal = userDal;
    }

    public User? ValidateLogin(string login, string pin)
    {
        var result = _userDal.ValidateLogin(login, pin);
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

    public bool Withdraw(int userId, decimal amount)
    {
        if (amount <= 0) return false;
        return _userDal.Withdraw(userId, amount);
    }

    public bool Deposit(int userId, decimal amount)
    {
        if (amount <= 0) return false;
        return _userDal.Deposit(userId, amount);
    }

    public (bool success, int? accountId, string? error) CreateUser(string login, string pin, string holdersName, decimal balance, string status)
    {
        if (_userDal.LoginExists(login))
            return (false, null, "Error: Duplicate Login");

        var id = _userDal.CreateUser(login, pin, holdersName, balance, status);
        return id > 0 ? (true, id, null) : (false, null, "Error");
    }

    public User? GetUserById(int userId)
    {
        var result = _userDal.GetUserById(userId);
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

    public bool DeleteUser(int userId)
    {
        return _userDal.DeleteUser(userId);
    }

    public (bool success, string? error) UpdateUser(int userId, string login, string pin, string holdersName, string status)
    {
        var existing = _userDal.GetUserById(userId);
        if (existing == null)
            return (false, "Account not found.");

        if (existing.Value.login != login && _userDal.LoginExists(login))
            return (false, "Error: Duplicate Login");

        var updated = _userDal.UpdateUser(userId, login, pin, holdersName, status);
        return updated ? (true, null) : (false, "Error: Update failed");
    }
}
