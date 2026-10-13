public interface IUserService
{
    Task<Result<User>> RegisterAsync(UserRegisterDto dto);
    Task<string> LoginAsync(UserLoginDto dto);
}