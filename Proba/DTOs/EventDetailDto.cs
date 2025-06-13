using System;
using System.Collections.Generic;
namespace Proba.DTOs;

public class EventDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public UserDto Organizer { get; set; } = null!;
    public List<UserDto> Participants { get; set; } = new();
    public List<TagDto> Tags { get; set; } = new();
}

