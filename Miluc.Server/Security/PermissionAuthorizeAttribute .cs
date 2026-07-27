
using Microsoft.AspNetCore.Authorization;

namespace Miluc.Server.Security
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {

        public PermissionAuthorizeAttribute(string permiso)
        {
            Policy = $"Permission:{permiso}";
        }

    }
}
