using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        var events = await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Tags)
            .Include(e => e.Users)
            .OrderBy(e => e.Date)
            .ToListAsync();

        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = e.Date,
            Organizer = new UserDto
            {
                Id = e.Organizer.Id,
                Username = e.Organizer.Username,
                Email = e.Organizer.Email
            },
            Tags = e.Tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList(),
            Participants = e.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList()
        }).ToList();
    }

    public async Task UpdateEventAsync(int id, PutEventDto dto)
    {
        var ev = await _context.Events
            .Include(e => e.Tags)
            .Include(e => e.Users)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            throw new Exception("Event not found");

        var organizer = await _context.Users.FindAsync(dto.OrganizerId);
        if (organizer == null)
            throw new Exception("Organizer not found");

        var tags = await _context.Tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
        if (tags.Count != dto.TagIds.Count)
            throw new Exception("Some tags not found");

        var participants = await _context.Users.Where(u => dto.ParticipantIds.Contains(u.Id)).ToListAsync();
        if (participants.Count != dto.ParticipantIds.Count)
            throw new Exception("Some participants not found");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            ev.Title = dto.Title;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            ev.Organizer = organizer;

            ev.Tags.Clear();
            foreach (var tag in tags)
                ev.Tags.Add(tag);

            ev.Users.Clear();
            foreach (var user in participants)
                ev.Users.Add(user);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
