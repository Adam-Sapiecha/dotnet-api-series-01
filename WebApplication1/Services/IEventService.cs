using WebApplication1.Dtos;

namespace WebApplication1.Services;

public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync();
    Task UpdateEventAsync(int id, PutEventDto dto);
}
