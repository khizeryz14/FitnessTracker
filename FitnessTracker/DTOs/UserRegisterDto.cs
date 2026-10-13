using System.ComponentModel.DataAnnotations;

public class UserRegisterDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Range(0.5, 2.5)]            // meters
    public double Height { get; set; }

    [Range(20, 500)]             // kg
    public double Weight { get; set; }

    [EnumDataType(typeof(Sex))]
    public Sex Sex { get; set; }

    public DateOnly DateOfBirth { get; set; }

    [EnumDataType(typeof(ActivityLevel))]
    public ActivityLevel ActivityLevel { get; set; }
}