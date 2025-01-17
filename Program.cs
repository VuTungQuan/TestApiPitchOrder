using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestApiPitchOrder.Data;
using TestApiPitchOrder;
using TestApiPitchOrder.Services;
using System.Reflection;
using TestApiPitchOrder.Data;
using TestApiPitchOrder.Repository;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext for postgresql
//builder.Services.AddDbContext<PitchOrderDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add DbContext for SQlServer
builder.Services.AddDbContext<PitchOrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db")).EnableSensitiveDataLogging());

builder.Services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                 .AllowAnyHeader()
    )
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.Api", Version = "v1", Description = "Api DEMO" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
});
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
