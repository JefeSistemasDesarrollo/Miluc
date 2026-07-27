using Microsoft.AspNetCore.Authorization;

namespace Miluc.Server.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permiso { get; }
        public PermissionRequirement(string permiso)
        {
            Permiso = permiso;
        }
    }
}
