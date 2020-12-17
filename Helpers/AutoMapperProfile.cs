using Artaplan.MapModels.Users;
using Artaplan.Models;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artaplan.MapModels.Slots;
using Artaplan.MapModels.Stages;

namespace Artaplan.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<User, UserDTO>();
            CreateMap< UserDTO, User>();
            CreateMap<Slot, SlotDTO>();
            CreateMap<SlotDTO, Slot>();
            CreateMap<Stage, StageDTO>();
            CreateMap<StageDTO,Stage>();
        }
    }
}
