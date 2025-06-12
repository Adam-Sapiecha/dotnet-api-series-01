using System;

using Microsoft.EntityFrameworkCore;
using Proba.Data;

namespace Proba.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        public ClientService(AppDbContext context) => _context = context;

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == clientId);
            if (client == null) 
                return false;
            if (client.ClientTrips.Any()) 
                throw new InvalidOperationException("Klient przypisany do wycieczki");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
