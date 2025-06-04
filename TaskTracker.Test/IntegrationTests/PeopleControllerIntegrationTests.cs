using System.Net.Http.Json;
using FluentAssertions;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Features.People.Dtos;
using Xunit;

namespace TaskTracker.Test.IntegrationTests
{
    public class PeopleControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PeopleControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Post_CreatesPerson()
        {
            using var client = _factory.CreateClient();
            var cmd = new CreatePersonCommand("John","Doe", DateOnly.FromDateTime(DateTime.Today.AddYears(-25)));

            var response = await client.PostAsJsonAsync("/api/People", cmd);

            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<PersonDto>();
            dto!.FirstName.Should().Be("John");
        }
    }
}
