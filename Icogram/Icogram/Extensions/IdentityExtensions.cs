using System;
using System.Security.Claims;
using System.Security.Principal;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity;

namespace Icogram.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserRole(this IIdentity identity)
        {
            if (identity == null)
            {
                return "";
            }
            var ci = identity as ClaimsIdentity;
            var role = "";
            if (ci == null) return role;
            var id = ci.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            if (id != null)
                role = id.Value;

            return role;
        }

        public static bool IsInRole(this IIdentity identity, string userRole)
        {
            if (identity == null)
            {
                return false;
            }
            var ci = identity as ClaimsIdentity;
            var role = "";
            if (ci == null) return false;
            var id = ci.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            if (id != null)
                role = id.Value;

            return userRole == role;
        }
    }
}