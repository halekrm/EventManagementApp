namespace Entities.Models;

public class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string EncryptedPassword { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public string Role { get; set; } = "User";

    public ICollection<Event> Events { get; set; } = new List<Event>();
}