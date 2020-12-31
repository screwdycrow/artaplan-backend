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
        private readonly IUserProvider _userProvider;
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly ISlotService _slotService;

        public SlotsController(
            ArtaplanContext context,
            IUserProvider userProvider,
            IMapper mapper,
            ISlotService slotService
            )
        {
            _mapper = mapper;
            _userProvider = userProvider;
            _context = context;
            _slotService = slotService;
        }

        // GET: api/Slots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slot>>> GetSlots()
        {
            try
            {
             var slots = await _slotService.GetAll();
                return Ok(_mapper.Map<List<SlotDTO>>(slots));

            }catch(Exception)
            {
                return BadRequest(ErrorMessage.ShowErrorMessage(Error.InternalServerError));
            }

        }

        // GET: api/Slots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SlotDTO>> GetSlots(int id)
        {

            Slot slot = await _slotService.GetById(id);
            if (slot == null)
            {
                return NotFound(ErrorMessage.ShowErrorMessage(Error.SlotNotFound));
            }

            return _mapper.Map<SlotDTO>(slot); 
        }

        // PUT: api/Slots/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        //Todo: use update from service possibly?
        public async Task<ActionResult<SlotDTO>> PutSlots(int id, Slot slot)
        {
            if (id != slot.SlotId)
            {
                return BadRequest(ErrorMessage.ShowErrorMessage(Error.NonMatchingId));
            }

            _context.Entry(slot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlotsExists(id))
                {
                    return NotFound(ErrorMessage.ShowErrorMessage(Error.SlotNotFound));
                }
                else
                {
                    throw;
                }
            }

            return _mapper.Map<SlotDTO>(slot);
        }

        // POST: api/Slots
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SlotDTO>> PostSlots(Slot slot)
        {
            slot = await _slotService.Create(slot);
            return _mapper.Map<SlotDTO>(slot);
        }

        // DELETE: api/Slots/5
        [HttpDelete("{id}")]
        //Todo: use delete from the service lul
        public async Task<ActionResult<Slot>> DeleteSlots(int id)
        {
            var slots = await _context.Slots.FindAsync(id);
            if (slots == null)
            {
                return NotFound();
            }

            _context.Slots.Remove(slots);
            await _context.SaveChangesAsync();

            return slots;
        }

        private bool SlotsExists(int id)
        {
            return _context.Slots.Any(e => e.SlotId == id);
        }
    }
}
