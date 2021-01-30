using Artaplan.MapModels.Jobs;
using Artaplan.Models;
using Artaplan.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IJobStageService _jobStageService;

        public JobsController(
            ArtaplanContext context,
            IMapper mapper,
            IJobService jobService,
            IJobStageService jobStageService
            )
        {
            _mapper = mapper;
            _context = context;
            _jobService = jobService;
            _jobStageService = jobStageService;
        }

        // POST: api/Jobs
        [HttpPost]
        public async Task<ActionResult<JobDetailedDTO>> PostJob(JobDTO jobDTO)
        {
            var job = _mapper.Map<Job>(jobDTO);
            var Addedjob = await _jobService.Create(job);
            return _mapper.Map<JobDetailedDTO>(Addedjob);
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDetailedDTO>> GetJob(int id)
        {
            Job job = await _jobService.GetById(id);
            if (job == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobDetailedDTO>(job);
        }

        //GET: api/Jobs
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<JobDetailedDTO>>> GetJobs()
        {
            var jobs = await _jobService.GetAll();
   
            return _mapper.Map<List<JobDetailedDTO>>(jobs);
        }

        //PUT: api/Jobs/1
        [HttpPut("{id}")]
        public async Task<ActionResult<JobDetailedDTO>> UpdateJob(int id, JobDTO jobDTO)
        {
            var job = _mapper.Map<Job>(jobDTO);
            if (job.JobId != id)
            {
                return BadRequest();
            }
            if (!_context.Jobs.Where(x => x.JobId == job.JobId).Any())
            {
                await _jobService.Create(job);
                return _mapper.Map<JobDetailedDTO>(job);
            }
            try
            {
                job = await _jobService.Update(job);
            }
            catch
            {
                return BadRequest();
            }
            if (job == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobDetailedDTO>(job);
        }
        
        //DELETE: api/Jobs/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobDetailedDTO>> DeleteJob(int id)
        {
            var job = await _jobService.GetById(id);
            if(job == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobDetailedDTO>(await _jobService.Delete(job));
        }

        //GET: api/Jobs/1/scheduleentries
        [HttpGet("{id}/scheduleentries")]
        public async Task<ActionResult<IEnumerable<ScheduleEntryDetailedDTO>>> GetScheduleEntries(int id)
        {
            HashSet<ScheduleEntry> entries = new HashSet<ScheduleEntry>();  
            var job = await _jobService.GetById(id);
            foreach (var stage in job.JobStages)
             {
                var updatedStage = await _jobStageService.GetById(stage.JobStageId);
                foreach (var entry in updatedStage.ScheduleEntries)
                {
                    entries.Add(entry);
                }
            }
            return _mapper.Map<List<ScheduleEntryDetailedDTO>>(entries);
        }
    }
}
