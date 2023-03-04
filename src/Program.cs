
namespace oh_my_aes
{

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

      app.MapGet("/api/cipher/aes/encrypt/{mode}/{bit}", MyCipher.Encrypt).WithName("Encrypt").WithOpenApi();
      app.MapGet("/api/cipher/aes/decrypt/{mode}/{bit}", MyCipher.Decrypt).WithName("Decrypt").WithOpenApi();

      app.Run("http://0.0.0.0:8000");
    }
  }

}
