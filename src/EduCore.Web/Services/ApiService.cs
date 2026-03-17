using System.Net.Http.Headers;
using System.Net.Http.Json;
using EduCore.Application.DTOs;

namespace EduCore.Web.Services;

public interface IApiService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<List<T>> GetListAsync<T>(string endpoint);
    Task<T?> GetAsync<T>(string endpoint);
    Task<T> PostAsync<T>(string endpoint, object data);
    Task PutAsync(string endpoint, object data);
    Task DeleteAsync(string endpoint);
    Task<DashboardDto> GetDashboardAsync();
}

public class ApiService : IApiService
{
    private readonly HttpClient _http;
    private readonly CustomAuthStateProvider _authState;

    public ApiService(HttpClient http, CustomAuthStateProvider authState)
    {
        _http = http;
        _authState = authState;
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = await _authState.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return result ?? new LoginResponseDto { Success = false, ErrorMessage = "Error de conexión" };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        await SetAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return result ?? new LoginResponseDto { Success = false, ErrorMessage = "Error de conexión" };
    }

    public async Task<List<T>> GetListAsync<T>(string endpoint)
    {
        await SetAuthHeaderAsync();
        var result = await _http.GetFromJsonAsync<List<T>>($"api/{endpoint}");
        return result ?? new List<T>();
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await SetAuthHeaderAsync();
        return await _http.GetFromJsonAsync<T>($"api/{endpoint}");
    }

    public async Task<T> PostAsync<T>(string endpoint, object data)
    {
        await SetAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync($"api/{endpoint}", data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>();
        return result!;
    }

    public async Task PutAsync(string endpoint, object data)
    {
        await SetAuthHeaderAsync();
        var response = await _http.PutAsJsonAsync($"api/{endpoint}", data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint)
    {
        await SetAuthHeaderAsync();
        var response = await _http.DeleteAsync($"api/{endpoint}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        await SetAuthHeaderAsync();
        var result = await _http.GetFromJsonAsync<DashboardDto>("api/dashboard");
        return result ?? new DashboardDto();
    }
}
