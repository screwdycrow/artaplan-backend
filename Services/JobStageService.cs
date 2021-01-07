using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IJobStageService
    {
        Task<JobStage> GetById(int id);

        Task<IEnumerable<JobStage>> GetAll();
        Task<JobStage> Delete(JobStage jobStage);
        Task<JobStage> Update(JobStage jobStage);
        Task<JobStage> Create(JobStage jobStage);
    }

    public class JobStageService : IJobStageService
    {
        private ArtaplanContext _context;
        private int userId;
        public JobStageService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            userId = userProvider.GetUserId();
        }
        public async Task<JobStage> Create(JobStage jobStage)
        {
            jobStage.Job.UserId = userId;
            _context.JobStages.Add(jobStage);
            await _context.SaveChangesAsync();
            return jobStage;
        }

        public async Task<JobStage> Delete(JobStage jobStage)
        {
            if (userId != jobStage.Job.UserId)
            {
                return null;
            }
            try
            {
                _context.JobStages.Remove(jobStage);
                await _context.SaveChangesAsync();
                return jobStage;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<JobStage>> GetAll()
        {
            return await _context.JobStages.Where(x => x.Job.UserId == userId).ToListAsync();
        }

        public async Task<JobStage> GetById(int id)
        {
            return await _context.JobStages.Where(x => x.JobStageId == id && x.Job.UserId == userId).Include(x => x.Job).Include(x => x.Stage).FirstOrDefaultAsync();
        }

        public async Task<JobStage> Update(JobStage jobStage)
        {
            if (jobStage.Job.UserId != userId)
            {
                return null;
            }
            _context.Entry(jobStage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return jobStage;
        }
    }
}
