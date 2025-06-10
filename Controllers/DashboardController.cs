using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemBackend.Services.Interfaces;

namespace SystemBackend.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("")]
        public IActionResult GetDashboardInformation()
        {
            try
            {
                return Ok(_dashboardService.GetDashboardInformation());
            } 
            catch(Exception)
            {
                return StatusCode(500, new
                {
                    error = new { message = "Internal server error" }
                });
            }
        }
    }
}
