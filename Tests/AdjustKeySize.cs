using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public partial class MyTest : IClassFixture<WebApplicationFactory<Program>>
{

  public void AdjustKeySize()
  {
    {
      var size = 256;
      {
        var bytes_under = new byte[200];
        Util.AdjustKeySize(bytes_under, size);
        Assert.Equal(256, bytes_under.Length);
      }

      {
        var bytes_over = new byte[300];
        Util.AdjustKeySize(bytes_over, size);
        Assert.Equal(256, bytes_over.Length);
      }

      {
        var bytes_equal = new byte[256];
        Util.AdjustKeySize(bytes_equal, size);
        Assert.Equal(256, bytes_equal.Length);
      }
    }
    {
      var size = 512;
      {
        var bytes_under = new byte[200];
        Util.AdjustKeySize(bytes_under, size);
        Assert.Equal(512, bytes_under.Length);
      }

      {
        var bytes_over = new byte[300];
        Util.AdjustKeySize(bytes_over, size);
        Assert.Equal(512, bytes_over.Length);
      }

      {
        var bytes_equal = new byte[512];
        Util.AdjustKeySize(bytes_equal, size);
        Assert.Equal(512, bytes_equal.Length);
      }
    }
  }
}
