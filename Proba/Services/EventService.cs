using Microsoft.EntityFrameworkCore;
using Proba.Data;
using Proba.DTOs;
using Proba.Models;

namespace Proba.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;
    public EventService(AppDbContext context) => _context = context;

    public async Task<List<EventDetailDto>> GetAllEventDetailsAsync()
    {
        var events = await _context.Events.ToListAsync();
        var result = new List<EventDetailDto>();
        foreach (var e in events)
        {
            var organizer = await _context.Users.FindAsync(e.OrganizerId);
            var participants = await _context.EventParticipants
                .Where(ep => ep.EventId == e.Id)
                .Join(_context.Users,
                      ep => ep.UserId,
                      u => u.Id,
                      (ep, u) => new UserDto { Id = u.Id, Username = u.Username })
                .ToListAsync();
            var tags = await _context.EventTags
                .Where(et => et.EventId == e.Id)
                .Join(_context.Tags,
                      et => et.TagId,
                      t => t.Id,
                      (et, t) => new TagDto { Id = t.Id, Name = t.Name })
                .ToListAsync();

            result.Add(new EventDetailDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Organizer = new UserDto { Id = organizer!.Id, Username = organizer.Username },
                Participants = participants,
                Tags = tags
            });
        }
        return result;
    }

    public async Task<EditEventResultDto> UpdateEventAsync(int id, EditEventDto dto)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null)
            throw new KeyNotFoundException($"Event {id} not found");
        ev.Title = dto.Title;
        ev.Description = dto.Description;
        ev.Date = dto.Date;
        var oldTags = _context.EventTags.Where(et => et.EventId == id);
        _context.EventTags.RemoveRange(oldTags);
        var newTags = dto.TagIds.Select(tagId => new EventTag { EventId = id, TagId = tagId });
        await _context.EventTags.AddRangeAsync(newTags);

        var oldParts = _context.EventParticipants.Where(ep => ep.EventId == id);
        _context.EventParticipants.RemoveRange(oldParts);
        var newParts = dto.ParticipantIds.Select(userId => new EventParticipant { EventId = id, UserId = userId });
        await _context.EventParticipants.AddRangeAsync(newParts);

        await _context.SaveChangesAsync();

        return new EditEventResultDto
        {
            Message = "Wydarzenie zaktualizowane pomyślnie.",
            EventId = id,
            UpdatedTags = dto.TagIds,
            UpdatedParticipants = dto.ParticipantIds
        };
    }
}
