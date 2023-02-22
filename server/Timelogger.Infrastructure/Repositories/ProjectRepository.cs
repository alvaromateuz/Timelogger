using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimeloggerContext _context;

        public ProjectRepository(TimeloggerContext context)
        {
            _context = context;
        }

        public async Task<Project> AddAsync(Project entity)
        {
            var result = await _context.Projects.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Project> DeleteAsync(int id)
        {
            var entity = await _context.Projects.FindAsync(id);
            if (entity == null)
            { return null; }

            var result = _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var result = await _context.Projects
                .Include(t => t.Customer)
                .Include(t => t.ProjectStage)
                .ToListAsync();
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            var result = await _context.Projects.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Project> UpdateAsync(Project entity)
        {
            var result = _context.Projects.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
