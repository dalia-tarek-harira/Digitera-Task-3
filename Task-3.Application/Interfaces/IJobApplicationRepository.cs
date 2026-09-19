using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Domain.Models;

namespace Task_3.Application.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<JobApplication?> GetByIdAsync(int id);

        Task<JobApplication?> GetByJobAndCandidateAsync(
            int jobId,
            int candidateId);

        Task AddAsync(JobApplication application);

        Task SaveChangesAsync();
    }
}
