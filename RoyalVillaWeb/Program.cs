using RoyalVillaWeb.Services.IServices;
using RoyalVillaWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient factory
builder.Services.AddHttpClient<IVillaServices, VillaServices>();
builder.Services.AddScoped<IVillaServices, VillaServices>();
builder.Services.AddScoped<IBaseServices, BaseServices>();

// Configure named HttpClient for RoyalVillaAPI
builder.Services.AddHttpClient("RoyalVillaAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServiceUrls:VillaAPI"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
