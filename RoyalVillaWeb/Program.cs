using RoyalVillaWeb.Services.IServices;
using RoyalVillaWeb.Services;
using Microsoft.Extensions.Configuration;
using RoyalVilla.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Add Automapper VillaDTO to Villa
builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
    o.CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options=>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath ="/auth/login";
    options.AccessDeniedPath = "/auth/accessdenied";
})
;

//Add HTTP Client
builder.Services.AddHttpClient("RoyalVillaAPI",client =>
{
    var villaAPIUrl =builder.Configuration.GetValue<string>("ServiceUrls:VillaAPI");
    client.BaseAddress = new Uri(villaAPIUrl ?? string.Empty);
    client.DefaultRequestHeaders.Add("Accept","application/json");

});

builder.Services.AddScoped<IVillaServices, VillaServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
