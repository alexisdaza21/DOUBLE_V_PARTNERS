using Data;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using Services.Deuda;
using Utils;

namespace ApiDeudas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //permitir todos los origenes
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                 );
            });

            services.AddDbContext<ContextPostgres>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("postgres")));

            services.AddTransient<IUsuarios, UsuarioServices>();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JWT:ClaveSecreta"])
                    )
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    //Title = ".:Ecopetrol e-salud:.",
                    Title = ".:Prueba -DOUBLE V PARTNERS- Alexis Daza:.",
                    Version = "v1",
                    Description= "Esta API proporciona endpoints para administrar y gestionar deudas entre amigos."
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Autorition header \r\n\r\n 'Bearer' [spacio] y escribe tu token. \r\n\r\n ajemplo \"Bearer ***********\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"

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
                                 },
                                 Scheme = "oauth2",
                                 Name = "Bearer",
                                 In = ParameterLocation.Header

                             },
                             new List<string>()
                     }
                 });


            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (Configuration.GetSection("viewSwagger").Get<bool>())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(Configuration.GetSection("urlSuagger").Get<string>(), "ApiDeudas v1"));
            }

            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
