public class FoodLogResponseDto
{
            public Guid Id {get; set;}
            public Guid UserId {get; set;}
            public Guid FoodItemId {get; set;}
            public double QuantityGrams {get; set;}
            public DateTime LoggedTimestamp {get; set;}
            public double CaloriesConsumed {get; set;}
}