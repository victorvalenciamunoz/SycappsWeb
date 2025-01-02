using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SycappsWeb.Server.Data;
using SycappsWeb.Server.Services;
using SycappsWeb.Server.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.AddSwaggerServices();
builder.AddAuthenticationServices();
builder.AddApiVersioningServices();
builder.AddDomainEventHandlers();
builder.AddHealthChecks();
builder.AddCustomIdentity();


builder.Services.AddScoped<IPoiService, PoiService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", name: "v1");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("Open");

app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.MapFallbackToFile("index.html");

app.Run();
