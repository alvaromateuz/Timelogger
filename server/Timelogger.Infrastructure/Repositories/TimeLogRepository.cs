using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Infrastructure.Repositories
{
    public class TimeLogRepository : ITimeLogRepository
    {
        private readonly TimeloggerContext _context;

        public TimeLogRepository(TimeloggerContext context)
        {
            _context = context;
        }

        public async Task<TimeLog> AddAsync(TimeLog entity)
        {
            var result = await _context.TimeLogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<TimeLog> DeleteAsync(int id)
        {
            var entity = await _context.TimeLogs.FindAsync(id);
            if (entity == null)
            { return null; }

            var result = _context.TimeLogs.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<TimeLog>> GetAllAsync()
        {
            var result = await _context.TimeLogs
                .Include(t => t.Developer)
                .Include(t => t.Project)
                .ThenInclude(p => p.Customer)
                .ToListAsync();
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TimeLog> GetByIdAsync(int id)
        {
            var result = await _context.TimeLogs.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TimeLog> UpdateAsync(TimeLog entity)
        {
            var result = _context.TimeLogs.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
