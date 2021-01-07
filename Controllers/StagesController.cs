using Artaplan.MapModels.Stages;
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
    public class StagesController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly IStageService _stageService;

        public StagesController(
            ArtaplanContext context,
            IMapper mapper,
            IStageService stageService
            )
        {
            _mapper = mapper;
            _context = context;
            _stageService = stageService;
        }

        // GET: api/Stages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StageDTO>>> GetStages()
        {
            var stages = await _stageService.GetAll();
            if (!stages.Any())
            {
                return NotFound();
            }

            return _mapper.Map<List<StageDTO>>(stages);

        }

        // GET: api/Stages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StageDTO>> GetStages(int id)
        {

            var stage = await _stageService.GetById(id);
            if (stage == null)
            {
                return NotFound();
            }

            return _mapper.Map<StageDTO>(stage);
        }

        // POST: api/Stages
        [HttpPost]
        public async Task<ActionResult<StageDTO>> PostStages(StageDTO stageDTO)
        {
            var stage = _mapper.Map<Stage>(stageDTO);
            stage = await _stageService.Create(stage);
            return _mapper.Map<StageDTO>(stage);
        }

        //DELETE: api/Stages
        [HttpDelete("{id}")]
        public async Task<ActionResult<StageDTO>> DeleteStage(int id)
        {
            var stage = await _stageService.GetById(id);
            if (stage == null)
            {
                return NotFound();
            }
            stage = await _stageService.Delete(stage);
            if (stage == null)
            {
                return NotFound();
            }
            return _mapper.Map<StageDTO>(stage);
        }

        //PUT: api/Stages/5
        [HttpPut("{id}")]
        public async Task<ActionResult<StageDTO>> UpdateStage(int id, StageDTO stageDTO)
        {
            var stage = _mapper.Map<Stage>(stageDTO);
            if (stage.StageId != id)
            {
                return BadRequest();
            }
            if (!_context.Stages.Where(x => x == stage).Any())
            {
                await _stageService.Create(stage);
                return _mapper.Map<StageDTO>(stage);
            }
            stage = await _stageService.Update(stage);
            if (stage == null)
            {
                return NotFound();
            }
            return _mapper.Map<StageDTO>(stage);
        }
    }
}
