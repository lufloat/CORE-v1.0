using CORE.Blazor.Components;
using CORE.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration["API_URL"]
             ?? "https://localhost:7022/";

var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]
                     ?? "https://localhost:7233";

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiUrl)
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins.Split(","))  // ← usa a variável agora
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();  // ← necessário para SignalR
    });
});

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AudioService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseCors();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();