using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;
using Xunit;

public class UserServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 13, 12, 0, 0, TimeSpan.Zero);

    private static UserService CreateService(AppDbContext db) =>
        new(db, new ConfigurationBuilder().Build(), new FakeTimeProvider(Now));

    private static UserRegisterDto ValidDto(string email = "a@test.com") => new()
    {
        Name = "Test", Email = email, Password = "password123",
        Height = 1.75, Weight = 70, Sex = Sex.Male,
        DateOfBirth = new DateOnly(1996, 10, 13), ActivityLevel = ActivityLevel.Average
    };

    [Fact]
    public async Task Register_StoresHashedPassword_NotPlaintext()
    {
        using var testDb = new TestDb();
        var result = await CreateService(testDb.Context).RegisterAsync(ValidDto());

        Assert.True(result.Success);
        Assert.NotEqual("password123", result.Value!.PasswordHash);
        var verify = new PasswordHasher<User>()
            .VerifyHashedPassword(result.Value, result.Value.PasswordHash, "password123");
        Assert.NotEqual(PasswordVerificationResult.Failed, verify);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsConflict_EvenWithDifferentCasing()
    {
        using var testDb = new TestDb();
        var service = CreateService(testDb.Context);

        await service.RegisterAsync(ValidDto("a@test.com"));
        var second = await service.RegisterAsync(ValidDto("A@Test.com"));

        Assert.False(second.Success);
        Assert.Equal(ErrorKind.Conflict, second.Kind);
    }

    [Fact]
    public async Task Register_Under12_ReturnsValidationError()
    {
        using var testDb = new TestDb();
        var dto = ValidDto();
        dto.DateOfBirth = new DateOnly(2020, 1, 1);

        var result = await CreateService(testDb.Context).RegisterAsync(dto);

        Assert.False(result.Success);
        Assert.Equal(ErrorKind.Validation, result.Kind);
    }
}