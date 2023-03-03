using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit.Abstractions;

public partial class MyTest : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public MyTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
  {
    _client = factory.CreateClient();
    _testOutputHelper = testOutputHelper;
  }

  private readonly ITestOutputHelper _testOutputHelper;

}
