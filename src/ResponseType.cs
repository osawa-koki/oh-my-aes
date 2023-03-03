
public enum MyCipherMethod
{
  Encrypt,
  Decrypt,
}

public enum MyCipherAlgo
{
  AES
}

public enum MyCipherMode
{
  ECB
}

public record MyResponseType(
  MyCipherMethod CipherMethod,
  MyCipherAlgo CipherAlgo,
  MyCipherMode CipherMode,
  int CipherLength,
  string Key,
  string? IV,
  string data,
  string? encrypted,
  string? decypted
);
