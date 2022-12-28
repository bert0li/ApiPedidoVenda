using ApiPedidoVenda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddNewtonsoftJson(json => json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextoPedidoVenda>(o => o.UseSqlite(connectionString).LogTo(Console.Write));

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ApiPedidoVenda",
        Description = "API para estudo pessoal.",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "Bertoli",
            Email = "bertoli@webeasy.com.br"
        }
    });    
});

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPedidoVenda v1"));

app.Run();