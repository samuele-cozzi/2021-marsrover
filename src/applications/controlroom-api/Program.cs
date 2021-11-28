var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services
    .AddApi(builder.Configuration)
    .AddConfiguration(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddSwagger(builder.Configuration)
    .AddHangfireServices(builder.Configuration)
    .AddEventFlow(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.WithOrigins("*")
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseHttpsRedirection();
app.UseRouting();
app.UseHangfireDashboard();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();
});

app.MapGet("/api/rover/position", (RoverServices services) => { return services.Get(); })
    .WithName("GetRoverPositionsHistory");

app.MapGet("/api/rover/position/last", (RoverServices services) => { return services.GetLastPosition(); })
    .WithName("GetRoverPosition");

app.MapGet("/api/rover/position/landing", (RoverServices services) => { return services.GetLandingPosition(); })
    .WithName("GetRoverLandingPosition");

app.MapPost("/api/rover/takeoff", (RoverServices services) => { return services.TakeOff(); })
    .WithName("RoverTakeOff");

app.MapPost("/api/rover/move", (RoverServices services, string[] moves) => { return services.Move(moves); })
    .WithName("RoverMove");

app.MapPost("/api/rover/explore", (RoverServices services) => { return services.Explore(); })
    .WithName("RoverExplore");

app.Run();