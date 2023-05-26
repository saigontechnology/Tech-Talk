using AuthNET.Sharing.WebApp.Models;
using AuthNET.Sharing.WebApp.Persistence;
using AutoMapper;
using System.Linq;

namespace AuthNET.Sharing.WebApp.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<AppResource, AppResourceModel>()
                .ForMember(e => e.UserName, opt => opt.MapFrom(e => e.User.UserName));

            CreateMap<AppUser, AppUserModel>()
                .ForMember(e => e.Roles, opt => opt.MapFrom(e => e.UserRoles.Select(ur => ur.Role.Name).ToList()))
                .ForMember(e => e.PermissionGroups, opt => opt.MapFrom(e => e.UserPermissionGroups.Select(ur => ur.PermissionGroup.GroupName).ToList()))
                .ForMember(e => e.UserPermissions, opt => opt.MapFrom(e => e.UserPermissions.Select(ur => ur.Permission).ToList()))
                .ForMember(e => e.GroupPermissions, opt => opt.MapFrom(e => e.UserPermissionGroups
                    .SelectMany(ur => ur.PermissionGroup.Records.Select(record => record.Permission)).ToList()));
        }
    }
}
