using System;
using Proba.DTOs;


// Dtos/TripsResponse.cs
namespace Proba.Dtos;
public class TripsResponse
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; } = new();
}
