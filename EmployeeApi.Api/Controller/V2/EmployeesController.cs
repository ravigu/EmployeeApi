using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.Controller.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "Employee API V2"
            });
        }
    }
}
