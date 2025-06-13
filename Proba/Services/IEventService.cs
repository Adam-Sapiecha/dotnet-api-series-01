using System;
using Proba.DTOs;

namespace Proba.Services;

public interface IEventService
{
    Task<List<EventDetailDto>> GetAllEventDetailsAsync();
    Task<EditEventResultDto> UpdateEventAsync(int id, EditEventDto dto);
}