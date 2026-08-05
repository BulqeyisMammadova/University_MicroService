using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using User.Servic.Business.Services.Abstractions;
using User.Service.Business.DTOs;
using User.Service.Core.Enum;

namespace User.Service.Business.Clients.Implementations;

public class AuthServiceClient(HttpClient httpClient) : IAuthServiceClient
{

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true   

    };

    public async Task<AccessTokenDto> GenerateTokenAsync(int userId, string email, Role role)
    {
//Auth zeng
        var response = await httpClient.PostAsJsonAsync("api/auth/token", new
        {
            UserId = userId,
            Email = email,
            Role = role
        }, JsonOptions);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AccessTokenDto>(JsonOptions);
        return result!;
    }
}