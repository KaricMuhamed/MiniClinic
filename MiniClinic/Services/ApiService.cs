using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;
    private readonly string _baseUrl = "https://sandbox-healthservice.priaid.ch";

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiToken = configuration["ApiToken"];
    }

    // General function to make API requests
    private async Task<string> GetApiResponseAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
        HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"API request failed: {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    // Fetch Body Locations
    public async Task<string> GetBodyLocations() =>
        await GetApiResponseAsync("body/locations?language=en-gb");

    // Fetch Body Sub-Locations
    public async Task<string> GetBodyLocation(int bodyLocationId) =>
        await GetApiResponseAsync($"body/locations/{bodyLocationId}?language=en-gb");

    // Fetch Symptoms based on Body Sub-Location
    public async Task<string> GetSymptoms(int bodySubLocationId) =>
        await GetApiResponseAsync($"symptoms/{bodySubLocationId}?language=en-gb");

}
