using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TimeloggerContext _context;

        public CustomerRepository(TimeloggerContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            var result = await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer> DeleteAsync(int id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity == null)
            { return null; }

            var result = _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var result = await _context.Customers.ToListAsync();
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var result = await _context.Customers.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            var result = _context.Customers.Update(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
