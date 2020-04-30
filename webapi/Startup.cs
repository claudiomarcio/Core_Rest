extern alias signed;

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Infra.EntityConfiguration;
using Infra.Repositories;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Rewrite;
using webapi.Controllers;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc;
using Domain.DTO;
using signed::StackExchange.Redis;
using Infra.Interfaces.Repositories;

namespace webapi
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

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("webapi")));
            services.AddScoped<ApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<ISistemaRepository, SistemaRepository>();
            services.AddTransient<IGrupoEmpresaRepository, GrupoEmpresaRepository>();
            services.AddTransient<GrupoEmpresa, GrupoEmpresa>();
            services.AddTransient<Sistema, Sistema>();
            services.AddTransient<Empresa, Empresa>();
            services.AddTransient<Usuario, Usuario>();

        
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(Configuration.GetSection("TokenConfiguration")).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
               .RequireAuthenticatedUser().Build());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            //services.Configure<MvcOptions>(options => {
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            //});

            services.AddMvc();
            services.AddCors();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware();
            if (env.IsDevelopment())

            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors(option => option.WithOrigins("https://gametripfront.azurewebsites.net"));
            app.UseCors(builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseCors("AllowAll");
            app.UseMvc();


            //Documentação Swagger
            //app.UseSwagger();
            //app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Gametrip"); });

            //var option = new RewriteOptions();
            //option.AddRedirect("^$", "swagger");
            //app.UseRewriter(option);

        }
    }
}