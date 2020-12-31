using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IJobService
    {
        Task<Job> GetById(int id);

        Task<IEnumerable<Job>> GetAll();
        void Delete();
        Task<Job> Update(Job job);
        Task<Job> Create(Job job);
    }
    public class JobService : IJobService
    {
        private ArtaplanContext _context;
        private IUserProvider _userProvider;
        private int userId;
        public JobService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
            userId = userProvider.GetUserId();
        }
        public async Task<Job> Create(Job job)
        {
            job.UserId = userId;
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;

        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Job>> GetAll()
        {
            var jobs = await _context.Jobs
                .Where(s => s.UserId == userId)
                .Include(j => j.JobStages)
                .ToListAsync();
            return jobs;
        }

        public async Task<Job> GetById(int id)
        {
                var job = await _context.Jobs
                   .Where(s => s.JobId == id)
                   .Where(s => s.UserId == userId)
                   .Include(s => s.JobStages)
                   .Include(s => s.Slot)
                   .FirstOrDefaultAsync();
                return job;

        }

        public Task<Job> Update(Job slot)
        {
            throw new NotImplementedException();
        }
    }
}
