using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Web.Models;

namespace Web.Controllers
{
    public class PublicEventController : Controller
    {
        private readonly IEventService _eventService;

        public PublicEventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var events = _eventService.GetUpcomingActiveEvents(false);

            return View(events);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var eventEntity = _eventService.GetEventById(id, false);

            if (eventEntity is null  || !eventEntity.IsActive || eventEntity.StartDateTime < DateTime.Now)
            {
                return NotFound();
            }

            var latestEvents = _eventService.GetLatestEvents(5, false);

            var model = new PublicEventDetailViewModel
            {
                Event = eventEntity,
                LatestEvents = latestEvents
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Calendar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CalendarEvents()
        {
            var events = _eventService.GetUpcomingActiveEvents(false);

            var calendarEvents = events.Select(eventEntity => new
            {
                id = eventEntity.EventId,
                title = eventEntity.Title,
                start = eventEntity.StartDateTime,
                end = eventEntity.EndDateTime,
                url = Url.Action("Details", "PublicEvent", new { id = eventEntity.EventId })
            });

            return Json(calendarEvents);
        }

    }
}