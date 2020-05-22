using Microsoft.AspNetCore.Http;

namespace App.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarRoleUsuario(HttpContext context, string role)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.IsInRole(role);
        }
    }
}
