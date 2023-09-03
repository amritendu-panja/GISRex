using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public interface ITokenHelper
    {
        string GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(string refreshToken);
    }
}
