using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodItemsController : ControllerBase
{
    private readonly IFoodItemService _service;
    public FoodItemsController(IFoodItemService service) => _service = service;

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        var results = await _service.SearchAsync(query);
        return Ok(results);
    }

    [HttpPost("from-usda")]
    public async Task<IActionResult> CreateFromUsda(int fdcId, string name, double caloriesPer100g)
    {
        var foodItem = await _service.GetOrCreateFromUsdaAsync(fdcId, name, caloriesPer100g);
        return Ok(foodItem);
    }

    [HttpPost("manual")]
    public async Task<IActionResult> CreateManual(string name, double caloriesPer100g)
    {
        var foodItem = await _service.CreateManualAsync(name, caloriesPer100g);
        return Ok(foodItem);
    }
}