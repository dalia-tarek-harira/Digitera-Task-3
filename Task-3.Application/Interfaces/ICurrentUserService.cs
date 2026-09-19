using System;
using System.Collections.Generic;
using System.Text;

namespace Task_3.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        string? Role { get; }
    }
}
