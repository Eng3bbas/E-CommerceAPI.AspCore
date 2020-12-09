using System;
using System.Collections;
using System.Linq;
using System.Security.Claims;
using E_Commerce.Data.Entities;

namespace E_Commerce.Extensions
{
    public static class ClaimsExtensions
    {
        public static Nullable<Guid> GetUserId(this ClaimsPrincipal principal)
        {
            var rawId = principal.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).FirstOrDefault();
            if (rawId == null)
            {
                return null;
            }
            return Guid.Parse(rawId);
        }

        public static Nullable<User.Roles> GetUserRole(this ClaimsPrincipal principal)
        {
            var rawRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (rawRole == null)
            {
                return null;
            }

            return Enum.Parse<User.Roles>(rawRole);
        }
    }
}