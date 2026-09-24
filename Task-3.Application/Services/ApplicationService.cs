using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Application.DTOs;
using Task_3.Application.Interfaces;
using Task_3.Domain.Enums;
using Task_3.Domain.Models;
using Hangfire;
namespace Task_3.Application.Services
{
    public class ApplicationService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly ICurrentUserService _currentUserService;

        public ApplicationService(
            IJobRepository jobRepository,
            IJobApplicationRepository applicationRepository,
            ICurrentUserService currentUserService)
        {
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<int> CreateAsync(CreateApplicationDto dto)
        {
            if (_currentUserService.UserId == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (_currentUserService.Role != "Candidate")
                throw new UnauthorizedAccessException(
                    "Only candidates can apply for jobs.");

            var job = await _jobRepository.GetByIdAsync(dto.JobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found.");

            if (job.ClosedAt != null)
                throw new InvalidOperationException("This job is already closed.");

            var candidateId = _currentUserService.UserId.Value;

            var existingApplication =
                await _applicationRepository
                    .GetByJobAndCandidateAsync(dto.JobId, candidateId);

            if (existingApplication != null)
                throw new InvalidOperationException(
                    "You have already applied for this job.");

            var application = new JobApplication
            {
                JobId = dto.JobId,
                CandidateId = candidateId,
                Status = ApplicationStatus.Applied,
                CreatedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);

            await _applicationRepository.SaveChangesAsync();

            return application.Id;
        }

        public async Task CancelAsync(int id)
        {
            if (_currentUserService.UserId == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            if (_currentUserService.Role != "Candidate")
                throw new UnauthorizedAccessException(
                    "Only candidates can cancel applications.");

            var application =
                await _applicationRepository.GetByIdAsync(id);

            if (application == null)
                throw new KeyNotFoundException("Application not found.");

            if (application.CandidateId != _currentUserService.UserId.Value)
                throw new UnauthorizedAccessException(
                    "You can only cancel your own application.");

            if (application.Status != ApplicationStatus.Applied &&
                application.Status != ApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException(
                    "Application cannot be cancelled in its current status.");
            }

            application.Status = ApplicationStatus.Cancelled;
            application.CancelledAt = DateTime.UtcNow;

            await _applicationRepository.SaveChangesAsync();

            BackgroundJob.Enqueue<INotificationService>(
            x => x.NotifyCandidate(application.Id));
        }
    }
}
