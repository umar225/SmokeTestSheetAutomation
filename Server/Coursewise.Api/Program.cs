using Coursewise.Api.Extensions;
using Coursewise.Module.Extensions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddCors(options =>
{
    
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                          builder.WithOrigins("https://app.d.getboardwise.com",
                              "https://app.s.getboardwise.com",
                              "https://app.getboardwise.com",
                              "http://localhost:3000",
                              "https://localhost:3000");
                          builder.SetPreflightMaxAge(TimeSpan.FromHours(1));

                      });
});
builder.Services.AddControllersWithViews(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
    ).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;    
});
builder.SetBuilder();
var app = builder.Build();


app.Services.UseAutoMigration(app.Environment.EnvironmentName, app.Configuration).GetAwaiter().GetResult();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var logger = services.GetRequiredService<Coursewise.Logging.ICoursewiseLogger<Coursewise.Api.Extensions.CoursewiseException>>();
    app.ConfigureExceptionHandler(logger);    
}

app.MapControllers();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.Run();
