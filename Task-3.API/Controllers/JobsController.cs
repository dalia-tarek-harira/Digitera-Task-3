using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_3.Application.Services;

namespace Task_3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;

        public JobsController(JobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            try
            {
                await _jobService.CloseAsync(id);

                return Ok(new
                {
                    message = "Job closed successfully."
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
