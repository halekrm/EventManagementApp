using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;

        public ServiceManager(
            IUserService userService,
            IEventService eventService
        )
        {
            _userService = userService;
            _eventService = eventService;
        }

        public IUserService UserService => _userService;
        public IEventService EventService => _eventService;
    }
}