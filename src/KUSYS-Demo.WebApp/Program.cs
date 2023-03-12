using KUSYS_Demo.DataAccess;
using KUSYS_Demo.DataAccess.DataSeeder;
using KUSYS_Demo.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

var dataSeeder = app.Configuration.GetValue<bool>("AppOptions:DataSeeder");
if (dataSeeder)
    app.Services.DataSeed();

app.Run();
