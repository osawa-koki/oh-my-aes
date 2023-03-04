using System.Security.Cryptography;
using System.Text;

public static class ECB
{
  public static IResult Encrypt(int bit = 256, string key = "", string data = "")
  {
    try
    {
      // 対象文字列をUTF8でエンコードしてバイト配列に変換する
      byte[] plain_bytes = Encoding.UTF8.GetBytes(data);

      // 暗号化キーをバイト配列に変換する
      byte[] key_bytes = Encoding.UTF8.GetBytes(key);

      // キーの長さを調整する
      Util.AdjustKeySize(ref key_bytes, bit);

      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.ECB;

      // 暗号化を行う
      byte[] encryptedBytes;
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
      {
        encryptedBytes = encryptor.TransformFinalBlock(plain_bytes, 0, plain_bytes.Length);
      }

      // 暗号化されたバイト配列をBase64文字列に変換する
      string encryptedString = Convert.ToBase64String(encryptedBytes);
      Console.WriteLine($"Encrypted string: {encryptedString}");

      MyResponseType response = new(
        MyCipherMethod.Encrypt,
        MyCipherAlgo.AES,
        MyCipherMode.ECB,
        bit,
        key,
        null,
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

  public static IResult Decrypt(int bit = 256, string key = "", string data = "")
  {
    try
    {
      Console.WriteLine($"Decrypting {data} with {key}...");

      // 暗号化された文字列をBase64でデコードしてバイト配列に変換する
      byte[] encrypted_bytes = Convert.FromBase64String(data);

      // 暗号化キーをバイト配列に変換する
      byte[] key_bytes = Encoding.UTF8.GetBytes(key);

      // キーの長さを調整する
      Util.AdjustKeySize(ref key_bytes, bit);

      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.ECB;

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
        MyCipherMethod.Encrypt,
        MyCipherAlgo.AES,
        MyCipherMode.ECB,
        bit,
        key,
        null,
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
