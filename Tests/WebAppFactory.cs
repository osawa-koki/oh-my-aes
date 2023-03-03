using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

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
    var response = await _client.GetAsync("/cipher/encrypt/256?key=ACB&data=HelloWorld");
    response.EnsureSuccessStatusCode();
    var responseString = await response.Content.ReadAsStringAsync();

    // JSONをデシリアライズして、期待値と比較する
    var result = JsonConvert.DeserializeObject<MyResponseType>(responseString);

    Assert.Equal("uaSInHrfPqXFv+87JUnPAw==", result.encrypted);
  }
}
