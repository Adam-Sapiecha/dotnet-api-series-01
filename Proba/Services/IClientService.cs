using System;

namespace Proba.Services
{
    public interface IClientService
    {
        Task<bool> DeleteClientAsync(int clientId);
    }
}
