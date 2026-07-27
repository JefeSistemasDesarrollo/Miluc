using Microsoft.AspNetCore.Authorization;

namespace Miluc.Server.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      PermissionRequirement requirement)
        {
            var permisosUsuario = context.User.FindAll("Permission").Select(x => x.Value);

            if (permisosUsuario.Contains(requirement.Permiso))context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
