using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Mapping_Profiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel,IdentityRole>()
                .ForMember(d => d.Name, O => O.MapFrom(s => s.RoleName))
                .ReverseMap();
            //d=>distnation   o=>options    s=>source

        }
    }
}
