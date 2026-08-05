using System.Net;
using System.Net.Http.Json;
using RacehubApi.DTOs;
using Xunit;

namespace RacehubApi.Tests;

public class IntegrationTests : IClassFixture<TestFactory>
{
    private readonly HttpClient _client;

    public IntegrationTests(TestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        var loginDto = new { Email = "admin@racehub.com", Password = "admin123" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.Equal("admin@racehub.com", result.User.Email);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var loginDto = new { Email = "admin@racehub.com", Password = "wrongpassword" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetRaces_ReturnsList()
    {
        var response = await _client.GetAsync("/api/trailrunning");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // Since we seeded the DB, there should be some races
        var races = await response.Content.ReadFromJsonAsync<List<RaceDto>>();
        Assert.NotNull(races);
        Assert.NotEmpty(races);
    }
}
