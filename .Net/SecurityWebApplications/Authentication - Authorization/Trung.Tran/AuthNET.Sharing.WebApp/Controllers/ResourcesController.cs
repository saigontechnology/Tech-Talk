using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [Route("api/resources")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ResourcesController(DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AppResourceModel> Get()
        {
            var resources = _dataContext.Resources.ProjectTo<AppResourceModel>(_mapper.ConfigurationProvider)
                .ToArray();

            return resources;
        }
    }
}
