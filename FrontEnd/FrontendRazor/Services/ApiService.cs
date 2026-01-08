namespace FrontEndRazor.Services;

using FrontendRazor.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("BackendAPI");

        // Obtener token de la sesión
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/{endpoint}");

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"/api/{endpoint}", content);

        if (!response.IsSuccessStatusCode)
            return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<bool> PostAsync<TRequest>(string endpoint, TRequest data)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"/api/{endpoint}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"/api/{endpoint}", content);

        if (!response.IsSuccessStatusCode)
            return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest data)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"/api/{endpoint}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/{endpoint}");
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Success, string? Error)> LoginAsync(string nombreUsuario, string password)
    {
        var client = _httpClientFactory.CreateClient("BackendAPI");
        var loginData = new { NombreUsuario = nombreUsuario, Password = password };
        var json = JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorObj = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                return (false, errorObj?.GetValueOrDefault("message", "Error al iniciar sesión"));
            }
            catch
            {
                return (false, "Error al iniciar sesión");
            }
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
        {
            // Guardar token en sesión
            _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", loginResponse.Token);
            _httpContextAccessor.HttpContext?.Session.SetString("Usuario", JsonSerializer.Serialize(loginResponse.Usuario));
            return (true, null);
        }

        return (false, "Respuesta inválida del servidor");
    }

    public async Task<List<PermisoDto>?> GetPermisosAsync()
    {
        return await GetAsync<List<PermisoDto>>("auth/permisos");
    }
}