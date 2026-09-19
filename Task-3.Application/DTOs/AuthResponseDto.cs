using System;
using System.Collections.Generic;
using System.Text;

namespace Task_3.Application.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
