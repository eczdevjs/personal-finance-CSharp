using Microsoft.EntityFrameworkCore;
using PersonalFinanceMng.Application.Extensions;
using PersonalFinanceMng.Infrastructure;
using Serilog;
using Serilog.Formatting.Compact;

try
{
    var builder = WebApplication.CreateBuilder(args);
    //logger
    builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
    // Registra os controllers e documentação
    builder.Services.AddControllers();

    // 💡 CORREÇÃO 1: Adicionados os serviços do Swagger no Container de DI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Injeção de dependência das camadas
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);


        if (context.HostingEnvironment.IsProduction())
        {
            configuration.WriteTo.Console(new CompactJsonFormatter());
        }
        else
        {
            configuration.WriteTo.Console();
        }

    }
);

    var app = builder.Build();


    app.UseSerilogRequestLogging();

    // Teste de conexão temporário
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<PersonalFinanceMng.Infrastructure.Database.Context.AppDbContext>();

            if (context.Database.CanConnect())
            {
                Console.WriteLine("✅ Conexão com o SQL Server realizada com sucesso!");
            }
            else
            {
                Console.WriteLine("❌ Não foi possível conectar ao banco de dados.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine($"❌ ERRO DETALHADO DE CONEXÃO: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"🔍 EXCEÇÃO INTERNA: {ex.InnerException.Message}");
            }
            Console.WriteLine("==================================================");
        }
    }

    // Configurando Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // 💡 CORREÇÃO 2: Corrigido o nome do arquivo para swagger.json (com 2 'g's)
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Personal Finance App v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    Log.Information("Iniciando a aplicação...Logger working");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("==================================================");
    Console.WriteLine($"💥 FALHA NA INICIALIZAÇÃO DA APLICAÇÃO:");
    Console.WriteLine(ex.ToString());
    Console.WriteLine("==================================================");
    throw;
}
finally
{
    Log.CloseAndFlush();
}