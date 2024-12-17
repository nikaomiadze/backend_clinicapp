using clinic.auth;
using clinic.packpages;
using clinic.services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using clinic.filters;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IPKG_USER, PKG_USER>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<IjwtManager, JWTmanager>();
        builder.Services.AddScoped<GlobalExceptionFilter>();
        builder.Services.AddScoped<IPKG_LOG, PKG_LOGS>();
        builder.Services.AddScoped<IPKG_ADMIN, PKG_ADMIN>();
        builder.Services.AddScoped<IPKG_CATEGORY, PKG_CATEGORY>();
        builder.Services.AddScoped<IPKG_DOCTOR, PKG_DOCTOR>();


        builder.Services.AddCors(option =>
        {
            option.AddPolicy("AllowAllCors", config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();
            });
        });
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("roleID", "3")); // Only allow role_id = 3
        });



        var app = builder.Build();
        app.UseCors(builder =>
         builder.WithOrigins("http://localhost:4200") // Allow Angular app
           .AllowAnyHeader()
           .AllowAnyMethod());
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseCors("AllowAllCors");


        app.MapControllers();

        app.Run();
    }
}
