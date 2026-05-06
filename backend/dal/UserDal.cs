// <copyright file="UserDal.cs" company="Midterm">
// Copyright (c) Midterm. All rights reserved.
// </copyright>

namespace Dal;

using MySql.Data.MySqlClient;

/// <summary>
/// Data access layer implementation for user repository operations.
/// Provides database access for user account management using MySQL.
/// </summary>
public class UserDal : IUserRepository
{
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDal"/> class.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    public UserDal(string connectionString)
    {
        this.connectionString = connectionString;
    }

    /// <summary>
    /// Validates user login credentials against the database.
    /// </summary>
    /// <param name="login">The user's login username.</param>
    /// <param name="pin">The user's PIN.</param>
    /// <returns>A tuple containing user information if credentials are valid; otherwise, null.</returns>
    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? ValidateLogin(string login, string pin)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT id, login, pin, holders_name, balance, is_admin, status FROM users WHERE login = @login AND pin = @pin",
            connection);
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (
                reader.GetInt32("id"),
                reader.GetString("login"),
                reader.GetString("pin"),
                reader.IsDBNull(reader.GetOrdinal("holders_name")) ? null : reader.GetString("holders_name"),
                reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                reader.GetBoolean("is_admin"),
                reader.GetString("status"));
        }

        return null;
    }

    /// <summary>
    /// Withdraws the specified amount from a user account in the database.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to withdraw.</param>
    /// <returns>True if the withdrawal was successful; otherwise, false.</returns>
    public bool Withdraw(int userId, decimal amount)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "UPDATE users SET balance = balance - @amount WHERE id = @id AND balance >= @amount",
            connection);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@amount", amount);

        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Deposits the specified amount into a user account in the database.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>True if the deposit was successful; otherwise, false.</returns>
    public bool Deposit(int userId, decimal amount)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "UPDATE users SET balance = balance + @amount WHERE id = @id",
            connection);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@amount", amount);

        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Checks if a login username already exists in the database.
    /// </summary>
    /// <param name="login">The login username to check.</param>
    /// <returns>True if the login exists; otherwise, false.</returns>
    public bool LoginExists(string login)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT COUNT(*) FROM users WHERE login = @login",
            connection);
        cmd.Parameters.AddWithValue("@login", login);

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    /// <summary>
    /// Creates a new user account in the database.
    /// </summary>
    /// <param name="login">The login username for the new account.</param>
    /// <param name="pin">The PIN for the new account.</param>
    /// <param name="holdersName">The name of the account holder.</param>
    /// <param name="balance">The initial account balance.</param>
    /// <param name="status">The status of the account (default: "Active").</param>
    /// <returns>The ID of the newly created user.</returns>
    public int CreateUser(string login, string pin, string holdersName, decimal balance, string status = "Active")
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "INSERT INTO users (login, pin, holders_name, balance, is_admin, status) VALUES (@login, @pin, @holdersName, @balance, FALSE, @status)",
            connection);
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);
        cmd.Parameters.AddWithValue("@holdersName", holdersName);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);

        cmd.ExecuteNonQuery();
        return (int)cmd.LastInsertedId;
    }

    /// <summary>
    /// Retrieves user information from the database by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A tuple containing user information if found; otherwise, null.</returns>
    public (int Id, string Login, string Pin, string? HoldersName, decimal? Balance, bool IsAdmin, string Status)? GetUserById(int userId)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "SELECT id, login, pin, holders_name, balance, is_admin, status FROM users WHERE id = @id",
            connection);
        cmd.Parameters.AddWithValue("@id", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (
                reader.GetInt32("id"),
                reader.GetString("login"),
                reader.GetString("pin"),
                reader.IsDBNull(reader.GetOrdinal("holders_name")) ? null : reader.GetString("holders_name"),
                reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                reader.GetBoolean("is_admin"),
                reader.GetString("status"));
        }

        return null;
    }

    /// <summary>
    /// Deletes a user account from the database.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public bool DeleteUser(int userId)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "DELETE FROM users WHERE id = @id",
            connection);
        cmd.Parameters.AddWithValue("@id", userId);

        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Updates user account information in the database.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="login">The new login username.</param>
    /// <param name="pin">The new PIN.</param>
    /// <param name="holdersName">The new holder name.</param>
    /// <param name="status">The new account status.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    public bool UpdateUser(int userId, string login, string pin, string holdersName, string status)
    {
        using var connection = new MySqlConnection(this.connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(
            "UPDATE users SET login = @login, pin = @pin, holders_name = @holdersName, status = @status WHERE id = @id",
            connection);
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@pin", pin);
        cmd.Parameters.AddWithValue("@holdersName", holdersName);
        cmd.Parameters.AddWithValue("@status", status);

        return cmd.ExecuteNonQuery() > 0;
    }
}
