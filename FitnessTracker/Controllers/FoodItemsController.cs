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
        try{
            var results = await _service.SearchAsync(query);
            return Ok(results);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Food API not available. Add manually via POST /api/fooditems/manual");
        }
    }
    

    [HttpPost("from-usda")]
    public async Task<IActionResult> CreateFromUsda(int fdcId)
    {
        var foodItem = await _service.GetOrCreateFromUsdaAsync(fdcId);
        return Ok(foodItem);
    }

    [HttpPost("manual")]
    public async Task<IActionResult> CreateManual(FoodItemManualCreateDto dto)
    {
        var foodItem = await _service.CreateManualAsync(dto.Name, dto.CaloriesPer100g);
        return Ok(foodItem);
    }
}