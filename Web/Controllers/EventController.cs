using System.Security.Claims;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _environment;
        public EventController(IEventService eventService, IWebHostEnvironment environment)
        {
            _eventService = eventService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var events = _eventService.GetAllEvents(false);

            return View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Image is null)
            {
                ModelState.AddModelError("Image", "Etkinlik görseli zorunludur.");

                return View(model);
            }

            string extension = Path.GetExtension(model.Image.FileName).ToLower();

            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("Image", "Sadece JPG, JPEG veya PNG dosyaları yüklenebilir.");

                return View(model);
            }

            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Görsel boyutu en fazla 2 MB olabilir.");

                return View(model);
            }

            string fileName = Guid.NewGuid().ToString() + extension;

            string uploadFolder = Path.Combine(_environment.WebRootPath, "images", "events");

            Directory.CreateDirectory(uploadFolder);

            string filePath = Path.Combine(uploadFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");

            }

            int userId = int.Parse(userIdValue);

            var eventDto = new EventDto
            {
                Title = model.Title,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                ImagePath = "/images/events/" + fileName,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                IsActive = model.IsActive
            };

            try
            {
                _eventService.CreateEvent(eventDto, userId);

                return RedirectToAction("Index");
            }
            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var eventEntity = _eventService.GetEventById(id, false);

            if (eventEntity is null)
            {
                return NotFound();
            }

            var model = new EventFormViewModel
            {
                EventId = eventEntity.EventId,
                Title = eventEntity.Title,
                StartDateTime = eventEntity.StartDateTime,
                EndDateTime = eventEntity.EndDateTime,
                ShortDescription = eventEntity.ShortDescription,
                LongDescription = eventEntity.LongDescription,
                IsActive = eventEntity.IsActive
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventEntity = _eventService.GetEventById(model.EventId, false);

            if (eventEntity is null)
            {
                return NotFound();
            }

            string imagePath = eventEntity.ImagePath;

            if (model.Image is not null)
            {
                string extension = Path.GetExtension(model.Image.FileName).ToLower();

                string[] allowedExtensions =
                {
                     ".jpg",
                     ".jpeg",
                     ".png"
                };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Sadece JPG, JPEG veya PNG dosyaları yüklenebilir.");

                    return View(model);
                }

                if (model.Image.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Görsel boyutu en fazla 2 MB olabilir.");

                    return View(model);
                }

                string fileName = Guid.NewGuid().ToString() + extension;

                string uploadFolder = Path.Combine(_environment.WebRootPath, "images", "events");

                Directory.CreateDirectory(uploadFolder);

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imagePath = "/images/events/" + fileName;
            }

            var updatedEvent = new Entities.Models.Event
            {
                EventId = eventEntity.EventId,
                CreatedByUserId = eventEntity.CreatedByUserId,
                Title = model.Title,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                ImagePath = imagePath,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                IsActive = model.IsActive,
                CreatedAt = eventEntity.CreatedAt
            };

            try
            {
                _eventService.UpdateEvent(updatedEvent);

                return RedirectToAction("Index");
            }

            catch (InvalidOperationException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);

                return View(model);
            }
        }
    }
}