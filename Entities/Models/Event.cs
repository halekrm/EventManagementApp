namespace Entities.Models;

public class Event
{
    public int EventId { get; set; }

    public int CreatedByUserId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string LongDescription { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public User CreatedByUser { get; set; } = null!;
}