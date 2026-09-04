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

        private int? GetCurrentUserId()
        {
            string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdValue))
            {
                return null;
            }

            return int.Parse(userIdValue);
        }

        private bool CanManageEvent(Entities.Models.Event eventEntity)
        {
            int? currentUserId = GetCurrentUserId();

            return User.IsInRole("Admin") ||
            (currentUserId.HasValue && eventEntity.CreatedByUserId == currentUserId.Value);
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
            var now = DateTime.Now;

            var startDateTime = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                0);

            var model = new EventFormViewModel
            {
                StartDateTime = startDateTime,
                EndDateTime = DateTime.Now.AddHours(1),
                IsActive = true
            };

            return View("EventForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EventForm", model);
            }

            if (model.Image is null)
            {
                ModelState.AddModelError("Image", "Etkinlik görseli zorunludur.");

                return View("EventForm", model);
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

                return View("EventForm", model);
            }

            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Görsel boyutu en fazla 2 MB olabilir.");

                return View("EventForm", model);
            }

            string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");

            }

            int userId = int.Parse(userIdValue);

            if (_eventService.UserHasEventWithTitle(userId, model.Title))
            {
                ModelState.AddModelError("Title", "Bu isimde bir etkinliğiniz zaten var. Lütfen başlığı değiştirin.");

                return View("EventForm", model);
            }

            string fileName = Guid.NewGuid().ToString() + extension;

            string uploadFolder = Path.Combine(_environment.WebRootPath, "images", "events");

            Directory.CreateDirectory(uploadFolder);

            string filePath = Path.Combine(uploadFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

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
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                ModelState.AddModelError(string.Empty, exception.Message);

                return View("EventForm", model);
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

            if (!CanManageEvent(eventEntity))
            {
                return Forbid();
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

            return View("EventForm", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EventForm", model);
            }

            var eventEntity = _eventService.GetEventById(model.EventId, false);

            if (eventEntity is null)
            {
                return NotFound();
            }

            if (!CanManageEvent(eventEntity))
            {
                return Forbid();
            }

            if (_eventService.UserHasEventWithTitle(eventEntity.CreatedByUserId, model.Title, eventEntity.EventId))
            {
                ModelState.AddModelError("Title", "Bu isimde bir etkinliğiniz zaten var. Lütfen başlığı değiştirin.");

                return View("EventForm", model);
            }

            string imagePath = eventEntity.ImagePath;
            string oldImagePath = eventEntity.ImagePath;

            string? newFilePath = null;

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

                    return View("EventForm", model);
                }

                if (model.Image.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Görsel boyutu en fazla 2 MB olabilir.");

                    return View("EventForm", model);
                }

                string fileName = Guid.NewGuid().ToString() + extension;

                string uploadFolder = Path.Combine(_environment.WebRootPath, "images", "events");

                Directory.CreateDirectory(uploadFolder);

                newFilePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
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

                if (model.Image is not null && !string.IsNullOrEmpty(oldImagePath))
                {
                    string relativeOldPath = oldImagePath.TrimStart('/');

                    string fullOldPath = Path.Combine(_environment.WebRootPath, relativeOldPath.Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(fullOldPath))
                    {
                        System.IO.File.Delete(fullOldPath);
                    }
                }

                return RedirectToAction("Index");
            }

            catch (InvalidOperationException exception)
            {
                if (!string.IsNullOrEmpty(newFilePath) && System.IO.File.Exists(newFilePath))
                {
                    System.IO.File.Delete(newFilePath);
                }

                ModelState.AddModelError(string.Empty, exception.Message);

                return View("EventForm", model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var eventEntity = _eventService.GetEventById(id, false);

            if (eventEntity is null)
            {
                return NotFound();
            }

            if (!CanManageEvent(eventEntity))
            {
                return Forbid();
            }

            string imagePath = eventEntity.ImagePath;

            try
            {
                _eventService.DeleteEvent(id);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    string relativePath = imagePath.TrimStart('/');

                    string fullPath = Path.Combine(_environment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                return RedirectToAction("Index");
            }

            catch (InvalidOperationException exception)
            {
                TempData["ErrorMessage"] = exception.Message;

                return RedirectToAction("Index");
            }
        }
    }
}