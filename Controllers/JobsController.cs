using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artaplan.Helpers;
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

        // POST: api/Jobs
        [HttpPost]
        public async Task<ActionResult<JobDTO>> PostJob(JobDTO jobDTO)
        {
            var job = _mapper.Map<Job>(jobDTO);
            var Addedjob = await _jobService.Create(job);
            return _mapper.Map<JobDTO>(Addedjob);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDTO>> GetJob(int id)
        {
            Job job = await _jobService.GetById(id);
            if (job == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobDTO>(job);
        }

        //GET: api/Jobs
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs()
        {
            var jobs = await _jobService.GetAll();
            if (!jobs.Any())
            {
                return NotFound();
            }
            return _mapper.Map<List<JobDTO>>(jobs);
        }

        //PUT: api/Jobs/1
        [HttpPut("{id}")]
        public async Task<ActionResult<JobDTO>> UpdateJob(int id, JobDTO jobDTO)
        {
            
        }
    }
}
