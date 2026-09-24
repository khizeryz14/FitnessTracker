public class UsdaFoodClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public UsdaFoodClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<UsdaFood>> SearchFoodAsync(string query)
    {
        var apiKey = _config["Usda:ApiKey"];
        var response = await _httpClient.GetAsync($"foods/search?query={query}&pageSize=5&api_key={apiKey}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UsdaSearchResponse>();
        return result?.Foods ?? new List<UsdaFood>();
    }
}