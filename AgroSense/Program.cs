using AgroSense.Repositories.Merenja;
using AgroSense.Repositories.Senzor;
using AgroSense.Services;
using Cassandra;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cassandra session
builder.Services.AddSingleton<Cassandra.ISession>(sp =>
{
    var cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1")
        .WithPort(9042)
        .Build();

    return cluster.Connect("agrosense");
});

// Repositories
builder.Services.AddScoped<MerenjaPoLokacijiRepository>();
builder.Services.AddScoped<MerenjaPoDanuRepository>();
builder.Services.AddScoped<MerenjaPoslednjaVrednostRepository>();
builder.Services.AddScoped<SenzorRepository>();

// Service
builder.Services.AddScoped<SenzorService>();
builder.Services.AddScoped<MerenjaService>();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
