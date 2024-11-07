using Microsoft.OpenApi.Models;
using WebApplication1;
using WebApplication1.Models.Seeding;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
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
                }
            },
            new string[]{ }
        }
    });
});

builder.Services
    .AddInjects(builder.Configuration)
    .AddControllersWithViews();

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountsSeeder.SeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
    
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseSwaggerUI(c => {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Name of Your API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/

app.MapControllers();

app.Run();