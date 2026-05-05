namespace dal;


/// Data access interface
public interface IUserRepository
{
    (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? ValidateLogin(string login, string pin);
    bool Withdraw(int userId, decimal amount);
    bool Deposit(int userId, decimal amount);
    bool LoginExists(string login);
    int CreateUser(string login, string pin, string holdersName, decimal balance, string status = "Active");
    (int id, string login, string pin, string? holdersName, decimal? balance, bool isAdmin, string status)? GetUserById(int userId);
    bool DeleteUser(int userId);
    bool UpdateUser(int userId, string login, string pin, string holdersName, string status);
}
