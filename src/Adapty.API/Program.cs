using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Necessário para JWT
using Microsoft.IdentityModel.Tokens; // Necessário para assinar o token
using Microsoft.OpenApi.Models; // Necessário para o Swagger com Auth
using System.Text;
using Adapty.API.Data; // Seu DbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados (MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<Adapty.API.Services.SpacedRepetitionService>();
builder.Services.AddScoped<Adapty.API.Services.AuthService>();
builder.Services.AddScoped<Adapty.API.Services.DeckService>();
builder.Services.AddScoped<Adapty.API.Services.CardService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// --- MUDANÇA 1: Configurar Autenticação JWT ---
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("JWT Key is not configured. Please set 'Jwt:Key' in appsettings.");
var key = Encoding.UTF8.GetBytes(jwtKey); // Pega a chave do appsettings
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Em dev é false
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Simplificado para o projeto acadêmico
        ValidateAudience = false
    };
});

// 2. Adicionar Controllers
builder.Services.AddControllers();

// --- MUDANÇA 2: Configurar Swagger com cadeado de Auth ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT assim: Bearer seu_token_aqui"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 4. Configurar Pipeline de Requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// --- MUDANÇA 3: Ordem correta dos Middlewares ---
app.UseAuthentication(); // <-- OBRIGATÓRIO vir antes do Authorization
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Aplica migrações pendentes
}

app.Run();