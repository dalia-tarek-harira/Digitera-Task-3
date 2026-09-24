using System;
using System.Collections.Generic;
using System.Text;

namespace Task_3.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyCandidate(int applicationId);
    }
}
