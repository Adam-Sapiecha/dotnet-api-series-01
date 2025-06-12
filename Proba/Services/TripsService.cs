using System;

using Microsoft.EntityFrameworkCore;
using Proba.Data;
using Proba.Dtos;
using Proba.DTOs;

namespace Proba.Services
{
    public class TripService : ITripService
    {
        private readonly AppDbContext _context;
        public TripService(AppDbContext context) => _context = context;

        public async Task<DTOs.TripsResponse> GetPagedTripsAsync(int page, int pageSize)
        {
            var total = await _context.Trips.CountAsync();
            var pages = (int)Math.Ceiling(total / (double)pageSize);

            var trips = await _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TripDto {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries
                        .Select(c => new CountryDto { Name = c.Name })
                        .ToList(),
                    Clients = t.ClientTrips
                        .Select(ct => new ClientShortDto {
                            FirstName = ct.IdClientNavigation.FirstName,
                            LastName  = ct.IdClientNavigation.LastName
                        })
                        .ToList()
                })
                .ToListAsync();

            return new DTOs.TripsResponse {
                PageNum  = page,
                PageSize = pageSize,
                AllPages = pages,
                Trips    = trips
            };
        }

        public async Task<TripDto?> GetTripByIdAsync(int idTrip)
        {
            var t = await _context.Trips
                .Include(x => x.IdCountries)
                .Include(x => x.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip);
            if (t == null) return null;

            return new TripDto {
                Name        = t.Name,
                Description = t.Description,
                DateFrom    = t.DateFrom,
                DateTo      = t.DateTo,
                MaxPeople   = t.MaxPeople,
                Countries   = t.IdCountries.Select(c => new CountryDto { Name = c.Name }).ToList(),
                Clients     = t.ClientTrips.Select(ct => new ClientShortDto {
                                   FirstName = ct.IdClientNavigation.FirstName,
                                   LastName  = ct.IdClientNavigation.LastName
                               }).ToList()
            };
        }

        public async Task<bool> AddClientToTripAsync(int idTrip, ClientCreateDto dto)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
            if (trip == null || trip.DateFrom <= DateTime.UtcNow)
                return false;

            if (await _context.Clients.AnyAsync(c => c.Pesel == dto.Pesel))
                throw new InvalidOperationException($"PESEL {dto.Pesel} już istnieje");

            var client = new Proba.Models.Client {
                FirstName = dto.FirstName,
                LastName  = dto.LastName,
                Email     = dto.Email,
                Telephone = dto.Telephone,
                Pesel     = dto.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            _context.ClientTrips.Add(new Proba.Models.ClientTrip {
                IdTrip       = idTrip,
                IdClient     = client.IdClient,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate  = dto.PaymentDate
            });
            await _context.SaveChangesAsync();

            return true;
        }

        Task<DTOs.TripsResponse> ITripService.GetPagedTripsAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        Task<TripDto?> ITripService.GetTripByIdAsync(int idTrip)
        {
            throw new NotImplementedException();
        }

        Task<bool> ITripService.AddClientToTripAsync(int idTrip, ClientCreateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
