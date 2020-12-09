using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Castle.Core.Logging;
using E_Commerce.Configurations;
using E_Commerce.Data;
using E_Commerce.Extensions;
using E_Commerce.Http;
using E_Commerce.Http.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using ILogger = Castle.Core.Logging.ILogger;

namespace E_Commerce
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(l => l.AddConsole());

            services.AddControllers().AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            
            services.AddDbContext<ApplicationDbContext>(op => 
                op.UseSqlServer(Configuration.GetConnectionString("DB"))
                    .UseLazyLoadingProxies()
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            );
            services.Configure<JWTSettings>(Configuration.GetSection("JWT"));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory =
                    context => new BadRequestObjectResult(new
                    {
                        
                        Errors = context.ModelState.Where(m => context.ModelState[m.Key].Errors.Count > 0).Select(model => new ErrorModel
                        {
                            Errors = model.Value.Errors.Select(error => error.ErrorMessage).ToArray(),
                            Field = model.Key
                        }).ToArray()
                    });
            });
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "E Commerce API", 
                    Version = "v1" 
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        RequireAudience = true,
                        IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"])),
                        ValidIssuer = Configuration["JWT:CurrentIssuer"],
                        ValidAudience = Configuration["JWT:CurrentAudience"],
                        
                    };
                });
            services.AddMyServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler(a => 
                    a.Run(async context =>
                {
                    var e = context.Features.Get<IExceptionHandlerFeature>().Error;
                    logger.LogError(message: $"Exception throw in :{e.StackTrace} says : {e.Message}", exception: e);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        ServerError = e.Message
                    }));
                    return;
                }));
            }
            else
            {
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    var e = context.Features.Get<IExceptionHandlerFeature>().Error;
                    logger.LogError(message: $"Exception throw in : {e.Source} says : {e.Message}", exception: e);
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        Error = e.Message
                    }));
                }));
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(cors => cors.AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint("/swagger/v1/swagger.json","E Commerce");
            });
            app.UseWhen(
                context => context.User.GetUserId().HasValue && context.User.GetUserRole().HasValue,
                a => a.UseVerifyTokenNotRevoked()
            );
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/seed", async context =>
                {
                    
                });
                endpoints.MapControllers();
            });
        }
    }
}