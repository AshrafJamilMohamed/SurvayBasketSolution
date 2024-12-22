using SurvayBasket.Contracts.Roles;

namespace SurvayBasket.Service.RolesService
{
    public interface IRoleService
    {
        public Task<IReadOnlyList<RolesResponse>> GetAll(CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<RolesResponse>> GetActive(CancellationToken cancellationToken = default);
        public Task<RolesResponse?> GetById(string id, CancellationToken cancellationToken = default);

    }
}
