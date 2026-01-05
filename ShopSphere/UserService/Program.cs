
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UserService.Data;
using UserService.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(o =>
    o.UseInMemoryDatabase("UsersDb"));

builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddHostedService<UserEventConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
