using MySql.Data.MySqlClient;

namespace dal;

public class UserDal
{
    private readonly string _connectionString;

    public UserDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public (int id, string login, string? holdersName, decimal? balance, bool isAdmin, string status)? ValidateLogin(string login, string pin)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT id, login, holders_name, balance, is_admin, status FROM users WHERE login = @login AND pin = @pin",
            connection
        );
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (
                reader.GetInt32("id"),
                reader.GetString("login"),
                reader.IsDBNull(reader.GetOrdinal("holders_name")) ? null : reader.GetString("holders_name"),
                reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                reader.GetBoolean("is_admin"),
                reader.GetString("status")
            );
        }
        return null;
    }

    public bool Withdraw(int userId, decimal amount)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "UPDATE users SET balance = balance - @amount WHERE id = @id AND balance >= @amount",
            connection
        );
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@amount", amount);

        return cmd.ExecuteNonQuery() > 0;
    }

    public bool Deposit(int userId, decimal amount)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "UPDATE users SET balance = balance + @amount WHERE id = @id",
            connection
        );
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@amount", amount);

        return cmd.ExecuteNonQuery() > 0;
    }

    public bool LoginExists(string login)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT COUNT(*) FROM users WHERE login = @login",
            connection
        );
        cmd.Parameters.AddWithValue("@login", login);

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public int CreateUser(string login, string pin, string holdersName, decimal balance, string status = "Active")
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "INSERT INTO users (login, pin, holders_name, balance, is_admin, status) VALUES (@login, @pin, @holdersName, @balance, FALSE, @status)",
            connection
        );
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);
        cmd.Parameters.AddWithValue("@holdersName", holdersName);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);

        cmd.ExecuteNonQuery();
        return (int)cmd.LastInsertedId;
    }

    public (int id, string login, string? holdersName, decimal? balance, bool isAdmin, string status)? GetUserById(int userId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT id, login, holders_name, balance, is_admin, status FROM users WHERE id = @id",
            connection
        );
        cmd.Parameters.AddWithValue("@id", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (
                reader.GetInt32("id"),
                reader.GetString("login"),
                reader.IsDBNull(reader.GetOrdinal("holders_name")) ? null : reader.GetString("holders_name"),
                reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                reader.GetBoolean("is_admin"),
                reader.GetString("status")
            );
        }
        return null;
    }

    public bool DeleteUser(int userId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "DELETE FROM users WHERE id = @id",
            connection
        );
        cmd.Parameters.AddWithValue("@id", userId);

        return cmd.ExecuteNonQuery() > 0;
    }
}
