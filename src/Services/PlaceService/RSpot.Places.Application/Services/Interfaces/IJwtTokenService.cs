﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Places.Application.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string role);
    }
}
