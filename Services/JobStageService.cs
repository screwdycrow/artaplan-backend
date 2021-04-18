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
        Task<JobStage> UpdateWorkHours(int id);
        Task<JobStage> UpdateJobHours(int id, int Hours);
    }

    public class JobStageService : IJobStageService
    {
        private ArtaplanContext _context;
        private int userId;
        private IScheduleEntryService _scheduleEntryService;
        public JobStageService(ArtaplanContext context, IUserProvider userProvider, IScheduleEntryService scheduleEntryService)
        {
            _context = context;
            userId = userProvider.GetUserId();
            _scheduleEntryService = scheduleEntryService;
        }
        public async Task<JobStage> Create(JobStage jobStage)
        {
            if (!await BelongsToUser(jobStage))
                return null;
            jobStage.JobStageId = 0;
            _context.JobStages.Add(jobStage);
            await _context.SaveChangesAsync();
            return jobStage;
        }

        

        public async Task<JobStage> Delete(JobStage jobStage)
        {
            if (!await BelongsToUser(jobStage))
                return null;
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
            return await _context.JobStages.Where(x => x.Job.UserId == userId).Include(x => x.Job).Include(x => x.Stage).Include(x => x.ScheduleEntries).ToListAsync();
        }

        public async Task<JobStage> GetById(int id)
        {
            return await _context.JobStages.Where(x => x.JobStageId == id && x.Job.UserId == userId).Include(x => x.Job).Include(x => x.Stage).Include(x => x.ScheduleEntries).FirstOrDefaultAsync();
        }

        public async Task<JobStage> Update(JobStage jobStage)
        {
            if (!await BelongsToUser(jobStage))
                return null;
            _context.Entry(jobStage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return jobStage;
        }

        public async Task<JobStage> UpdateJobHours(int id,int jobHours)
        {

            JobStage jobstage = new JobStage() { JobStageId = id, JobHours = jobHours };
            _context.JobStages.Attach(jobstage);
            _context.Entry(jobstage).Property(js => js.JobHours).IsModified = true;
            await _context.SaveChangesAsync();
            return await GetById(id);
        }

        public async Task<JobStage> UpdateWorkHours(int id)
        {
            List<ScheduleEntry> entries = await _scheduleEntryService.GetByJobStageId(id, true);
            JobStage js = await GetById(id);
            js.WorkHours = entries.Count;
          
            return await Update(js);

        }

        private async Task<bool> BelongsToUser(JobStage jobStage)
        {
            var job = await _context.Jobs.Where(x => x.JobId == jobStage.JobId && x.UserId == userId).FirstOrDefaultAsync();
            var stage = await _context.Stages.Where(x => x.StageId == jobStage.StageId && x.UserId == userId).FirstOrDefaultAsync();
            return !(job == null || stage == null);
        }
    }
}
