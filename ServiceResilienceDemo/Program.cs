using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.RateLimiting;
using Polly.Retry;
using ServiceResilienceDemo;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<WeatherService>();

builder.Services.AddSingleton<WeatherService>();

builder.Services.AddResiliencePipeline("default", x =>
{
    x.AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        Delay = TimeSpan.FromSeconds(2),
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    })

    .AddTimeout(TimeSpan.FromSeconds(30))

    .AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions
    {
        SamplingDuration = TimeSpan.FromSeconds(30),
        MinimumThroughput = 10,
        BreakDuration = TimeSpan.FromSeconds(15),
        FailureRatio = 0.5
    })

    .AddConcurrencyLimiter(new ConcurrencyLimiterOptions
    {
        PermitLimit = 10,
        QueueLimit = 20,
        QueueProcessingOrder = QueueProcessingOrder.NewestFirst
    })
    
    .AddRateLimiter(new RateLimiterStrategyOptions
    {
        DefaultRateLimiterOptions = new ConcurrencyLimiterOptions
        {
            PermitLimit = 10,
            QueueLimit = 20
        },        
        Name = "Test",
        RateLimiter = null
    });            
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://github.com/"), name: "GitHub", failureStatus: HealthStatus.Degraded);

builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("Health Checks Api", "/health");
}).AddInMemoryStorage();

//builder.Services.AddHealthChecks().AddCheck<SampleHealthCheck>("Sample");


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();

