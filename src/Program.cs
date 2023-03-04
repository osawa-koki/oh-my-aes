
public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    var MyAllowSpecificOrigins = "MyCORS";

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options =>
    {
      options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
          policy.WithOrigins("*");
        });
    });

    var app = builder.Build();

    app.UseCors(MyAllowSpecificOrigins);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.MapGet("/api/cipher/aes/ecb/encrypt/{bit}", ECB.Encrypt).WithName("EcbEncrypt").WithOpenApi();
    app.MapGet("/api/cipher/aes/ecb/decrypt/{bit}", ECB.Decrypt).WithName("EcbDecrypt").WithOpenApi();
    app.MapGet("/api/cipher/aes/cbc/encrypt/{bit}", CBC.Encrypt).WithName("CbcEncrypt").WithOpenApi();
    app.MapGet("/api/cipher/aes/cbc/decrypt/{bit}", CBC.Decrypt).WithName("CbcDecrypt").WithOpenApi();

    app.Run("http://0.0.0.0:8000");
  }
}
