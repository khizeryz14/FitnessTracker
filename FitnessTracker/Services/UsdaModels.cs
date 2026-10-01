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

public class UsdaFoodDetail
{
    public string Description { get; set; }
    public List<UsdaDetailNutrient> FoodNutrients { get; set; }
}

public class UsdaDetailNutrient
{
    public UsdaNutrientInfo Nutrient { get; set; }
    public double Amount { get; set; }
}

public class UsdaNutrientInfo
{
    public string Name { get; set; }
}