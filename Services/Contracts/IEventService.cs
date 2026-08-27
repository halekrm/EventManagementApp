using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IEventService
    {
        IEnumerable<Event> GetAllEvents(bool trackChanges);
        IEnumerable<Event> GetUpcomingActiveEvents(bool trackChanges);
        IEnumerable<Event> GetLatestEvents(int count, bool trackChanges);
        Event? GetEventById(int id, bool trackChanges);
        void CreateEvent(Event eventEntity);
        void CreateEvent(EventDto eventDto, int userId);
        void UpdateEvent(Event eventEntity);
        void DeleteEvent(int id);
        bool UserHasEventWithTitle(
            int userId,
            string title,
            int? excludedEventId = null
        );
    }
}