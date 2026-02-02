namespace Iyzico_Stripe_Strategy.Extension
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwaggerGenAdvanced(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("MyToken", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "X-MyToken",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "MyToken",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "MyToken token"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "MyToken"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }
    }
}