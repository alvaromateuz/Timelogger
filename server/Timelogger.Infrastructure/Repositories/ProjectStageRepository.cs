using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Infrastructure.Repositories
{
    public class ProjectStageRepository : IProjectStageRepository
    {
        private readonly TimeloggerContext _context;

        public ProjectStageRepository(TimeloggerContext context)
        {
            _context = context;
        }

        public async Task<ProjectStage> AddAsync(ProjectStage entity)
        {
            var result = await _context.ProjectStages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ProjectStage> DeleteAsync(int id)
        {
            var entity = await _context.ProjectStages.FindAsync(id);
            if (entity == null)
            { return null; }

            var result = _context.ProjectStages.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<ProjectStage>> GetAllAsync()
        {
            var result = await _context.ProjectStages.ToListAsync();
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ProjectStage> GetByIdAsync(int id)
        {
            var result = await _context.ProjectStages.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ProjectStage> UpdateAsync(ProjectStage entity)
        {
            var result = _context.ProjectStages.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
