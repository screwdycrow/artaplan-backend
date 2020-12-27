using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artaplan.MapModels.Jobs;
using Artaplan.Models;
using Artaplan.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Artaplan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
       
        public JobsController(
            ArtaplanContext context,
            IUserProvider userProvider,
            IMapper mapper,
            IJobService jobService
            )
        {
            _mapper = mapper;
            _userProvider = userProvider;
            _context = context;
            _jobService = jobService;
        }

        public async Task<ActionResult<JobDTO>> PostJob(JobDTO jobDTO)
        {
            var job = _mapper.Map<Job>(jobDTO);
            var Addedjob = await _jobService.Create(job);
            return _mapper.Map<JobDTO>(job);
        }
    }
}
