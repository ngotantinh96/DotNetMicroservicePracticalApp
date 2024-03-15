using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if(builder.Environment.IsProduction())
{
    Console.WriteLine("---> Use MSSQL database....");
    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn"))
    );
}
else 
{
    Console.WriteLine("---> Use In Memory database....");
    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseInMemoryDatabase("InMem")
    );
}
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

DatabaseInitializer.Seed(app, app.Environment.IsProduction());

app.Run();
