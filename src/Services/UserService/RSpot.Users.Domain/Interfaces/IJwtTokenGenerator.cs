using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        DateTime GetExpiration(string jwt);   // *дополнительно* вернуть exp
    }
}
