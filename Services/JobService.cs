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
        Task<Job> Delete(Job job);
        Task<Job> Update(Job job);
        Task<Job> Create(Job job);
    }
    public class JobService : IJobService
    {
        private ArtaplanContext _context;
        private IUserProvider _userProvider;
        private int userId;
        private ICustomerService _customerService;
        public JobService(ArtaplanContext context, IUserProvider userProvider, ICustomerService customerService)
        {
            _context = context;
            _userProvider = userProvider;
            _customerService = customerService;
            userId = userProvider.GetUserId();
        }
        public async Task<Job> Create(Job job)
        {
            job.UserId = userId;

            //add customer if not found
            var customer = await _customerService.GetById(job.CustomerId);
            job.Slot = null;
            if (customer != null)
            {
                job.Customer = null;
            }
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
