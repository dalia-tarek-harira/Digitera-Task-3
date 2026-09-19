using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_3.Application.DTOs;
using Task_3.Application.Interfaces;

namespace Task_3.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (dto.Role != "Candidate" &&
                dto.Role != "Recruiter")
            {
                throw new InvalidOperationException(
                    "Role must be Candidate or Recruiter.");
            }

            var existingUser =
                await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "Email is already registered.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager
                .CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(errors);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            var token = await GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email!,
                Role = dto.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user =
                await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var validPassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!validPassword)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault();

            if (role == null)
            {
                throw new UnauthorizedAccessException(
                    "User has no role.");
            }

            var token = await GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email!,
                Role = role
            };
        }

        private async Task<string> GenerateTokenAsync(
            ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                user.Email ?? string.Empty),

            new Claim(
                ClaimTypes.Name,
                user.FullName)
        };

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role));
            }

            var key = _configuration["Jwt:Key"]
                      ?? throw new InvalidOperationException(
                          "JWT Key is missing.");

            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            var expires =
                DateTime.UtcNow.AddMinutes(
                    double.Parse(
                        _configuration["Jwt:DurationInMinutes"]
                        ?? "60"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
