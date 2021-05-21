using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artaplan.Models;
using Artaplan.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Artaplan.MapModels.Slots;
using Artaplan.Errors;

namespace Artaplan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly ISlotService _slotService;
        private readonly IStageService _stageService;

        public SlotsController(
            ArtaplanContext context,
            IMapper mapper,
            ISlotService slotService,
            IStageService stageService
            )
        {
            _mapper = mapper;
            _context = context;
            _slotService = slotService;
            _stageService = stageService;
        }

        // GET: api/Slots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SlotDTO>>> GetSlots()
        {
            var slots = await _slotService.GetAll();
     
            return _mapper.Map<List<SlotDTO>>(slots);
        }

        // GET: api/Slots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SlotDTO>> GetSlots(int id)
        {

            Slot slot = await _slotService.GetById(id);
            if (slot == null)
            {
                NotFound();
            }
            return _mapper.Map<SlotDTO>(slot); 
        }

        // PUT: api/Slots/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<SlotDTO>> UpdateSlots(int id, SlotDTO slotDTO)
        {
            var slot = _mapper.Map<Slot>(slotDTO);
            if (id != slot.SlotId)
            {
                return BadRequest();
            }
            if(!_context.Slots.Where(x => x.SlotId == slot.SlotId).Any())
            {
                return _mapper.Map<SlotDTO>(await _slotService.Create(slot));
            }
            slot = await _slotService.Update(slot);
            foreach (Stage stage in slot.Stages)
            {
                await _stageService.Update(stage);
            }

            if (slot == null)
            {
                return NotFound();
            }
            return _mapper.Map<SlotDTO>(slot);
        }

        // POST: api/Slots
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SlotDTO>> PostSlots(SlotDTO slotDTO)
        {
            var slot = _mapper.Map<Slot>(slotDTO);
            slot = await _slotService.Create(slot);
            return _mapper.Map<SlotDTO>(slot);
        }

        //DELETE: api/Slots/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<SlotDTO>> DeleteJob(int id)
        {
            var slot = await _slotService.GetById(id);
            if (slot == null)
            {
                return NotFound();
            }
            return _mapper.Map<SlotDTO>(await _slotService.Delete(slot));
        }
    }
}
