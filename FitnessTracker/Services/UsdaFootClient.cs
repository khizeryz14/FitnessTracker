public class UsdaFoodClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public UsdaFoodClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> SearchFoodAsync(string query)
    {
        var apiKey = _config["Usda:ApiKey"];
        var response = await _httpClient.GetAsync($"foods/search?query={query}&api_key={apiKey}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}