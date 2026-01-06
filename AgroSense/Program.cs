using AgroSense.Repositories.Korisnici;
using AgroSense.Repositories.Merenja;
using AgroSense.Repositories.ProizvodneJedinice;
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
builder.Services.AddScoped<ProizvodneJediniceRepository>();
builder.Services.AddScoped<KorisnikRepository>();

// Services
builder.Services.AddScoped<SenzorService>();
builder.Services.AddScoped<MerenjaService>();
builder.Services.AddScoped<ProizvodneJediniceService>();
builder.Services.AddScoped<KorisnikService>();

// **CORS**
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();
