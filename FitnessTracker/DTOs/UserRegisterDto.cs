public class UserRegisterDto
{
    public string Name {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public double Height {get; set;}
    public double Weight {get; set;}
    public Sex Sex {get; set;}
    public DateOnly DateOfBirth {get; set;}

    public ActivityLevel ActivityLevel {get; set;}
}