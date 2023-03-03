using Microsoft.AspNetCore.Mvc.Testing;

public class MyTest : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public MyTest(WebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task TestMethod()
  {

  }
}
