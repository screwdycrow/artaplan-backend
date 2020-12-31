using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artaplan.Models;
using Microsoft.EntityFrameworkCore;

namespace Artaplan.Services
{
    public interface ISlotService
    {
        Task<Slot> GetById(int id);

        Task<IEnumerable<Slot>> GetAll();
        void Delete();
        Task<Slot> Update(Slot slot);
        Task<Slot> Create(Slot slot);
    }
    public class SlotService : ISlotService
    {
        private ArtaplanContext _context;
        //Todo: userprovider + userId? 
        private IUserProvider _userProvider;
        private int userId;
        public SlotService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
            userId = userProvider.GetUserId();
        }

        public  async Task<Slot> Create(Slot slot)
        {
            slot.UserId = userId;
            foreach (Stage stage in slot.Stages)
            {
                stage.UserId = userId;
            }
            _context.Slots.Add(slot);
            await  _context.SaveChangesAsync();
            return slot;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Slot>> GetAll()
        {
            var slots = await _context.Slots
                .Where(s => s.UserId == userId)
                .Include(s=> s.Stages)
                .ToListAsync();
            return slots;
        
        }

        public async Task<Slot> GetById(int id)
        {
            var slot = await _context.Slots
                .Include(s => s.Stages)
                .Where(s => s.UserId == userId)
                .Where(s => s.SlotId == id)
                .FirstAsync();
            return slot;
       
        }

        public Task<Slot> Update(Slot slot)
        {
            throw new NotImplementedException();
        }
    }
}
