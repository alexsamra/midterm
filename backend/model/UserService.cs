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
            Balance = r.balance,
            IsAdmin = r.isAdmin,
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
}
