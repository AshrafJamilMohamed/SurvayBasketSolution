using SurvayBasket.Contracts.Roles;

namespace SurvayBasket.Service.RolesService
{
    public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;

        public async Task<IReadOnlyList<RolesResponse>> GetActive(CancellationToken cancellationToken = default)
        {
            var Roles = await roleManager.Roles
                  .Where(x => !x.IsDefault && !x.IsDeleted)
                  .Select(r => new RolesResponse(r.Id, r.Name!, r.IsDeleted))
                  .ToListAsync(cancellationToken);
            return Roles;
        }

        public async Task<IReadOnlyList<RolesResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var Roles = await roleManager.Roles
                  .Where(x => !x.IsDefault)
                  .Select(r => new RolesResponse(r.Id, r.Name!, r.IsDeleted))
                  .ToListAsync(cancellationToken);
            return Roles;
        }

        public async Task<RolesResponse?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role is not { }) return null;

            return new RolesResponse(role.Id, role.Name!, role.IsDeleted);
        }
    }
}
