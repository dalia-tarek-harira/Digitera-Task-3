using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Task_3.Application.Interfaces;

namespace Task_3.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(value, out var id))
                    return id;

                return null;
            }
        }

        public string? Role
        {
            get
            {
                return _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.Role);
            }
        }
    }
}
