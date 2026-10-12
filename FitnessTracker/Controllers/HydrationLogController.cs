using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HydrationLogController : ControllerBase
{
    private readonly IHydrationLogService _service;
    public HydrationLogController(IHydrationLogService service) => _service = service;

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private static HydrationLogResponseDto ToDto(HydrationLogEntry e) => new()
    {
        Id = e.Id,
        AmountMl = e.AmountMl,
        LoggedTimestamp = e.LoggedTimestamp
    };

    [HttpPost]
    public async Task<IActionResult> Create(HydrationLogCreateDto dto)
    {
        var entry = await _service.CreateAsync(GetUserId(), dto);
        if (entry == null) return BadRequest("Amount must be greater than zero.");
        return Created($"/api/hydrationlog/{entry.Id}", ToDto(entry));
    }

    [HttpGet]
    public async Task<IActionResult> GetEntries(
        DateTime? from, DateTime? to, int page = 1, int pageSize = 10)
    {
        if (from.HasValue) from = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);
        if (to.HasValue)   to   = DateTime.SpecifyKind(to.Value, DateTimeKind.Utc);

        var result = await _service.GetAllForUserAsync(GetUserId(), from, to, page, pageSize);
        return Ok(new PagedResult<HydrationLogResponseDto>
        {
            Items = result.Items.Select(ToDto).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        });
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(Guid logId)
    {
        var deleted = await _service.DeleteAsync(GetUserId(), logId);
        return deleted ? NoContent() : NotFound();
    }
}