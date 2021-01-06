using Artaplan.MapModels.Users;
using Artaplan.Models;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artaplan.MapModels.Slots;
using Artaplan.MapModels.Stages;
using Artaplan.MapModels.Jobs;
using Artaplan.MapModels.Customers;

namespace Artaplan.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Slot, SlotDTO>();
            CreateMap<SlotDTO, Slot>();
            CreateMap<Stage, StageDTO>();
            CreateMap<StageDTO, Stage>();
            CreateMap<JobDetailedDTO, Job>();
            CreateMap<Job, JobDetailedDTO>();
            CreateMap<JobStageDTO, JobStage>();
            CreateMap<JobStage, JobStageDTO >();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Job, JobDTO>();
            CreateMap<JobDTO, Job>();
        }
    }
}
