using MySql.Data.MySqlClient;

namespace dal;

public class UserDal
{
    private readonly string _connectionString;

    public UserDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public (int id, string login, decimal? balance, bool isAdmin)? ValidateLogin(string login, string pin)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT id, login, balance, is_admin FROM users WHERE login = @login AND pin = @pin",
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
                reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                reader.GetBoolean("is_admin")
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

    public int CreateUser(string login, string pin, decimal balance)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "INSERT INTO users (login, pin, balance, is_admin) VALUES (@login, @pin, @balance, FALSE)",
            connection
        );
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);
        cmd.Parameters.AddWithValue("@balance", balance);

        cmd.ExecuteNonQuery();
        return (int)cmd.LastInsertedId;
    }
}
