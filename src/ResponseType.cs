
using System.Security.Cryptography;

public enum CipherMethod
{
  Encrypt,
  Decrypt,
}

public enum CipherAlgo
{
  AES,
}

public record MyResponseType(
  string CipherMethod,
  string CipherAlgo,
  string CipherMode,
  int CipherLength,
  string Key,
  string? IV,
  string Data,
  string? Encrypted,
  string? Decrypted
);
