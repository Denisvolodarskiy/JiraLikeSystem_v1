using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using System.Text.Json;
using Xunit;

namespace JiraLikeSystem.Tests.IntegrationTests;
public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ReturnsOkResult_WhenUserRegisteredSuccessfully()
    {
        // Arrange
        var registerModel = new
        {
            Email = "test@example.com",
            Password = "Password123",
            Role = "User"
        };

        var content = new StringContent(JsonSerializer.Serialize(registerModel), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/auth/register", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("User registered successfully!", responseString);
    }
}
