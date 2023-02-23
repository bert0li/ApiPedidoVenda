using ApiPedidoVenda.Data;
using ApiPedidoVenda.Models;
using ApiPedidoVenda.Repositorio;
using ApiPedidoVenda.Repositorio.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigurarMvc(builder);
ConfigurarServicos(builder);

if (builder.Environment.IsDevelopment())
    ConfigurarSwagger(builder);

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPedidoVenda v1"));
}

app.Run();

void ConfigurarMvc(WebApplicationBuilder builder)
{
    builder.Services.AddControllers()
                    .AddNewtonsoftJson(json => { json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; })
                    .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; });
}

void ConfigurarServicos(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ContextoPedidoVenda>(o => o.UseSqlite(connectionString).LogTo(Console.WriteLine));

    builder.Services.AddTransient<IRepositorio<Cliente>, RepositorioCliente>();
    builder.Services.AddTransient<IRepositorio<Produto>, RepositorioProduto>();
    builder.Services.AddTransient<IRepositorio<Pedido>, RepositorioPedido>();
}

void ConfigurarSwagger(WebApplicationBuilder builder)
{
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
}