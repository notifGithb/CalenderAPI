using ActivityCalender.API.Configurators;
using ActivityCalender.Business.Etkinlikler;
using ActivityCalender.Business.Kullanicilar;
using ActivityCalender.Business.OturumYonetimi;
using ActivityCalender.Business.OturumYonetimi.JWT;
using ActivityCalender.DataAccess;
using ActivityCalender.DataAccess.Etkinlikler;
using ActivityCalender.DataAccess.Kullanicilar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

SwaggerConfigurator.ConfigureSwaggerGen(builder.Services);

builder.Services.AddScoped<IOturumYonetimi, OturumYonetimi>();
builder.Services.AddScoped<IJwtServisi, JwtServisi>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IKullaniciServisi, KullaniciServisi>();
builder.Services.AddScoped<IEtkinlikRepository, EtkinlikRepository>();
builder.Services.AddScoped<IEtkinlikServisi, EtkinlikServisi>();
builder.Services.AddScoped<IKullaniciEtkinlikRepositroy, KullaniciEtkinlikRepositroy>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? string.Empty)
                    ),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        expires != null ? expires > DateTime.UtcNow : false,
                   
                    NameClaimType = ClaimTypes.Name
                };

                
            });

builder.Services.AddDbContext<ActivityCalenderContext>();


builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true)));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
