using Entities.Models;

namespace Web.Models
{
    public class PublicEventDetailViewModel
    {
        public Event Event { get; set; } = null!;

        public IEnumerable<Event> LatestEvents { get; set; } = new List<Event>();
    }
}