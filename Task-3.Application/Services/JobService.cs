using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Application.Interfaces;

namespace Task_3.Application.Services
{
    public class JobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICurrentUserService _currentUserService;

        public JobService(
            IJobRepository jobRepository,
            ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _currentUserService = currentUserService;
        }

        public async Task CloseAsync(int jobId)
        {
            if (_currentUserService.UserId == null)
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");

            if (_currentUserService.Role != "Recruiter")
                throw new UnauthorizedAccessException(
                    "Only recruiters can close jobs.");

            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found.");

            if (job.RecruiterId != _currentUserService.UserId.Value)
                throw new UnauthorizedAccessException(
                    "You can only close your own jobs.");

            if (job.ClosedAt != null)
                throw new InvalidOperationException(
                    "Job is already closed.");

            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = _currentUserService.UserId.Value;

            await _jobRepository.SaveChangesAsync();
        }
    }
}
