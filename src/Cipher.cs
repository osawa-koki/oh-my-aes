using System.Security.Cryptography;
using System.Text;

namespace oh_my_aes
{
  public static class MyCipher
  {
    public static int[] valid_bits = new int[] { 128, 192, 256 };

    public static IResult Encrypt(string mode = "", int bit = 0, string key = "", string data = "", string iv = "")
    {
      try
      {
        // ビット数が有効かチェックする
        if (valid_bits.Contains(bit) == false)
        {
          return Results.BadRequest(new {
            message = $"Invalid bit: {bit}",
          });
        }

        // モードが有効かチェックする
        CipherMode? my_mode = mode switch
        {
          "ECB" => CipherMode.ECB,
          "CBC" => CipherMode.CBC,
          "CFB" => CipherMode.CFB,
          "OFB" => CipherMode.OFB,
          "CTS" => CipherMode.CTS,
          _ => null,
        };
        if (my_mode == null)
        {
          return Results.BadRequest(new {
            message = $"Invalid mode: {mode}",
          });
        }

        // 対象文字列をUTF8でエンコードしてバイト配列に変換する
        byte[] plain_bytes = Encoding.UTF8.GetBytes(data);

        // 暗号化キーをバイト配列に変換する
        byte[] key_bytes = Encoding.UTF8.GetBytes(key);
        Util.AdjustKeySize(ref key_bytes, bit);

        // 初期化ベクトルをバイト配列に変換する
        byte[] iv_bytes = Encoding.UTF8.GetBytes(iv);
        Util.AdjustKeySize(ref iv_bytes, 128);

        // AESのインスタンスを作成する
        Aes aes = Aes.Create();
        aes.Key = key_bytes;
        aes.Mode = (CipherMode)my_mode;
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
          CipherMethod.Encrypt,
          CipherAlgo.AES,
          CipherMode.CBC,
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

    public static IResult Decrypt(string mode = "", int bit = 0, string key = "", string data = "", string iv = "")
    {
      try
      {
        // ビット数が有効かチェックする
        if (valid_bits.Contains(bit) == false)
        {
          return Results.BadRequest(new
          {
            message = $"Invalid bit: {bit}",
          });
        }

        // モードが有効かチェックする
        CipherMode? my_mode = mode switch
        {
          "ECB" => CipherMode.ECB,
          "CBC" => CipherMode.CBC,
          "CFB" => CipherMode.CFB,
          "OFB" => CipherMode.OFB,
          "CTS" => CipherMode.CTS,
          _ => null,
        };
        if (my_mode == null)
        {
          return Results.BadRequest(new
          {
            message = $"Invalid mode: {mode}",
          });
        }

        // 暗号化された文字列をBase64でデコードしてバイト配列に変換する
        byte[] encrypted_bytes = Convert.FromBase64String(data);

        // 暗号化キーをバイト配列に変換する
        byte[] key_bytes = Encoding.UTF8.GetBytes(key);
        Util.AdjustKeySize(ref key_bytes, bit);

        // 初期化ベクトルをバイト配列に変換する
        byte[] iv_bytes = Encoding.UTF8.GetBytes(iv);
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
          CipherMethod.Decrypt,
          CipherAlgo.AES,
          CipherMode.CBC,
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
}
