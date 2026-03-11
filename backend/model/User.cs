namespace model;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public decimal? Balance { get; set; }
    public bool IsAdmin { get; set; }
}
