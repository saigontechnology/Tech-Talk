using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.Resource;
using TAuth.ResourceAPI.Auth.Policies;
using TAuth.ResourceAPI.Entities;


namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly ResourceContext _context;
        private readonly IAuthorizationService _authorizationService;

        public ResourcesController(ResourceContext context,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<ResourceListItemModel>> Get()
        {
            var list = await _context.Resources.Select(o => new ResourceListItemModel
            {
                Id = o.Id,
                Name = o.Name
            }).ToArrayAsync();

            return list;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Resources.Where(o => o.Id == id)
                .Select(o => new ResourceDetailModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).SingleOrDefaultAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        //[Authorize(PolicyNames.IsLucky)]
        public async Task<int> Post([FromBody] CreateResourceModel model)
        {
            var entity = new ResourceEntity
            {
                Name = model.Name
            };

            await _context.Resources.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Resources.IgnoreQueryFilters()
                .Where(o => o.Id == id).Select(o => new ResourceEntity
                {
                    Id = o.Id,
                    Name = o.Name,
                    OwnerId = o.OwnerId
                }).SingleOrDefaultAsync();

            if (item == null)
                return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, new ResourceAuthorizationModel
            {
                Name = item.Name,
                OwnerId = item.OwnerId
            }, PolicyNames.Resource.CanDeleteResource);

            if (!authResult.Succeeded) return Forbid();

            _context.Resources.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
