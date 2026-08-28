using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class EventService : IEventService
    {
        private readonly IRepositoryManager _repositoryManager;

        public EventService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public IEnumerable<Event> GetAllEvents(bool trackChanges)
        {
            return _repositoryManager.Event
                    .FindAll(trackChanges)
                    .Include(eventEntity => eventEntity.CreatedByUser)
                    .OrderByDescending(eventEntity => eventEntity.CreatedAt)
                    .ToList();
        }

        public Event? GetEventById(int id, bool trackChanges)
        {
            return _repositoryManager.Event
                    .FindByCondition(eventEntity => eventEntity.EventId == id, trackChanges)
                    .SingleOrDefault();
        }
        public void CreateEvent(EventDto eventDto, int userId)
        {
            var eventEntity = new Event
            {
                CreatedByUserId = userId,
                Title = eventDto.Title,
                StartDateTime = eventDto.StartDateTime,
                EndDateTime = eventDto.EndDateTime,
                ImagePath = eventDto.ImagePath,
                ShortDescription = eventDto.ShortDescription,
                LongDescription = eventDto.LongDescription,
                IsActive = eventDto.IsActive
            };

            CreateEvent(eventEntity);
        }
        public void CreateEvent(Event eventEntity)
        {
            ValidateEventDates(eventEntity);

            if (UserHasEventWithTitle(eventEntity.CreatedByUserId, eventEntity.Title))
            {
                throw new InvalidOperationException("Aynı kullanıcı aynı başlıkla birden fazla etkinlik oluşturamaz!");
            }
            _repositoryManager.Event.Create(eventEntity);
            _repositoryManager.Save();
        }

        public void DeleteEvent(int id)
        {
            var eventEntity = GetEventById(id, true);
            if (eventEntity is null)
            {
                throw new InvalidOperationException("Silinecek etkinlik bulunamadı!");
            }
            _repositoryManager.Event.Delete(eventEntity);
            _repositoryManager.Save();
        }

        public void UpdateEvent(Event eventEntity)
        {
            ValidateEventDates(eventEntity);

            if (UserHasEventWithTitle(eventEntity.CreatedByUserId, eventEntity.Title, eventEntity.EventId))
            {
                throw new InvalidOperationException("Aynı kullanıcı aynı başlıkla birden fazla etkinlik oluşturamaz!");
            }
            _repositoryManager.Event.Update(eventEntity);
            _repositoryManager.Save();
        }

        public bool UserHasEventWithTitle(int userId, string title, int? excludedEventId = null)
        {
            return _repositoryManager.Event.FindByCondition(eventEntity =>
                    eventEntity.CreatedByUserId == userId &&
                    eventEntity.Title == title &&
                    (!excludedEventId.HasValue ||
                    eventEntity.EventId != excludedEventId.Value), false)
                    .Any();
        }

        private void ValidateEventDates(Event eventEntity)
        {
            if (eventEntity.EndDateTime <= eventEntity.StartDateTime)
            {
                throw new InvalidOperationException("Etkinlik bitiş tarihi başlangıç tarihinden sonra olmalıdır!");
            }
        }

        public IEnumerable<Event> GetUpcomingActiveEvents(bool trackChanges)
        {
            return _repositoryManager.Event.FindByCondition(eventEntity =>
                   eventEntity.IsActive &&
                   eventEntity.StartDateTime >= DateTime.Now, trackChanges)
                   .OrderByDescending(eventEntity => eventEntity.CreatedAt)
                   .ToList();
        }

        public IEnumerable<Event> GetLatestEvents(int count, bool trackChanges)
        {
            return _repositoryManager.Event.FindByCondition(eventEntity =>
            eventEntity.IsActive &&
            eventEntity.StartDateTime >= DateTime.Now, trackChanges)
            .OrderByDescending(eventEntity => eventEntity.CreatedAt)
            .Take(count)
            .ToList();
        }
    }
}