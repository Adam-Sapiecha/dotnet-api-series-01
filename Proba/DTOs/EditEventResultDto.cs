using System;

namespace Proba.DTOs;

public class EditEventResultDto
{
    public string Message { get; set; } = null!;
    public int EventId { get; set; }
    public List<int> UpdatedTags { get; set; } = new();
    public List<int> UpdatedParticipants { get; set; } = new();
}