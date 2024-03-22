using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ICommandRepository, CommandRepository>();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseInMemoryDatabase("InMem"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<IPlaformDataClient, PlaformDataClient>();

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

DatabaseInitializer.Seed(app);

app.Run();
