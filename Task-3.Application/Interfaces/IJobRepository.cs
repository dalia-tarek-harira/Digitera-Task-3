using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Domain.Models;

namespace Task_3.Application.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int id);

        Task AddAsync(Job job);

        Task SaveChangesAsync();
    }
}
