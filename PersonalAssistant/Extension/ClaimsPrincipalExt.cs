using IdentityModel;
using System.Security.Claims;

namespace PersonalAssistant.Extension
{
    public static class ClaimsPrincipalExt
    {
        /// <summary>
        /// Get subject id
        /// </summary>
        public static string GetSID(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;
        }
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(x => x.Type.Equals(JwtClaimTypes.Email))?.Value;
        }
    }
}
