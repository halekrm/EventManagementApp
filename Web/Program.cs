using Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration(builder.Configuration);
builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PublicEvent}/{action=Index}/{id?}");

app.Run();