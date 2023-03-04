
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

    app.MapGet("/api/cipher/aes/ecb/encrypt/{bit}", ECB.Encrypt).WithName("Encrypt").WithOpenApi();
    app.MapGet("/api/cipher/aes/ecb/decrypt/{bit}", ECB.Decrypt).WithName("Decrypt").WithOpenApi();

    app.Run("http://0.0.0.0:8000");
  }
}
