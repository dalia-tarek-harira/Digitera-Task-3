using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_3.Application.DTOs;
using Task_3.Application.Services;

namespace Task_3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationsController(
            ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateApplicationDto dto)
        {
            try
            {
                var id =
                    await _applicationService.CreateAsync(dto);

                return Created(
                    $"/api/applications/{id}",
                    new
                    {
                        id,
                        message = "Application created successfully."
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Candidate")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _applicationService.CancelAsync(id);

                return Ok(new
                {
                    message = "Application cancelled successfully."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
