

using Microsoft.AspNetCore.RateLimiting;
using SurvayBasket.Contracts.Roles;
using SurvayBasket.Service.RolesService;

namespace SurvayBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Admin)]
    [EnableRateLimiting("IPLimiter")]
    public class RolesController(IRoleService roleService, RoleManager<ApplicationRole> roleManager) : ControllerBase
    {
        private readonly IRoleService roleService = roleService;
        private readonly RoleManager<ApplicationRole> roleManager = roleManager;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
           => Ok(await roleService.GetAll(cancellationToken));

        [HttpGet("active")]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
          => Ok(await roleService.GetActive(cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id, CancellationToken cancellationToken)
        {
            var role = await roleService.GetById(id, cancellationToken);

            return role is null ? NotFound(new APIErrorResponse(400)) : Ok(role);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddRoleRequest roleRequest, CancellationToken cancellationToken)
        {
            var result = await roleManager.RoleExistsAsync(roleRequest.Name);
            if (result)
                return BadRequest(new APIErrorResponse(400, "You can not create duplicated role!"));
            var NewRole = new ApplicationRole()
            {
                Name = roleRequest.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var IsSucceeded = await roleManager.CreateAsync(NewRole);

            return IsSucceeded.Succeeded ? NoContent() : BadRequest();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> update([FromRoute] string id, UpdateRoleRequest roleRequest, CancellationToken cancellationToken)
        {
            var result = await roleManager.RoleExistsAsync(roleRequest.Name);
            if (result)
                return BadRequest(new APIErrorResponse(400, "You can not create duplicated role!"));

            var role = await roleManager.FindByIdAsync(id);
            if (role is not { })
                return BadRequest(new APIErrorResponse(404));

            role.Name = roleRequest.Name;
            var IsUpdated = await roleManager.UpdateAsync(role);
            return IsUpdated.Succeeded ? NoContent() : BadRequest();

        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
        {

            var role = await roleManager.FindByIdAsync(id);
            if (role is not { })
                return BadRequest(new APIErrorResponse(404));

            role.IsDeleted = !role.IsDeleted;
            var IsUpdated = await roleManager.UpdateAsync(role);
            return IsUpdated.Succeeded ? NoContent() : BadRequest();

        }

    }
}
