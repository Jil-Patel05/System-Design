using Webapi.Helpers;
using Webapi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ICookieService, CookieHelper>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((options) =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecret"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = (context) =>
        {
            // Func<context,Task> -> Which expects Task to be returned
            string? CookieName = builder.Configuration["Jwt:CookieName"];
            if (CookieName != null && context.Request.Cookies.TryGetValue(CookieName, out string? cookieName))
            {
                context.Token = cookieName;
            }
            // Or directly Use await which returns Task on completion
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization((option) =>
{
    option.AddPolicy("adminPolicy", policy =>
    {
        policy.RequireClaim("role", "admin"); // Only users with role "Admin" can access
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Routes are matched using this middleware
app.UseRouting();

// Authentication and authorization are performed using this middleware
app.UseAuthentication();
app.UseAuthorization();

// Controller and related method are assigned to HTTP request
app.MapControllers();

// Middlware are executed now your controller handler runs code
app.Run();
