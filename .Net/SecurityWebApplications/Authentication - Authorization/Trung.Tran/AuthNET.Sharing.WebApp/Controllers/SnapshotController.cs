using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Controllers
{
    [Route("api/snapshot")]
    [ApiController]
    public class SnapshotController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public SnapshotController(DataContext dataContext,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public DataSnapshotModel Get()
        {
            var snapshot = new DataSnapshotModel();

            snapshot.Users = _dataContext.Users.ProjectTo<AppUserModel>(_mapper.ConfigurationProvider)
                .ToArray();

            return snapshot;
        }
    }
}
