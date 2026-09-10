using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        var user = await _userService.RegisterAsync(dto);
        if (user == null) return BadRequest("Email already in use.");
        return Created($"/api/users/{user.Id}", new { user.Id, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var token = await _userService.LoginAsync(dto);
        if (token == null)
            return Unauthorized("Invalid email or password.");

        return Ok(new { token });
    }
    
}