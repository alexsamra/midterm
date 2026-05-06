// <copyright file="User.cs" company="Midterm">
// Copyright (c) Midterm. All rights reserved.
// </copyright>

namespace Model;

/// <summary>
/// Represents a user account in the banking system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the login username for the user account.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PIN for the user account.
    /// </summary>
    public string Pin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the account holder.
    /// </summary>
    public string? HoldersName { get; set; }

    /// <summary>
    /// Gets or sets the account balance.
    /// </summary>
    public decimal? Balance { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has administrator privileges.
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// Gets or sets the status of the user account (e.g., "Active", "Inactive").
    /// </summary>
    public string Status { get; set; } = "Active";
}
