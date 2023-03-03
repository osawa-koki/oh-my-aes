
using System.Security.Cryptography;
using System.Text;

public static class ECB
{
  public static IResult Encrypt(int bit = 256, string? key = null, string? data = null)
  {
    try
    {
      // 対象文字列をUTF8でエンコードしてバイト配列に変換する
      string target = data ?? "";
      byte[] plainBytes = Encoding.UTF8.GetBytes(target);

      // 暗号化キーをバイト配列に変換する
      string key_string = key ?? "";
      byte[] key_bytes = Encoding.UTF8.GetBytes(key_string);

      // キーの長さを調整する
      int keySize = bit / 8;
      if (key_bytes.Length != keySize)
      {
        Array.Resize(ref key_bytes, keySize);
        if (key_bytes.Length < keySize)
        {
          // キーの長さが指定されたビット数に満たない場合は、ゼロパディングする
          Array.Clear(key_bytes, key_bytes.Length, keySize - key_bytes.Length);
        }
        else
        {
          // キーの長さが指定されたビット数を超えている場合は、切り捨てる
          Array.Resize(ref key_bytes, keySize);
        }
      }

      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.ECB;

      // 暗号化を行う
      byte[] encryptedBytes;
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
      {
        encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
      }

      // 暗号化されたバイト配列をBase64文字列に変換する
      string encryptedString = Convert.ToBase64String(encryptedBytes);
      Console.WriteLine($"Encrypted string: {encryptedString}");

      MyResponseType response = new(
        MyCipherMethod.Encrypt,
        MyCipherAlgo.AES,
        MyCipherMode.ECB,
        bit,
        target,
        null,
        key_string,
        encryptedString,
        null
      );

      return Results.Ok(response);
    } catch (Exception ex)
    {
      Console.WriteLine($"{ex}");
      return Results.Problem($"{ex}");
    }
  }

  public static IResult Decrypt(int bit = 256, string? key = null, string? data = null)
  {
    try
    {
      // 暗号化された文字列をBase64でデコードしてバイト配列に変換する
      string encryptedString = data ?? "";
      byte[] encryptedBytes = Convert.FromBase64String(encryptedString);

      // 暗号化キーをバイト配列に変換する
      string key_string = key ?? "";
      byte[] key_bytes = Encoding.UTF8.GetBytes(key_string);

      // キーの長さを調整する
      int keySize = bit / 8;
      if (key_bytes.Length != keySize)
      {
        Array.Resize(ref key_bytes, keySize);
        if (key_bytes.Length < keySize)
        {
          // キーの長さが指定されたビット数に満たない場合は、ゼロパディングする
          Array.Clear(key_bytes, key_bytes.Length, keySize - key_bytes.Length);
        }
        else
        {
          // キーの長さが指定されたビット数を超えている場合は、切り捨てる
          Array.Resize(ref key_bytes, keySize);
        }
      }

      // AESのインスタンスを作成する
      Aes aes = Aes.Create();
      aes.Key = key_bytes;
      aes.Mode = CipherMode.ECB;

      // 復号化を行う
      byte[] decryptedBytes;
      using (ICryptoTransform decryptor = aes.CreateDecryptor())
      {
        decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
      }

      // 復号化されたバイト配列をUTF8文字列に変換する
      string decryptedString = Encoding.UTF8.GetString(decryptedBytes);
      Console.WriteLine($"Decrypted string: {decryptedString}");

      return Results.Ok(decryptedString);
    } catch (Exception ex)
    {
      Console.WriteLine($"{ex}");
      return Results.Problem($"{ex}");
    }
  }
}
