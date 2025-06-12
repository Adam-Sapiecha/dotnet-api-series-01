using System;

namespace Proba.DTOs;

public class TripsResponse
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; } = new();
}
