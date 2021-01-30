using Artaplan.MapModels.Jobs;
using Artaplan.Models;
using Artaplan.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobStagesController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly IJobStageService _jobStageService;

        public JobStagesController(
            ArtaplanContext context,
            IMapper mapper,
            IJobStageService jobStageService
            )
        {
            _mapper = mapper;
            _context = context;
            _jobStageService = jobStageService;
        }

        // GET: api/JobStages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobStageDetailedDTO>>> GetJobStages()
        {
            var jobStages = await _jobStageService.GetAll();

            return _mapper.Map<List<JobStageDetailedDTO>>(jobStages);

        }

        // GET: api/JobStages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobStageDetailedDTO>> GetJobStages(int id)
        {

            var jobStage = await _jobStageService.GetById(id);
            if (jobStage == null)
            {
                return NotFound();
            }

            return _mapper.Map<JobStageDetailedDTO>(jobStage);
        }

        // POST: api/JobStages
        [HttpPost]
        public async Task<ActionResult<JobStageDetailedDTO>> PostJobStages(JobStageDTO jobStageDTO)
        {
            var jobStage = _mapper.Map<JobStage>(jobStageDTO);
            jobStage = await _jobStageService.Create(jobStage);
            return _mapper.Map<JobStageDetailedDTO>(jobStage);
        }

        //DELETE: api/JobStages
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobStageDetailedDTO>> DeleteJobStage(int id)
        {
            var jobStage = await _jobStageService.GetById(id);
            if (jobStage == null)
            {
                return NotFound();
            }
            jobStage = await _jobStageService.Delete(jobStage);
            if (jobStage == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobStageDetailedDTO>(jobStage);
        }

        //PUT: api/JobStages/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JobStageDetailedDTO>> UpdateJobStage(int id, JobStageDTO jobStageDTO)
        {
            var jobStage = _mapper.Map<JobStage>(jobStageDTO);
            if (jobStage.JobStageId != id)
            {
                return BadRequest();
            }
            if (!_context.JobStages.Where(x => x.JobStageId == jobStage.JobStageId).Any())
            {
                await _jobStageService.Create(jobStage);
                return _mapper.Map<JobStageDetailedDTO>(jobStage);
            }
            jobStage = await _jobStageService.Update(jobStage);
            if (jobStage == null)
            {
                return NotFound();
            }
            return _mapper.Map<JobStageDetailedDTO>(jobStage);
        }
    }
}
