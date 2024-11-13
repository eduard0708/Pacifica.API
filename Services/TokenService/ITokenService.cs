using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Employee user);
    }
}