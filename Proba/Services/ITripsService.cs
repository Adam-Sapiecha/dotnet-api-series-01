using System;

using Proba.Dtos;
using Proba.DTOs;

namespace Proba.Services
{
    public interface ITripService
    {
        Task<DTOs.TripsResponse> GetPagedTripsAsync(int page, int pageSize);
        Task<TripDto?> GetTripByIdAsync(int idTrip);
        Task<bool> AddClientToTripAsync(int idTrip, ClientCreateDto dto);
    }
}
