using System.Security.Cryptography;
using System.Text;

public static class CBC
{
  public static IResult Encrypt(int bit = 256, string key = "", string data = "", string iv = "")
  {
    try
    {
      var valid_bits = new int[] { 128, 192, 256 };
      if (valid_bits.Contains(bit) == false)
      {
        return Results.BadRequest($"Invalid bit: {bit}");
      }

      // 対象文字列をUTF8でエンコードしてバイト配列に変換する
      byte[] plain_bytes = Encoding.UTF8.GetBytes(data);

      // 暗号化キーをバイト配列に変換する
      byte[] key_bytes = Encoding.UTF8.GetBytes(key);
      Util.AdjustKeySize(ref key_bytes, bit);

      // 初期化ベクトルをバイト配列に変換する
      byte[] iv_bytes = Convert.FromBase64String(iv);
      Util.AdjustKeySize(ref iv_bytes, 128);

      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.CBC;
      aes.IV = iv_bytes;

      // 暗号化を行う
      byte[] encrypted_bytes;
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
      {
        encrypted_bytes = encryptor.TransformFinalBlock(plain_bytes, 0, plain_bytes.Length);
      }

      // 暗号化されたバイト配列をBase64文字列に変換する
      string encryptedString = Convert.ToBase64String(encrypted_bytes);
      Console.WriteLine($"Encrypted string: {encryptedString}");

      MyResponseType response = new(
        MyCipherMethod.Encrypt,
        MyCipherAlgo.AES,
        MyCipherMode.CBC,
        bit,
        key,
        iv,
        data,
        encryptedString,
        null
      );

      return Results.Ok(response);
    }
    catch (CryptographicException ex)
    {
      return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex}");
      return Results.Problem($"{ex}");
    }
  }

  public static IResult Decrypt(int bit = 256, string key = "", string data = "", string iv = "")
  {
    try
    {
      var valid_bits = new int[] { 128, 192, 256 };
      if (valid_bits.Contains(bit) == false)
      {
        return Results.BadRequest($"Invalid bit: {bit}");
      }

      // 暗号化された文字列をBase64でデコードしてバイト配列に変換する
      byte[] encrypted_bytes = Convert.FromBase64String(data);

      // 暗号化キーをバイト配列に変換する
      byte[] key_bytes = Encoding.UTF8.GetBytes(key);
      Util.AdjustKeySize(ref key_bytes, bit);

      // 初期化ベクトルをバイト配列に変換する
      byte[] iv_bytes = Convert.FromBase64String(iv);
      Util.AdjustKeySize(ref iv_bytes, 128);


      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.CBC;
      aes.IV = iv_bytes;

      // 復号化を行う
      byte[] decryptedBytes;
      using (ICryptoTransform decryptor = aes.CreateDecryptor())
      {
        decryptedBytes = decryptor.TransformFinalBlock(encrypted_bytes, 0, encrypted_bytes.Length);
      }

      // 復号化されたバイト配列をUTF8文字列に変換する
      string decryptedString = Encoding.UTF8.GetString(decryptedBytes);
      Console.WriteLine($"Decrypted string: {decryptedString}");

      MyResponseType response = new(
        MyCipherMethod.Decrypt,
        MyCipherAlgo.AES,
        MyCipherMode.CBC,
        bit,
        key,
        iv,
        data,
        null,
        decryptedString
      );

      return Results.Ok(response);
    }
    catch (CryptographicException ex)
    {
      return Results.BadRequest(ex.Message);
    }
    catch (FormatException ex)
    {
      return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
      return Results.Problem($"{ex}");
    }
  }
}

