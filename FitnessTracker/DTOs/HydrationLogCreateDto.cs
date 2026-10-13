using System.ComponentModel.DataAnnotations;
public class HydrationLogCreateDto
{
    [Range(1, 5000)] public double AmountMl { get; set; }
    public DateTime LoggedTimestamp { get; set; }
}