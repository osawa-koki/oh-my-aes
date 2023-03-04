
public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.MapGet("/cipher/encrypt/{bit}", ECB.Encrypt).WithName("Encrypt").WithOpenApi();
    app.MapGet("/cipher/decrypt/{bit}", ECB.Decrypt).WithName("Decrypt").WithOpenApi();

    app.Run("http://localhost:8000");
  }
}
