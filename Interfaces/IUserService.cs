public interface IUserService
{
    Task<User?> RegisterAsync(UserRegisterDto dto);
    Task<string> LoginAsync(UserLoginDto dto);
}