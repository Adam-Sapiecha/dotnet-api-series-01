namespace WebApplication1.Dtos;

public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }

    public UserDto Organizer { get; set; } = null!;

    public List<TagDto> Tags { get; set; } = new();
    public List<UserDto> Participants { get; set; } = new();
}
