﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace library77.Models
{
    namespace TokenApp
    {
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                // укзывает, будет ли валидироваться издатель при валидации токена
                                ValidateIssuer = true,
                                // строка, представляющая издателя
                                ValidIssuer = AuthOptions.ISSUER,

                                // будет ли валидироваться потребитель токена
                                ValidateAudience = true,
                                // установка потребителя токена
                                ValidAudience = AuthOptions.AUDIENCE,
                                // будет ли валидироваться время существования
                                ValidateLifetime = true,

                                // установка ключа безопасности
                                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                                // валидация ключа безопасности
                                ValidateIssuerSigningKey = true,
                            };
                        });
                services.AddControllersWithViews();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseDeveloperExceptionPage();

                app.UseDefaultFiles();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                });
            }
        }
    }
}
