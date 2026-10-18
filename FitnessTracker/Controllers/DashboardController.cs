using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    public DashboardController(IDashboardService service) => _service = service;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
    [HttpGet]
    public async Task<IActionResult> Get(DateOnly? date, int tzOffsetMinutes = 0)
    {
        var dashboard = await _service.GetAsync(GetUserId(), date, tzOffsetMinutes);
        return dashboard is null ? NotFound() : Ok(dashboard);
    }
}