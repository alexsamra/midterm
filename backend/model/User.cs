namespace model;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty;
    public string? HoldersName { get; set; }
    public decimal? Balance { get; set; }
    public bool IsAdmin { get; set; }
    public string Status { get; set; } = "Active";
}
