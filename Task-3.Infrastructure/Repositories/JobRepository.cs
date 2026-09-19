using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_3.Application.Interfaces;
using Task_3.Domain.Models;
using Task_3.Infrastructure.Data;

namespace Task_3.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
