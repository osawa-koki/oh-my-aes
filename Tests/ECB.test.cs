using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public partial class MyTest : IClassFixture<WebApplicationFactory<Program>>
{
  [Fact]
  public async Task ECB_MATCHER_TEST()
  {
    // // テスト用のデータを生成する
    var cipher_data_list = new string[] { "HelloWorld", "あいうえお", "ｱｲｳｴｵ", "人工知能" };
    var cipher_key_list = new string[] { "ABC", "_ABC", "ｱｲｳｴｵ", "あ", "計算機" };
    var cipher_length_list = new int[] { 128, 192, 256 };

    for (int i = 0; i < 30; i++)
    {
      // キーとデータのビットはランダムで文字列を生成して、暗号化・復号化を行う
      var random = new Random();

      var key_string = cipher_key_list[random.Next(0, cipher_key_list.Length)];
      var data_string = cipher_data_list[random.Next(0, cipher_data_list.Length)];
      var cipher_length = cipher_length_list[random.Next(0, cipher_length_list.Length)];

      // 暗号化
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{cipher_length}?key={key_string}&data={data_string}";
      _testOutputHelper.WriteLine($"encrypt_request_path -> {encrypt_request_path}");
      var encrypt_response = await _client.GetAsync(encrypt_request_path);
      encrypt_response.EnsureSuccessStatusCode();
      var encrypt_response_string = await encrypt_response.Content.ReadAsStringAsync();

      // JSONをデシリアライズして、期待値と比較する
      _testOutputHelper.WriteLine($"encrypt_response_string -> {encrypt_response_string}");
      var encrypt_result = JsonConvert.DeserializeObject<MyResponseType>(encrypt_response_string);

      // 復号化を行う
      var decrypt_request_path = $"/api/cipher/aes/ecb/decrypt/{cipher_length}?key={key_string}&data={Uri.EscapeDataString(encrypt_result!.Encrypted!)}";
      _testOutputHelper.WriteLine($"decrypt_request_path -> {decrypt_request_path}");
      var decrypt_response = await _client.GetAsync(decrypt_request_path);
      decrypt_response.EnsureSuccessStatusCode();
      var decrypt_response_string = await decrypt_response.Content.ReadAsStringAsync();

      // JSONをデシリアライズして、期待値と比較する
      _testOutputHelper.WriteLine($"decrypt_response_string -> {decrypt_response_string}");
      var decrypt_result = JsonConvert.DeserializeObject<MyResponseType>(decrypt_response_string);

      Assert.Equal(data_string, decrypt_result!.Decrypted);

      _testOutputHelper.WriteLine($"========== ========= ========== ========= ==========");
    }
  }

  [Fact]
  public async Task ECB_size_valider()
  {
    {
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{100}?key=HelloWorld&data=HelloWorld";
      var response = await _client.GetAsync(encrypt_request_path);
      Assert.Equal(400, (int)response.StatusCode);
    }
    {
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{128}?key=HelloWorld&data=HelloWorld";
      var response = await _client.GetAsync(encrypt_request_path);
      Assert.Equal(200, (int)response.StatusCode);
    }
    {
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{192}?key=HelloWorld&data=HelloWorld";
      var response = await _client.GetAsync(encrypt_request_path);
      Assert.Equal(200, (int)response.StatusCode);
    }
    {
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{256}?key=HelloWorld&data=HelloWorld";
      var response = await _client.GetAsync(encrypt_request_path);
      Assert.Equal(200, (int)response.StatusCode);
    }
    {
      var encrypt_request_path = $"/api/cipher/aes/ecb/encrypt/{512}?key=HelloWorld&data=HelloWorld";
      var response = await _client.GetAsync(encrypt_request_path);
      Assert.Equal(400, (int)response.StatusCode);
    }
  }
}
