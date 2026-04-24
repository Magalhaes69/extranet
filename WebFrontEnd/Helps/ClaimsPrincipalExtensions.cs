using System.Security.Claims;

namespace WebFrontEnd.Helps
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetCardCode(this ClaimsPrincipal user)
        {
            return user.FindFirst("CardCode")?.Value ?? string.Empty;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public static string GetUserGuid(this ClaimsPrincipal user)
        {
            return user.FindFirst("UserId")?.Value ?? string.Empty;
        }
    }
}
