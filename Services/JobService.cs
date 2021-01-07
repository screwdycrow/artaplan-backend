using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IJobService
    {
        Task<Job> GetById(int id);

        Task<IEnumerable<Job>> GetAll();
        Task<Job> Delete(Job job);
        Task<Job> Update(Job job);
        Task<Job> Create(Job job);
    }
    public class JobService : IJobService
    {
        private ArtaplanContext _context;
        private int userId;
        public JobService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            userId = userProvider.GetUserId();
        }
        public async Task<Job> Create(Job job)
        {
            job.UserId = userId;
            job.JobId = 0;
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public async Task<Job> Delete(Job job)
        {
            if(job.UserId != userId)
            {
                return null;
            }
            try
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
                return job;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Job>> GetAll()
        {
            var jobs = await _context.Jobs
                .Where(j => j.UserId == userId)
                .Include(j => j.JobStages)
                .Include(j => j.Slot)
                .Include(j => j.Customer)
                .ToListAsync();
            return jobs;
        }

        public async Task<Job> GetById(int id)
        {
                var job = await _context.Jobs
                   .Where(j => j.JobId == id)
                   .Where(j => j.UserId == userId)
                   .Include(j => j.JobStages)
                   .Include(j => j.Slot)
                   .Include(j => j.Customer)
                   .FirstOrDefaultAsync();
                return job;

        }

        public async Task<Job> Update(Job job)
        {
            if(job.UserId != userId)
            {
                return null;
            }
            _context.Entry(job).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return job;
        }
    }
}
