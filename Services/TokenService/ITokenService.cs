using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacificaAPI.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Employee user);
    }
}