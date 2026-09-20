using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly UsdaFoodClient _usdaClient;
    public TestController(UsdaFoodClient usdaClient) => _usdaClient = usdaClient;

    [HttpGet("usda")]
    public async Task<IActionResult> TestUsda(string query)
    {
        var result = await _usdaClient.SearchFoodAsync(query);
        return Ok(result);
    }
}