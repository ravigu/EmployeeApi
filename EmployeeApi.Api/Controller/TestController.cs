using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Message = "Version 1",
            Time = DateTime.Now
        });
    }
}