using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebApiClient
{

    public async Task<string> GetSatellitesAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();  // Throws if status code is not 2xx
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching satellites: {ex.Message}");
            return null;
        }
    }

    private readonly HttpClient _httpClient = new();
}
