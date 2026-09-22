using MediatR;
using Microsoft.EntityFrameworkCore;
using Task_3.Domain.Entities;
using Task_3.Infrastructure.Persistence;

namespace Task_3.Application.Features.Jobs.Commands.CloseJob
{
    public class CloseJobCommandHandler : IRequestHandler<CloseJobCommand, Unit>
    {
        private readonly AppDbContext _db;

        public CloseJobCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(
            CloseJobCommand request,
            CancellationToken cancellationToken)
        {
            var job = await _db.Jobs
                .FirstOrDefaultAsync(
                    j => j.Id == request.JobId,
                    cancellationToken);

            if (job == null)
            {
                throw new KeyNotFoundException("Job not found.");
            }

            job.IsClosed = true;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}