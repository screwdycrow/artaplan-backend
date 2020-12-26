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
        Task<Job> Update(Job slot);
        Task<Job> Create(Job slot);
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
            try
            {
                var jobs = await _context.Jobs
                    .Where(s => s.UserId == userId)
                    .Include(j => j.JobStages)
                    .ToListAsync();
                return jobs;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public async Task<Job> GetById(int id)
        {
            try
            {
                var job = await _context.Jobs
                   .Where(s => s.JobId == id)
                   .Where(s => s.UserId == userId)
                   .Include(s => s.JobStages)
                   .Include(s => s.Slot)
                   .FirstAsync();
                return job;
            }
            catch (Exception e)
            {
                throw (e);
            }

        }

        public Task<Job> Update(Job slot)
        {
            throw new NotImplementedException();
        }
    }
}
