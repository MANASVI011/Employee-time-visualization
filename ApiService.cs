using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeTimeTracker;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<TimeEntry>> GetTimeEntriesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(ApiUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var timeEntries = JsonSerializer.Deserialize<List<TimeEntry>>(response, options);
            return timeEntries ?? new List<TimeEntry>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Failed to retrieve data from API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Failed to parse JSON response: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
