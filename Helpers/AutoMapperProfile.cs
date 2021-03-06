﻿using Artaplan.MapModels.Users;
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
            CreateMap<Slot, SlotSummary>();
            CreateMap<SlotSummary, Slot>();
            CreateMap<Stage, StageDTO>();
            CreateMap<StageDTO, Stage>();
            CreateMap<StageSummary, Stage>();
            CreateMap<Stage, StageSummary>();
            CreateMap<JobDetailedDTO, Job>();
            CreateMap<Job, JobDetailedDTO>();
            CreateMap<JobStageDetailedDTO, JobStage>();
            CreateMap<JobStage, JobStageDetailedDTO>();
            CreateMap<JobStageDTO, JobStage>();
            CreateMap<JobStageSummary, JobStage>();
            CreateMap<JobStage, JobStageSummary>();
            CreateMap<JobStage, JobStageDTO>();
            CreateMap<JobSummary, Job>();
            CreateMap<Job, JobSummary>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Job, JobDTO>();
            CreateMap<JobDTO, Job>();
            CreateMap<ScheduleEntry, ScheduleEntryDTO>();
            CreateMap<ScheduleEntryDTO, ScheduleEntry>();
            CreateMap<ScheduleEntry, ScheduleEntryDetailedDTO>();
            CreateMap<ScheduleEntryDetailedDTO, ScheduleEntry>();
 
        }
    }
}
