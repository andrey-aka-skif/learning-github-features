using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace experimental.api.tests.IntegrationTests;

public class WeatherForecastApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    [Fact]
    public async Task Get_ReturnsSuccessStatusCode()
    {
        // Arrange
        // (все уже настроено фабрикой)

        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        response.EnsureSuccessStatusCode(); // Проверяет, что статус код 200-299
        Assert.Equal("application/json; charset=utf-8",
                     response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_ReturnsValidJsonData()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var content = await response.Content.ReadAsStringAsync();
        var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(content, _jsonSerializerOptions);

        // Assert
        Assert.NotNull(forecasts);
        Assert.True(forecasts.Count > 0);
    }
}
