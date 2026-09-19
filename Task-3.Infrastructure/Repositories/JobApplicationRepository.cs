using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Task_3.Application.Interfaces;
using Task_3.Domain.Models;
using Task_3.Infrastructure.Data;

namespace Task_3.Infrastructure.Repositories
{
    public class JobApplicationRepository
    : IJobApplicationRepository
    {
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JobApplication?> GetByJobAndCandidateAsync(
            int jobId,
            int candidateId)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(x =>
                    x.JobId == jobId &&
                    x.CandidateId == candidateId);
        }

        public async Task AddAsync(JobApplication application)
        {
            await _context.JobApplications.AddAsync(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
