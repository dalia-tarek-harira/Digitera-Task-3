using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Application.DTOs;

namespace Task_3.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
