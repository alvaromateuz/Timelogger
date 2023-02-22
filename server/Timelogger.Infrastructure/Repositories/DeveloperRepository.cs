using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Infrastructure.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly TimeloggerContext _context;

        public DeveloperRepository(TimeloggerContext context)
        {
            _context = context;
        }

        public async Task<Developer> AddAsync(Developer entity)
        {
            var result = await _context.Developers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Developer> DeleteAsync(int id)
        {
            var entity = await _context.Developers.FindAsync(id);
            if (entity == null)
            { return null; }

            var result = _context.Developers.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Developer>> GetAllAsync()
        {
            var result = await _context.Developers.ToListAsync();
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Developer> GetByIdAsync(int id)
        {
            var result = await _context.Developers.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Developer> UpdateAsync(Developer entity)
        {
            var result = _context.Developers.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
