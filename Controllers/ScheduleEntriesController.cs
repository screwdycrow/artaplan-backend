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
    public class ScheduleEntriesController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly IScheduleEntryService _scheduleEntryService;
        private readonly IJobStageService _jobStageService;

        public ScheduleEntriesController(
            ArtaplanContext context,
            IMapper mapper,
            IScheduleEntryService scheduleEntryService,
            IJobStageService jobStageService
            )
        {
            _mapper = mapper;
            _context = context;
            _scheduleEntryService = scheduleEntryService;
            _jobStageService = jobStageService;
        }

        // GET: api/ScheduleEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleEntryDetailedDTO>>> GetScheduleEntries()
        {
            var scheduleEntries = await _scheduleEntryService.GetAll();

            return _mapper.Map<List<ScheduleEntryDetailedDTO>>(scheduleEntries);

        }

        // GET: api/ScheduleEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleEntryDetailedDTO>> GetScheduleEntries(int id)
        {

            var scheduleEntry = await _scheduleEntryService.GetById(id);
            if (scheduleEntry == null)
            {
                return NotFound();
            }

            return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);
        }

        // POST: api/ScheduleEntries
        [HttpPost]
        public async Task<ActionResult<ScheduleEntryDetailedDTO>> PostScheduleEntries(ScheduleEntryDTO scheduleEntryDTO)
        {
            var scheduleEntry = _mapper.Map<ScheduleEntry>(scheduleEntryDTO);
            scheduleEntry = await _scheduleEntryService.Create(scheduleEntry);
            if (scheduleEntry == null)
            {
                return NotFound();
            }
            return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);

        }


        //DELETE: api/ScheduleEntries
        [HttpDelete("{id}")]
        public async Task<ActionResult<ScheduleEntryDetailedDTO>> DeleteScheduleEntry(int id)
        {
            var scheduleEntry = await _scheduleEntryService.GetById(id);
            if (scheduleEntry == null)
            {
                return NotFound();
            }
            scheduleEntry = await _scheduleEntryService.Delete(scheduleEntry);
            await _jobStageService.UpdateWorkHours(scheduleEntry.JobStageId);
            if (scheduleEntry == null)
            {
                return NotFound();
            }
            return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);
        }
        [HttpPatch("{id}/status/{isDone}")]
        //Patch: api/ScheduleEntries/5/Done
        public async Task<ActionResult<ScheduleEntryDetailedDTO>> SetDone(int id, string isDone)
        {
            var scheduleEntry = await _scheduleEntryService.setDone(id, (isDone == "done") ? true : false); 
            await _jobStageService.UpdateWorkHours(scheduleEntry.JobStageId);
            return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);
        }

        //PUT: api/ScheduleEntries/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduleEntryDetailedDTO>> UpdateScheduleEntry(int id, ScheduleEntryDTO scheduleEntryDTO)
        {
            var scheduleEntry = _mapper.Map<ScheduleEntry>(scheduleEntryDTO);
            if (scheduleEntry.ScheduleEntriesId != id)
            {
                return BadRequest();
            }
            if (!_context.ScheduleEntries.Where(x => x.ScheduleEntriesId == scheduleEntry.ScheduleEntriesId).Any())
            {
                await _scheduleEntryService.Create(scheduleEntry);
                return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);
            }
            scheduleEntry = await _scheduleEntryService.Update(scheduleEntry);
            if (scheduleEntry == null)
            {
                return NotFound();
            }
            return _mapper.Map<ScheduleEntryDetailedDTO>(scheduleEntry);
        }
    }
}
