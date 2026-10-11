using Microsoft.Extensions.Caching.Memory;

public class UsdaFoodClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;

    public UsdaFoodClient(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _config = config;
        _cache = cache;
    }

    public async Task<List<UsdaFood>> SearchFoodAsync(string query)
    {
        var cacheKey = $"usda-search:{query.Trim().ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out List<UsdaFood>? cached) && cached is not null)
            return cached;

        var apiKey = _config["Usda:ApiKey"];
        var response = await _httpClient.GetAsync(
            $"foods/search?query={Uri.EscapeDataString(query)}&pageSize=5&api_key={apiKey}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UsdaSearchResponse>();
        var foods = result?.Foods ?? new List<UsdaFood>();

        _cache.Set(cacheKey, foods, TimeSpan.FromHours(1));
        return foods;
    }

    public async Task<UsdaFoodDetail?> GetFoodDetailsAsync(int fdcId)
    {
        var apiKey = _config["Usda:ApiKey"];
        var response = await _httpClient.GetAsync($"food/{fdcId}?api_key={apiKey}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UsdaFoodDetail>();
    }
}