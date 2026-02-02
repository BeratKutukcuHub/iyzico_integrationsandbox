
using Iyzico_Stripe_Strategy;
using Iyzico_Stripe_Strategy.Extension;
using Iyzico_Stripe_Strategy.Middlewares;
using Iyzico_Stripe_Strategy.Options;
using Iyzico_Stripe_Strategy.UseCase;
using Microsoft.AspNetCore.Authentication;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("My Scheme").
AddScheme<AuthenticationSchemeOptions, CustomAuthentication>("My Scheme", _ => {});
builder.Services.AddAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<IyzicoOption>(builder.Configuration.GetSection("IYZICO"));
builder.Services.Configure<MongoOption>(builder.Configuration.GetSection("MONGO"));
builder.Services.AddUseCase();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenAdvanced();
builder.Services.AddScoped<CorrelationMiddleware>();
builder.Services.AddScoped<ICurrentUserInformation, CurrentUserInformation>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CorrelationMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.MapControllers();
}

app.UseHttpsRedirection();

app.Run();

