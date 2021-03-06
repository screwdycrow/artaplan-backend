﻿using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IScheduleEntryService
    {
        Task<ScheduleEntry> GetById(int id);

        Task<IEnumerable<ScheduleEntry>> GetAll();
        Task<ScheduleEntry> Delete(ScheduleEntry scheduleEntry);
        Task<ScheduleEntry> Update(ScheduleEntry scheduleEntry);
        Task<ScheduleEntry> Create(ScheduleEntry scheduleEntry);
        Task<List<ScheduleEntry>> GetByJobStageId(int id,bool isDone);
        Task<ScheduleEntry> setDone(int id, bool isDone);

    }

    public class ScheduleEntryService : IScheduleEntryService
    {
        private ArtaplanContext _context;
        private IJobStageService _jobStageService;
        private int userId;
        public ScheduleEntryService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            userId = userProvider.GetUserId();
        }
        public async Task<ScheduleEntry> Create(ScheduleEntry scheduleEntry)
        {
            scheduleEntry.UserId = userId;
            scheduleEntry.ScheduleEntriesId = 0;
            _context.ScheduleEntries.Add(scheduleEntry);
            await _context.SaveChangesAsync();
            scheduleEntry = await GetById(scheduleEntry.ScheduleEntriesId);
            return scheduleEntry;
        }

        public async Task<ScheduleEntry> Delete(ScheduleEntry scheduleEntry)
        {
            if (userId != scheduleEntry.UserId)
            {
                return null;
            }
            try
            {
                _context.ScheduleEntries.Remove(scheduleEntry);
                await _context.SaveChangesAsync();
                return scheduleEntry;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<ScheduleEntry>> GetAll()
        {
            return await _context.ScheduleEntries.Where(x => x.UserId == userId)
                .Include(x => x.JobStage)
                .ThenInclude(js=>js.Stage)
                .Include(x => x.JobStage)
                .ThenInclude(js => js.Job)
                .ToListAsync();
        }

        public async Task<List<ScheduleEntry>> GetByJobStageId(int id,bool isDone)
        {
            return await _context.ScheduleEntries
                .Where(x => x.JobStageId == id && x.UserId == userId && x.IsDone == isDone)
                .ToListAsync();
        }
        public async Task<ScheduleEntry> GetById(int id)
        {
            return await _context.ScheduleEntries.Where(x => x.ScheduleEntriesId == id && x.UserId == userId)
                .Include(x => x.JobStage)
                .ThenInclude(js => js.Stage)
                .Include(x => x.JobStage)
                .ThenInclude(js => js.Job)
                .FirstOrDefaultAsync();
        }

        public async Task<ScheduleEntry> setDone(int id, bool isDone)
        {
            var scheduleEntry = await GetById(id);
            scheduleEntry.IsDone = isDone;
            ScheduleEntry entry = await Update(scheduleEntry);
            return entry;
        }

        public async Task<ScheduleEntry> Update(ScheduleEntry scheduleEntry)
        {
            if (scheduleEntry.UserId != userId)
            {
                return null;
            }
            _context.Entry(scheduleEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            scheduleEntry = await GetById(scheduleEntry.ScheduleEntriesId);
           
            return scheduleEntry;
        }
    }
}
