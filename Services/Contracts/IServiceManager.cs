namespace Services.Contracts
{
    public interface IServiceManager
    {
        IUserService UserService {get;}
        IEventService EventService {get;}
    }
}