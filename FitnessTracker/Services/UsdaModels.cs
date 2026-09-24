public class UsdaSearchResponse
{
    public List<UsdaFood> Foods { get; set; }
}

public class UsdaFood
{
    public int FdcId { get; set; }
    public string Description { get; set; }
    public List<UsdaNutrient> FoodNutrients { get; set; }
}

public class UsdaNutrient
{
    public string NutrientName { get; set; }
    public double Value { get; set; }
}