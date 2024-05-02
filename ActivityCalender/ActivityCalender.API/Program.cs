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

                    NameClaimType = ClaimTypes.NameIdentifier,
                };

                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var userService = context.HttpContext.RequestServices.GetRequiredService<IKullaniciServisi>();
                //        var user = userService.KullaniciGetir(context.Principal.Identity.Name);
                //        if (user == null)
                //        {
                //            // return unauthorized if user no longer exists
                //            context.Fail("Unauthorized");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                //options.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal.Identity;

                //        if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
                //        {
                //            try
                //            {
                //                if (claimsIdentity.HasClaim(x => x.Type == "kullaniciID"))

                //                    claimsIdentity.AddClaim(
                //                        new Claim(
                //                            ClaimTypes.Name, claimsIdentity.FindFirst(x => x.Type == "kullaniciID").Value
                //                        )
                //                    );

                //                else
                //                    throw new Exception("Kullanýcý adý hatasý");

                //                var userService = context.HttpContext.RequestServices.GetRequiredService<IKullaniciServisi>();
                //                var kullaniciBilgi = userService.KullaniciGetir(context.Principal.Identity.Name);

                //                if (kullaniciBilgi == null)
                //                    throw new Exception("Kullanýcý hatasý");
                //            }
                //            catch (Exception)
                //            {

                //                throw;
                //            }
                //        }
                //        //var accessToken = context.Request.Query["access_token"];
                //        //var path = context.HttpContext.Request.Path;
                //        //if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/message")))
                //        //{
                //        //    context.Token = accessToken;
                //        //}
                //        return Task.CompletedTask;
                //    }
                //};
                //options.Events = new JwtBearerEvents();


                //options.Events.OnTokenValidated = async context =>
                //{
                //    //ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal.Identity;

                //    if (context.Principal.Identity != null && context.Principal.Identity.IsAuthenticated)
                //    {
                //        ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal.Identity;

                //        if (claimsIdentity.HasClaim(x => x.Type == "kullaniciID"))
                //        {
                //            var kullaniciIdClaim = claimsIdentity.FindFirst(x => x.Type == "kullaniciID");
                //            if (kullaniciIdClaim != null)
                //            {
                //                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, kullaniciIdClaim.Value));
                //            }
                //            else
                //            {
                //              //  throw new Exception("kullaniciID claim not found");
                //            }
                //        }
                //        else
                //        {
                //            //throw new Exception("kullaniciID claim not found");
                //        }

                //        var userName = context.Principal.Identity.Name;
                //        if (userName != null)
                //        {
                //            var userService = context.HttpContext.RequestServices.GetRequiredService<IKullaniciServisi>();
                //            var kullaniciBilgi = userService.KullaniciGetir(userName);

                //            if (kullaniciBilgi == null)
                //            {
                //            //    throw new Exception("Kullanýcý hatasý");
                //            }
                //        }
                //        else
                //        {
                //          //  throw new Exception("User name not found");
                //        }
                //    }
                //    else
                //    {
                //        //throw new Exception("User not authenticated");
                //    }


                //};
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