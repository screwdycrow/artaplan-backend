using Artaplan.MapModels.Stages;
using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IStageService
    {
        Task<Stage> GetById(int id);
        Task<IEnumerable<Stage>> GetAll();
        Task<Stage> Delete(Stage stage);
        Task<Stage> Update(Stage stage);
        Task<Stage> Create(Stage stage);
    }

    public class StageService : IStageService
    {
        private ArtaplanContext _context;
        private int userId;

        public StageService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            userId = userProvider.GetUserId();
        }

        public async Task<Stage> Create(Stage stage)
        {
            stage.UserId = userId;
            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<Stage> Delete(Stage stage)
        {
            if (stage.UserId != userId)
            {
                return null;
            }
            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<IEnumerable<Stage>> GetAll()
        {
            return await _context.Stages.Where(x => x.UserId == userId).Include(x => x.JobStages).ToListAsync();
        }

        public async Task<Stage> GetById(int id)
        {
            return await _context.Stages.Where(x => x.UserId == userId && x.StageId == id).Include(x => x.JobStages).FirstOrDefaultAsync();
        }

        public async Task<Stage> Update(Stage stage)
        {
            if (stage.UserId != userId)
                return null;
            _context.Entry(stage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return stage;
        }
    }

}
