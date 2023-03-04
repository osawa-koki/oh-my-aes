
public static partial class Util
{
  public static void AdjustKeySize(byte[] bytes, int bit)
  {
    // キーの長さを調整する
    int keySize = bit / 8;
    if (bytes.Length != keySize)
    {
      Array.Resize(ref bytes, keySize);
      if (bytes.Length < keySize)
      {
        // キーの長さが指定されたビット数に満たない場合は、ゼロパディングする
        Array.Clear(bytes, bytes.Length, keySize - bytes.Length);
      }
      else
      {
        // キーの長さが指定されたビット数を超えている場合は、切り捨てる
        Array.Resize(ref bytes, keySize);
      }
    }
  }
}
