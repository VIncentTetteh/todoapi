using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoAPI.Auth;
using TodoAPI.Data;
using TodoAPI.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});


// Add database context
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });


builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable Swagger middleware only in Development
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
